using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.Location;
using Core.Application.Interfaces.MISReport;
using Core.Domain.Location;
using Core.Domain.MISReport;
using Core.Domain.Temporary;
using Dapper.Oracle;
using DocumentFormat.OpenXml.EMMA;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using Shared.DTOs.MISReport;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MISReport
{
    public class MisReportRepository : IMisReportRepository
    {
        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepositry;
        private readonly ILocationRepository _locationRepoistory;

        private readonly IMapper _mapper;
        public MisReportRepository(IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepositry, ILocationRepository locationRepoistory, IMapper mapper)
        {
            _dbConfigRepo = dbConfigRepo;
            _commRepositry = commRepositry;
            _locationRepoistory = locationRepoistory;
            _mapper = mapper;
        }
        #region All Customer Arrear Details

        #region Get All Customer Customer By Multiple Db and Location

        public List<CustomerArrear> AllCustomerArrearDataList { get; set; } = new List<CustomerArrear>();
        public async Task<List<CustomerArrear>> AllCustomerArrearDetails(string[] dbCodes, string[] locationCodes, int fromAmount, int toAmount, string tariff, string billGroupId, string bookId, int noOfMonth, string billMonth, int reportTypeId, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, string orderTypeId, int userId, int roleId)
        {
            try
            {
                List<Task> listOfTasks = new List<Task>();
                if (dbCodes.Length > 0 && locationCodes != null)
                {
                    string locCode = (locationCodes != null) ? "'" + string.Join("', '", locationCodes) + "'" : " ";
                    foreach (var code in dbCodes)
                    {
                        try
                        {
                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                            listOfTasks.Add(Task.Run(() => GetAllCustomerArrearDataAsync(locCode, fromAmount, toAmount, tariff, billGroupId, bookId, noOfMonth, billMonth, reportTypeId, isAll, isPrincipal, isVAT, isLPS, orderTypeId, connectionString)));
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            continue;
                        }
                    }
                }
                else if (dbCodes.Length > 0 && locationCodes == null)
                {
                    foreach (var center in dbCodes)
                    {
                        try
                        {

                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(center); // Getting database info by code.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                            if (connectionString == null) throw new ArgumentNullException();

                            var locationString = " ";
                            var locations = await _locationRepoistory.GetLocationByDBMIdAsync(center, userId, roleId);

                            foreach (var res in locations)
                            {
                                if (res.ID != 0)
                                {

                                    locationString += "'" + res.CODE + "'" + ",";
                                }
                            }

                            listOfTasks.Add(Task.Run(() => GetAllCustomerArrearDataAsync(locationString.TrimEnd(','), fromAmount, toAmount, tariff, billGroupId, bookId, noOfMonth, billMonth, reportTypeId, isAll, isPrincipal, isVAT, isLPS, orderTypeId, connectionString)));

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.Message);
                            continue;
                        }
                    }
                }


                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return AllCustomerArrearDataList.ToList();
        }

        #endregion

        private async Task GetAllCustomerArrearDataAsync(string locationCodes, int fromAmount, int toAmount, string tariff, string billGroupId, string bookId, int noOfMonth, string billMonth, int beportTypeId, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, string orderTypeId, string connectionString)
        {



            using IDbConnection conn = new OracleConnection(connectionString);
            try
            {
                string sQuery = "";
                string _billcyclecode = billMonth;
                
               /* bool locationCodeExist = (locationCodes != null) ? true : false;*/ // Setting existence of location code depending on there being a value or not.
                bool billGroupExist = (billGroupId != "0") ? true : false;
                bool tariffExist = (tariff.ToUpper() != "ALL") ? true : false;
                bool bookExist = (bookId != "0") ? true : false;
                bool NOMExist = (noOfMonth != 0) ? true : false;
                bool billCycleCodeExist = (billMonth != "") ? true : false;
                bool fromAmountExist = (fromAmount > 0) ? true : false;
                bool toAmountExist = (toAmount > 0) ? true : false;

                // order By conditions
                bool orderByBillCycleExist = (orderTypeId == "billcycle") ? true : false; // billcycle
                bool orderByExistBillgroup = (orderTypeId == "billgroup") ? true : false; // billgroup
                bool orderByExistBook = (orderTypeId == "book") ? true : false; //book
                bool orderByExistTariff = (orderTypeId == "tariff") ? true : false; //tariff

                string conditions = !isAll ? "and i.principal_arrear " + (isPrincipal ? ">" : "=") + @" 0
                    and i.lps_arrear " + (isLPS ? ">" : "=") + @" 0
                    and i.vat_arrear " + (isVAT ? ">" : "=") + @" 0" : "";

                sQuery = @$"select a.location_code loc,
                    g.DESCR office,
                    substr(a.AREA_CODE,4,2) bg,
                    substr(a.AREA_CODE,1,3) b_k,
                    a.CUSTOMER_NUM||a.CHECK_DIGIT con_no,
                    a.WALKING_SEQUENCE || ' /' wlk_or,
                    a.CONS_EXTG_NUM pv_ac,
                    substr(a.CUSTOMER_NAME,1,35) name,
                    a.F_H_NAME Father_Name,
                    e.addr_descr1 || e.addr_descr2 || e.addr_descr3 AS addr,
                    f.MONTHLY_LIKELY_CONS likely,
                    cm.usage_category_code as tariff,
                    i.NOM,
                    i.Principal_Arrear,
                    i.LPS_arrear,
                    i.vat_Arrear,
                    i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear total_arrear,
                    k.bill_cycle_code,
                    CASE METER_STATUS WHEN '1' THEN TEMP_DISCON_DATE
                   WHEN '3' THEN PERM_DISCON_DATE
                   ELSE PERM_DISCON_DATE END as DISC_DATE,
                    decode(M.METER_STATUS,'2','Reg.','1','Temp. Dis.','3','Perm. Dis.') STATUS
                    from ebc.bc_customers a,
                    ebc.bc_customer_category c,
                    ebc.bc_category_master cm,
                    ebc.bc_customer_addr e,
                    ebc.bc_monthly_likely f,
                    ebc.bc_location_master g,
                    (select cust_id,count(1) NOM,sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) Principal_Arrear,
                    sum(LPS_AMT+LPS_ADJ-LPS_APPL) LPS_arrear,
                    sum(VAT_AMT+VAT_ADJ-VAT_APPL) vat_Arrear from bc_invoice_hdr where  bill_cycle_code <= :Billcyclecode
                    group by cust_id) i,
                     (select DISTINCT cust_id,max(bill_cycle_code) as BILL_CYCLE_CODE from bc_invoice_hdr where bill_cycle_code <= :Billcyclecode
                    group by cust_id) k,(select distinct M.CUST_ID , max(M.CONNECTION_ID) AS CONNECTION_ID  from EBC.BC_CUSTOMER_METER m  group by M.CUST_ID ) MT,
                    EBC.BC_CUSTOMER_METER M
                    where  a.cust_id=e.cust_id
                    and  a.cust_id=f.cust_id
                    and a.cust_id=i.cust_id
                    and c.CUST_ID=a.CUST_ID
                    and c.cat_id=cm.category_id and c.exp_date is null
					AND  cm.usage_category_code<>'I'
                    and a.LOCATION_CODE=g.LOCATION_CODE
                    and f.EXP_DATE is null and e.addr_type='B' " + conditions +

                    @$" and e.ADDR_EXP_DATE is null and i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear>1
                    AND A.CUST_ID = MT.CUST_ID
                    AND MT.CONNECTION_ID = M.CONNECTION_ID
                    --and a.customer_status_code<>'D'
                    and i.cust_id=k.cust_id
                   -- AND BI.INVOICE_NUM is not null
                     AND A.LOCATION_CODE in ({locationCodes}) " 
                      + (billCycleCodeExist ? @" AND  k.bill_cycle_code <=:BillMonth " : "") + (tariffExist ? @" AND  cm.usage_category_code =:Tariff  " : "") + (fromAmountExist ? @"AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear >=:FromAmount" : "") + (toAmountExist ? @" AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear <=:ToAmount " : "") + (billGroupExist ? @" AND  substr(a.AREA_CODE,4,2)=:BillGroupId " : "")
                      + (bookExist ? @" AND  substr(a.AREA_CODE,1,3)=:BookId  " : "") + (NOMExist ? @" AND  i.NOM >=:NoOfMonth " : "")
                      + @" order by  " + (orderByBillCycleExist ? @" k.bill_cycle_code ASC " : "") + (orderByExistBillgroup ? @" substr(a.AREA_CODE,4,2) " : "") + (orderByExistBook ? @" substr(a.AREA_CODE,1,3) " : "") + (orderByExistTariff ? @" cm.usage_category_code " : "");


                var result = await conn.QueryAsync<CustomerArrear>(sQuery, new
                {
                    BillMonth = billMonth,
                    Tariff = tariff,
                    FromAmount = Convert.ToInt32(fromAmount),
                    ToAmount = Convert.ToInt32(toAmount),
                    BillGroupId = billGroupId,
                    BookId = bookId.ToString(),
                    NoOfMonth = Convert.ToInt32(noOfMonth),
                    Billcyclecode = _billcyclecode.ToString(),
                });

                AllCustomerArrearDataList.AddRange(result);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw ex;
            }
        }


        public Task<List<RegularCustomerArrear>> GetArrearRegularCustomerData(int[] dbIds, int[] locationIds, string BillMonth, int ConnStatusId, int ArrearFrom, int ArrrearTo, int TariffId, string Tariff, string BillGroupId, string BookId, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, string OrderTypeId, int UId, int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Domain.Location.Locations> GetLocationByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion  All Customer Arrear Details

        #region All Customer Arrear Summary

        #region All customer Arrear summary location wise
        public List<AllCustoemrArrearSummary> AllCustomerArrearSummaryDataList { get; set; } = new List<AllCustoemrArrearSummary>();
        public async Task<List<AllCustoemrArrearSummary>> AllCustomerArrearSummaryCenterWise(string billCycleCode, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, int userId, int roleId)
        {
            try
            {

                List<Task> listOfTasks = new List<Task>();
                var userDbs = await _dbConfigRepo.GetDatabaseByUserIdRoleId(userId, roleId);
                if (userDbs == null)
                    throw new ArgumentNullException();

                foreach (var dbItem in userDbs)
                {
                    try
                    {

                        string connectionString = _commRepositry.CreateConnectionString(dbItem.HOST, dbItem.PORT, dbItem.SERVICE_NAME, dbItem.USER_ID, dbItem.PASSWORD); // Getting connection string by database.

                        if (connectionString == null)
                            throw new ArgumentNullException();
                           var locations = await _locationRepoistory.GetLocationByDBMIdAsync(dbItem.CODE, userId, roleId);
                        if (locations == null)
                            throw new ArgumentNullException();

                        string locationString = "";
                        foreach (var res in locations)
                        {
                            if (res.ID != 0)
                                locationString += "'" + res.CODE + "'" + ",";
                        }
                        locationString = locationString.TrimEnd(',');
                        string dbName = dbItem.NAME;
                        int order = (int)dbItem.ORDER_NO;
                        listOfTasks.Add(Task.Run(() => ExecuteCenterWiseArrearQuery(connectionString, dbName, billCycleCode, isAll, isPrincipal, isVAT, isLPS, order, locationString)));

                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        continue;
                    }
                }



                Task.WaitAll(listOfTasks.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return AllCustomerArrearSummaryDataList.OrderBy(x => x.ORDER).ToList();
        }

        private async Task ExecuteCenterWiseArrearQuery(string connectionString, string dbName, string billCycleCode, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, int order, string locationCodeString)
        {
            try
            {
                using IDbConnection conn = new OracleConnection(connectionString); // Creating new connection with current connection string.

                string conditions = !isAll ? " " + (isPrincipal ? "and i.principal_arrear >" : "and i.principal_arrear =") + @" 0
                    and i.lps_arrear " + (isLPS ? ">" : "=") + @" 0
                    and i.vat_arrear " + (isVAT ? ">" : "=") + @" 0" : "";
                bool isLocationString = locationCodeString != "" ? true : false;

                string sQuery = @"SELECT '' as Center,
                    count(i.CUST_ID ) as NOC,
                    SUM(i.Principal_Arrear) AS ARR_PRIN,
                    SUM(i.LPS_arrear) AS ARR_LPS,
                    SUM(i.vat_Arrear) AS ARR_VAT,
                    SUM( i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear) AS TOTAL_BILL
                    from ebc.bc_customers a,
                    ebc.bc_location_master g,
                    (select cust_id,count(1) NOM,sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) Principal_Arrear,
                    sum(LPS_AMT+LPS_ADJ-LPS_APPL) LPS_arrear,
                    sum(VAT_AMT+VAT_ADJ-VAT_APPL) vat_Arrear from bc_invoice_hdr 
                    --where ((principal_AMT +principal_ADJ-principal_appl) + (LPS_AMT+LPS_ADJ-LPS_APPL) + (VAT_AMT+VAT_ADJ-VAT_APPL))>0 
                    where (principal_AMT +principal_ADJ-principal_appl)>0
                    and  bill_cycle_code <= :billCycleCode
                    group by cust_id) i
                    where  a.cust_id=i.cust_id
                    and a.LOCATION_CODE=g.LOCATION_CODE " +
                    (isLocationString ? @$" and a.LOCATION_CODE in ( {locationCodeString} ) " : " ") + conditions +
                    @" and i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear>1 " +
                    @" and a.customer_status_code<>'D'";

                var result = await conn.QueryFirstOrDefaultAsync<AllCustoemrArrearSummary>(sQuery, new
                {
                    billCycleCode
                });

                result.CENTER = dbName + " Computer Center";
                result.ORDER = order;
                AllCustomerArrearSummaryDataList.Add(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        #endregion  All customer Arrear summary location wise


        #region all custoemr Arrear summary location wise


        public async Task<List<AllCustoemrArrearSummary>> GetAllCustomerArrearLocationSummaryData(string dbCode, string billMonth, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, int userId, int roleId)
        {
            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by code.
            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

            if (connectionString == null)
                throw new NullReferenceException();

            var locations = await _locationRepoistory.GetLocationByDBMIdAsync(dbCode, userId, roleId);
            if (locations == null)
                throw new ArgumentNullException();

            string locationString = "";
            foreach (var res in locations)
            {
                if (res.ID != 0)
                    locationString += "'" + res.CODE + "'" + ",";
            }
            locationString = locationString.TrimEnd(',');
            using IDbConnection conn = new OracleConnection(connectionString);
            try
            {
                string conditions = !isAll ? "and i.principal_arrear " + (isPrincipal ? ">" : "=") + @" 0
                    and i.lps_arrear " + (isLPS ? ">" : "=") + @" 0
                    and i.vat_arrear " + (isVAT ? ">" : "=") + @" 0" : "";

                string sQuery = @"select a.location_code LOC,
                    g.DESCR OFFICE,
                    count(i.CUST_ID ) as NOC,
                    SUM(i.Principal_Arrear) AS ARR_PRIN,
                    SUM(i.LPS_arrear) AS ARR_LPS,
                    SUM(i.vat_Arrear) AS ARR_VAT,
                    SUM( i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear) AS TOTAL_BILL
                    from ebc.bc_customers a,
                    ebc.bc_customer_category c,
                    ebc.bc_category_master cm,
                    ebc.bc_location_master g,
                    (select cust_id,count(1) NOM,sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) Principal_Arrear,
                    sum(LPS_AMT+LPS_ADJ-LPS_APPL) LPS_arrear,
                    sum(VAT_AMT+VAT_ADJ-VAT_APPL) vat_Arrear from bc_invoice_hdr where   bill_cycle_code <=:billcyclecode
                    group by cust_id) i
                    where  a.cust_id=i.cust_id
                    and c.CUST_ID=a.CUST_ID
                    and c.cat_id=cm.category_id and c.exp_date is null
                    AND  cm.usage_category_code<>'I'
                    and a.LOCATION_CODE=g.LOCATION_CODE  " + $" and a.LOCATION_CODE in ( {locationString} ) " + conditions +
                    @" --and a.customer_status_code<>'D'
                     GROUP BY  a.location_code,g.DESCR
                     order by  a.location_code ";


                var result = await conn.QueryAsync<AllCustoemrArrearSummary>(sQuery, new { billcyclecode = billMonth });
                return result.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw ex;
            }

        }

      


        #endregion  all custoemr Arrear summary location wise

        #endregion All Customer Arrea Summary

        public async Task<List<CenterWiseArrearSummaryDTO>> GetArrearLocationSummaryInfoAsync(string CenterCode, string BillMonth, bool isAll, bool isPrincipal, bool isVat, bool isLPS, int userId, int roleId)
        {
            try
            {
                var db = await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(CenterCode); // Getting database info by id.
                string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

                
                if (connectionString == null)
                    return null;

                var locations = await _locationRepoistory.GetLocationByDBMIdAsync(CenterCode, userId, roleId);
                if (locations == null)
                    throw new ArgumentNullException();

                string locationString = "";
                foreach (var res in locations)
                {
                    if (res.ID != 0)
                        locationString += "'" + res.CODE + "'" + ",";
                }
                locationString = locationString.TrimEnd(',');
                using IDbConnection conn = new OracleConnection(connectionString); // Creating new connection with current connection string.
                try
                {

                    string conditions = !isAll ? " and (nvl(arr_adv_adj_prn,0)+ nvl(adjusted_prn, 0)) " + (isPrincipal ? " > " : "=") + @" 0
                    and (nvl(current_lps,0)+nvl(arr_adv_adj_lps,0)+nvl(adjusted_lps,0)) " + (isLPS ? ">" : "=") + @" 0
                    and (nvl(b.ARR_ADV_ADJ_VAT,0)+nvl(adjusted_vat,0)) " + (isVat ? ">" : "=") + @" 0" : "";

                    string sQuery = @"select  b.location_code Loc,
                    count(b.CUSTOMER_NUM ) as Noc,
                        f.DESCR Office,
                        b.bill_cycle_code Month,
                                      SUM(NVL (cons_kwh_sr, 0)
                                    + NVL (cons_kwh_ofpk, 0)
                                    + NVL (cons_kwh_pk, 0)
                                    + NVL (old_kwh_sr_cons, 0)
                                    + NVL (old_kwh_ofpk_cons, 0)
                                    + NVL (old_kwh_pk_cons, 0)
                                    + NVL (adjusted_cons, 0)
                                    + NVL (pfc_sr_cons, 0)
                                    + NVL (pfc_ofpk_cons, 0)
                                    + NVL (pfc_pk_cons, 0)
                                    + NVL (xf_loss_sr_cons, 0)
                                    + NVL (xf_loss_ofpk_cons, 0)
                                    + NVL (xf_loss_pk_cons, 0)
                                    + NVL (cons_kwh_mr1, 0)
                                    + NVL (cons_kwh_mr2, 0)
                                    + NVL (cons_kwh_mr3, 0)
                                    + NVL (cons_kwh_mr4, 0)
                                    + NVL (pfc_mr1_cons, 0)
                                    + NVL (pfc_mr2_cons, 0)
                                    + NVL (pfc_mr3_cons, 0)
                                    + NVL (pfc_mr4_cons, 0)
                                    + NVL (xf_loss_mr1_cons, 0)
                                    + NVL (xf_loss_mr2_cons, 0)
                                    + NVL (xf_loss_mr3_cons, 0)
                                    + NVL (xf_loss_mr4_cons, 0))
                                       BldUnit,
                        SUM(nvl(ENG_CHRG_SR,0)+
                        nvl(ENG_CHRG_OFPK,0)+ nvl(ENG_CHRG_PK,0)+
                        nvl(minimum_CHRG,0)+nvl(service_CHRG,0)+nvl(demand_CHRG,0)+
                        nvl(adjusted_prn,0)+nvl(pfc_charge,0)
                        +nvl(xf_loss_chrg,0)+nvl(xf_rent,0)
                        +nvl(eng_chrg_mr1,0)+nvl(eng_chrg_mr2,0)+nvl(eng_chrg_mr3,0)+nvl(eng_chrg_mr4,0))  CurPrin,
                        SUM(b.current_vat) CurVat,
                        SUM(nvl(arr_adv_adj_prn,0)
                        +nvl(adjusted_prn,0)) ArrPrin,
                        SUM(nvl(b.ARR_ADV_ADJ_VAT,0)+nvl(adjusted_vat,0)) ArrVat,
                        SUM(nvl(current_lps,0)+nvl(arr_adv_adj_lps,0)+nvl(adjusted_lps,0)) ArrLps,
                        SUM(b.TOTAL_BILL) TotalBill,
                        SUM(nvl(current_lps,0)) CurrentLps
                        from ebc.bc_bill_image b,
                        ebc.bc_location_master f
                         where  b.LOCATION_CODE=f.LOCATION_CODE
                        and nvl(arr_adv_adj_prn,0)+nvl(adjusted_prn,0) + (nvl(b.ARR_ADV_ADJ_VAT,0)+nvl(adjusted_vat,0)) + (nvl(current_lps,0)+nvl(arr_adv_adj_lps,0)+nvl(adjusted_lps,0)) > 0
                        and b.TARIFF<>'I'
                        and b.invoice_num is not null
                           AND  b.bill_cycle_code = :BillMonth " +
                         $" AND b.location_code in ( {locationString} ) " + conditions +
                        @"GROUP BY b.location_code, f.DESCR,b.bill_cycle_code
                        order by  b.location_code,ArrPrin desc";

                    var result = await conn.QueryAsync<CenterWiseArrearSummaryDTO>(sQuery, new
                    {
                        BillMonth = BillMonth,
                    });
                    return result.ToList();
                }
                catch (Exception)
                {
                    throw;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CenterWiseArrearSummaryDTO> CenterWiseArrearSummaryDataList { get; set; } = new List<CenterWiseArrearSummaryDTO>();

        public async Task<List<CenterWiseArrearSummaryDTO>> GetCenterWiseArrearSummary(string billCycleCode, bool isAll, bool isPrincipal, bool isVat, bool isLPS, int userId, int roleId)
        {
            try
            {
                List<Task> listOfTasks = new List<Task>();
                var userDbs = await _dbConfigRepo.GetDatabaseByUserIdRoleId(userId, roleId);
                foreach (var dbItem in userDbs) 
                {
                    try
                    {
                        string connectionString = _commRepositry.CreateConnectionString(dbItem.HOST, dbItem.PORT, dbItem.SERVICE_NAME, dbItem.USER_ID, dbItem.PASSWORD); // Getting Connection String by Database
                        if (connectionString == null)
                            throw new ArgumentException();
                        var locations = await _locationRepoistory.GetLocationByDBMIdAsync(dbItem.CODE, userId, roleId);
                        if (locations == null)
                            throw new ArgumentException();

                        string locationString = "";

                        foreach (var res in locations)
                        {
                            if (res.ID != 0)
                                locationString += "'" + res.CODE + "'" + ",";
                        }
                        locationString = locationString.TrimEnd(',');
                        string dbName = dbItem.NAME;
                        int order = dbItem.ORDER_NO;
                        //listOfTasks.Add(Task.Run(() => ExecuteGetCenterWiseArrearSummaryQuery(connectionString, dbName, billCycleCode, isAll, isPrincipal, isVat, isLPS, order, locationString)));
                        await ExecuteGetCenterWiseArrearSummaryQuery(connectionString, dbName, billCycleCode, isAll, isPrincipal, isVat, isLPS, order, locationString);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
            return CenterWiseArrearSummaryDataList.ToList();
        }

        private async Task ExecuteGetCenterWiseArrearSummaryQuery(string connectionString, string dbName, string billCycleCode, bool isAll, bool isPrincipal, bool isVat, bool isLPS, int order, string locationString) 
        {
            try
            {
                using IDbConnection conn = new OracleConnection(connectionString);
                string conditions = !isAll ? " and (nvl(arr_adv_adj_prn,0)+ nvl(adjusted_prn, 0)) " + (isPrincipal ? " > " : "=") + @" 0
                    and (nvl(current_lps,0)+nvl(arr_adv_adj_lps,0)+nvl(adjusted_lps,0)) " + (isLPS ? ">" : "=") + @" 0
                    and (nvl(b.ARR_ADV_ADJ_VAT,0)+nvl(adjusted_vat,0)) " + (isVat ? ">" : "=") + @" 0" : "";

                string sQuery = @"select '' as Center,
                            count(B.CUSTOMER_NUM ) as Noc,
                            b.bill_cycle_code Month,
                            SUM(NVL (cons_kwh_sr, 0)
                            + NVL (cons_kwh_ofpk, 0)
                            + NVL (cons_kwh_pk, 0)
                            + NVL (old_kwh_sr_cons, 0)
                            + NVL (old_kwh_ofpk_cons, 0)
                            + NVL (old_kwh_pk_cons, 0)
                            + NVL (adjusted_cons, 0)
                            + NVL (pfc_sr_cons, 0)
                            + NVL (pfc_ofpk_cons, 0)
                            + NVL (pfc_pk_cons, 0)
                            + NVL (xf_loss_sr_cons, 0)
                            + NVL (xf_loss_ofpk_cons, 0)
                            + NVL (xf_loss_pk_cons, 0)
                            + NVL (cons_kwh_mr1, 0)
                            + NVL (cons_kwh_mr2, 0)
                            + NVL (cons_kwh_mr3, 0)
                            + NVL (cons_kwh_mr4, 0)
                            + NVL (pfc_mr1_cons, 0)
                            + NVL (pfc_mr2_cons, 0)
                            + NVL (pfc_mr3_cons, 0)
                            + NVL (pfc_mr4_cons, 0)
                            + NVL (xf_loss_mr1_cons, 0)
                            + NVL (xf_loss_mr2_cons, 0)
                            + NVL (xf_loss_mr3_cons, 0)
                            + NVL (xf_loss_mr4_cons, 0))
                               BldUnit,
                SUM(nvl(ENG_CHRG_SR,0)+
                nvl(ENG_CHRG_OFPK,0)+ nvl(ENG_CHRG_PK,0)+
                nvl(minimum_CHRG,0)+nvl(service_CHRG,0)+nvl(demand_CHRG,0)+
                nvl(adjusted_prn,0)+nvl(pfc_charge,0)
                +nvl(xf_loss_chrg,0)+nvl(xf_rent,0)
                +nvl(eng_chrg_mr1,0)+nvl(eng_chrg_mr2,0)+nvl(eng_chrg_mr3,0)+nvl(eng_chrg_mr4,0))  CurPrin,
                SUM(b.current_vat) CurVat,
                SUM(nvl(arr_adv_adj_prn,0)
                +nvl(adjusted_prn,0)) ArrPrin,
                SUM(nvl(b.ARR_ADV_ADJ_VAT,0)+nvl(adjusted_vat,0)) ArrVat,
                SUM(nvl(current_lps,0)+nvl(arr_adv_adj_lps,0)+nvl(adjusted_lps,0)) ArrLps,
                SUM(b.TOTAL_BILL) TotalBill,
                SUM(nvl(current_lps,0)) CurrentLps
                from ebc.bc_bill_image b
                 where
                 nvl(arr_adv_adj_prn,0)+nvl(adjusted_prn,0) + (nvl(current_lps,0)+nvl(arr_adv_adj_lps,0)+nvl(adjusted_lps,0)) + (nvl(b.ARR_ADV_ADJ_VAT,0)+nvl(adjusted_vat,0)) >0
                and b.invoice_num is not null
                and b.TARIFF<>'I'
                AND  b.bill_cycle_code =:billCycleCode " +
                $" AND b.LOCATION_CODE in ({locationString}) " + conditions +
                @" GROUP BY b.bill_cycle_code";

                var result = await conn.QueryFirstOrDefaultAsync<CenterWiseArrearSummaryDTO>(sQuery, new
                {
                    billCycleCode
                });
                if (result != null) 
                {
                    result.Center = dbName + "Computer Center";
                    result.Order = order;
                    CenterWiseArrearSummaryDataList.Add(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        #region All Customer Arrear Based on Offset

        #region Details 
        public List<PrepaidCustomerArrearBasedOnOffsetDTO> ArrearPrepaidCustomerDataList { get; set; } = new List<PrepaidCustomerArrearBasedOnOffsetDTO>();
      

        public async Task<List<PrepaidCustomerArrearBasedOnOffsetDTO>> PrepaidCustomerArrarBasedOnOffset(string[] dbCodes, string[]? locationCodes, string consumerNo, string tariff, decimal fromAmount, decimal toAmount, int status, bool isAll, bool isPrincipal, bool isVat, bool isLps, string orderBy, string reportType, int userId, int roleId = 0)
        {
            try
            {
                List<Task> listOfTasks = new List<Task>();
                if (dbCodes.Length > 0 && locationCodes != null)
                {
                    string locCodeList = (locationCodes != null) ? "'" + string.Join("', '", locationCodes) + "'" : " ";
                    foreach (var dbCode in dbCodes)
                    {
                        try
                        {

                            //var loc = await _locationRepoistory.GetLocationByLocation(location);
                            //if (loc == null) { throw new ArgumentNullException(); }
                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by code.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.
                                                                                                                                                        
                            listOfTasks.Add(Task.Run(() =>  GetPrepaidArrearDataAsync(locationCodes.Length, locCodeList, consumerNo, tariff, fromAmount, toAmount, status, orderBy, isAll, isPrincipal, isVat, isLps, connectionString)));
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
                else if (dbCodes.Length > 0 && locationCodes == null)
                {
                    foreach (var code in dbCodes)
                    {
                        try
                        {

                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.



                            if (connectionString == null)
                                throw new ArgumentNullException();
                            var locationString = " ";
                            //var locations = await _misInfoRepo.GetLocationByDBMIdAsync(center, Id, roleId);
                            var locations = await _locationRepoistory.GetLocationByDBMIdAsync(code, userId, roleId);
                            //string locCodeList = (locations != null) ? "'" + string.Join("', '", locations.Select(c => c.CODE)) + "'" : " ";
                            foreach (var res in locations)
                            {
                                //locationString += "'" + res.LOCATION_CODE + "'" + ",";
                                if (res.ID != 0)
                                {

                                    locationString += "'" + res.CODE + "'" + ",";
                                }
                            }
                            listOfTasks.Add(Task.Run(() =>  GetPrepaidArrearDataAsync(locations.Count(), locationString.TrimEnd(','), consumerNo, tariff, fromAmount, toAmount, status, orderBy, isAll, isPrincipal, isVat, isLps, connectionString)));


                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }


                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return ArrearPrepaidCustomerDataList.ToList();
        }


        private async Task GetPrepaidArrearDataAsync(int locationLength, string locationCodeList, string consumerNumber, string Tariff, decimal FromAmount, decimal ToAmount, int status, string orderBy, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, string connectionString)
        {
            //DatabaseConfig db = (DatabaseConfig)await _dbConfigRepo.GetDatabaseConfigByIDAsync(CenterId);
            //string connectionString = _commonRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            //if (connectionString == null)
            //    return connectionString;
            using IDbConnection conn = new OracleConnection(connectionString);

            //var billCycleCode = DateTime.UtcNow.AddMonths(-4).ToString("yyyyMM");
            var billCycleCode = "202012";

            try
            {
                string sQuery = "";
                bool locationCodeExist = (locationLength != 0); // Setting existence of location code depending on there being a value or not.
                bool isTariffExist = (Tariff.ToUpper() != "ALL");
                bool isConsumerNo = (int.Parse(consumerNumber) > 0);
                bool isFromAmountExist = (FromAmount != 0);
                bool isToAmountExist = (ToAmount != 0);
                string conditions = !isAll ? "and i.principal_arrear " + (isPrincipal ? ">" : "=") + @" 0
                and i.lps_arrear " + (isLPS ? ">" : "=") + @" 0
                and i.vat_arrear " + (isVAT ? ">" : "=") + @" 0" : "";


                sQuery = @"select a.location_code LocationCode,
                    g.DESCR Office,
                    substr(a.AREA_CODE,4,2) BillGroup,
                    substr(a.AREA_CODE,1,3) Book,
                    a.CUSTOMER_NUM||a.CHECK_DIGIT ConsumerNo,
                    a.WALKING_SEQUENCE WlkOr,
                    a.CONS_EXTG_NUM PvAc,
                    substr(a.CUSTOMER_NAME,1,35) Name,
                    a.F_H_NAME FatherName,
                    e.addr_descr1 || e.addr_descr2 || e.addr_descr3 AS Addr,
                    f.MONTHLY_LIKELY_CONS Likely,
                    c.usage_category_code Tariff,
                    i.lastbillmonth LastBillMonth,
                    i.Principal_Arrear PrincipalArrear,
                    i.LPS_arrear LpsArrear,
                    i.vat_Arrear VatArrear,
                    i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear TotalArrear
                    from ebc.bc_customers a,
                    ebc.bc_customer_category b,
                    ebc.bc_category_master c,
                    ebc.bc_customer_addr e,
                    ebc.bc_monthly_likely f,
                    ebc.bc_location_master g,
                    (select cust_id,MAX(BILL_CYCLE_CODE) LASTBILLMONTH, rec_status, sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) Principal_Arrear,
                    sum(LPS_AMT+LPS_ADJ-LPS_APPL) LPS_arrear,
                    sum(VAT_AMT+VAT_ADJ-VAT_APPL) vat_Arrear from bc_invoice_hdr
                    group by cust_id, rec_status
                    having sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL)+
                    sum(LPS_AMT+LPS_ADJ-LPS_APPL)+sum(VAT_AMT+VAT_ADJ-VAT_APPL)>1) i

                    where
                    a.cust_id not in (select cust_id from EBC.BC_METER_READING_CARD_DTL WHERE BILL_CYCLE_CODE> '" + billCycleCode + @"' GROUP BY Cust_id)
                   --and a.customer_status_code<>'D'
                    and a.cust_id=e.cust_id
                    and  a.cust_id=f.cust_id
                    and a.cust_id=i.cust_id and b.CUST_ID=a.CUST_ID
                    and b.cat_id=c.category_id and b.exp_date is null
                    and a.LOCATION_CODE=g.LOCATION_CODE
                    and f.EXP_DATE is null and e.addr_type='B' "
                + conditions +
                @" --and c.usage_category_code<>'I'
                    "
                + (status != 0 ? " AND i.REC_STATUS " + (status == 1 ? "=" : "<>") + @" 'D'" : "") + @"
                    and e.ADDR_EXP_DATE is null "
                 + (locationCodeExist ? @$"   AND A.LOCATION_CODE IN ( {locationCodeList} ) " : "")
                + (isConsumerNo ? @" AND a.CUSTOMER_NUM||a.CHECK_DIGIT =:ConsumerNo" : "")
                + (isTariffExist ? @" AND  c.usage_category_code =:tariff " : "")
                + (isFromAmountExist ? @"  AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear >=:ArrearFrom " : "") +
                (isToAmountExist ? @"  AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear <=:ArrrearTo " : "") +
                @" order by a.location_code, TotalArrear " + (orderBy == "0" ? "asc" : "desc");



                var result = await conn.QueryAsync<PrepaidCustomerArrearBasedOnOffsetDTO>(sQuery, new
                {
                   // Location_Code = Location_Code.ToUpper().ToString(),
                    ConsumerNo = consumerNumber,
                    tariff = Tariff.ToString(),
                    ArrearFrom = FromAmount,
                    ArrrearTo = ToAmount
                });
                ArrearPrepaidCustomerDataList.AddRange(result);
                // return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region summary

        public async Task<List<PrepaidCustomerArrearBasedOnOffsetDTO>> PrepaidCustomerArrarBasedOnOffsetSummary(string[] dbCodes, string[]? locationCodes, string consumerNo, string tariff, decimal fromAmount, decimal toAmount, int status, bool isAll, bool isPrincipal, bool isVat, bool isLps, string orderBy, string reportType, int userId, int roleId = 0)
        {
            try
            {
                List<Task> listOfTasks = new List<Task>();
                if (dbCodes.Length > 0 && locationCodes != null)
                {
                    string locCodeList = (locationCodes != null) ? "'" + string.Join("', '", locationCodes) + "'" : " ";
                    foreach (var dbCode in dbCodes)
                    {
                        try
                        {

                            //var loc = await _locationRepoistory.GetLocationByLocation(location);
                            //if (loc == null) { throw new ArgumentNullException(); }
                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(dbCode); // Getting database info by code.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.

                            listOfTasks.Add(Task.Run(() => GetPrepaidArrearSummaryDataAsync(locationCodes.Length, locCodeList, consumerNo, tariff, fromAmount, toAmount, status, orderBy, isAll, isPrincipal, isVat, isLps, connectionString)));
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
                else if (dbCodes.Length > 0 && locationCodes == null)
                {
                    foreach (var code in dbCodes)
                    {
                        try
                        {

                            var db = (Core.Domain.DatabaseConfig.DatabaseConfig)await _dbConfigRepo.GetDatabaseDataByDbCodeAsync(code); // Getting database info by code.
                            string connectionString = _commRepositry.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD); // Getting connection string by database.



                            if (connectionString == null)
                                throw new ArgumentNullException();
                            var locationString = " ";
                            //var locations = await _misInfoRepo.GetLocationByDBMIdAsync(center, Id, roleId);
                            var locations = await _locationRepoistory.GetLocationByDBMIdAsync(code, userId, roleId);
                            //string locCodeList = (locations != null) ? "'" + string.Join("', '", locations.Select(c => c.CODE)) + "'" : " ";
                            foreach (var res in locations)
                            {
                                //locationString += "'" + res.LOCATION_CODE + "'" + ",";
                                if (res.ID != 0)
                                {

                                    locationString += "'" + res.CODE + "'" + ",";
                                }
                            }
                            listOfTasks.Add(Task.Run(() => GetPrepaidArrearSummaryDataAsync(locations.Count(), locationString.TrimEnd(','), consumerNo, tariff, fromAmount, toAmount, status, orderBy, isAll, isPrincipal, isVat, isLps, connectionString)));


                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }


                Task.WaitAll(listOfTasks.ToArray());

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return ArrearPrepaidCustomerDataList.ToList();
        }


        private async Task GetPrepaidArrearSummaryDataAsync(int locationLength, string locationCodeList, string consumerNumber, string Tariff, decimal FromAmount, decimal ToAmount, int status, string orderBy, bool isAll, bool isPrincipal, bool isVAT, bool isLPS, string connectionString)
        {
            //DatabaseConfig db = (DatabaseConfig)await _dbConfigRepo.GetDatabaseConfigByIDAsync(CenterId);
            //string connectionString = _commonRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            //if (connectionString == null)
            //    return connectionString;
            using IDbConnection conn = new OracleConnection(connectionString);

            var billCycleCode = DateTime.UtcNow.AddMonths(-4).ToString("yyyyMM");

            try
            {
                string sQuery = "";
                bool locationCodeExist = (locationLength != 0); // Setting existence of location code depending on there being a value or not.
                bool isTariffExist = (Tariff.ToUpper() != "ALL");
                bool isConsumerNo = (int.Parse(consumerNumber) > 0);
                bool isFromAmountExist = (FromAmount != 0);
                bool isToAmountExist = (ToAmount != 0);
                string conditions = !isAll ? "and i.principal_arrear " + (isPrincipal ? ">" : "=") + @" 0
                and i.lps_arrear " + (isLPS ? ">" : "=") + @" 0
                and i.vat_arrear " + (isVAT ? ">" : "=") + @" 0" : "";


                sQuery = @"select a.location_code LocationCode,
                    g.DESCR Office,
                    COUNT(*) NoOfConsumer,
                    sum(nvl(i.Principal_Arrear,0)) PrincipalArrear,
                    SUM(nvl(i.LPS_arrear,0)) LpsArrear,
                    SUM(nvl(i.vat_Arrear,0)) VatArrear,
                    SUM(nvl(i.Principal_Arrear,0)+nvl(i.LPS_arrear,0)+nvl(i.vat_Arrear,0)) TotalArrear
                    from ebc.bc_customers a,
                    ebc.bc_customer_category b,
                    ebc.bc_category_master c,
                    ebc.bc_customer_addr e,
                    ebc.bc_monthly_likely f,
                    ebc.bc_location_master g,
                    (select cust_id,MAX(BILL_CYCLE_CODE) LASTBILLMONTH, rec_status, sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL) Principal_Arrear,
                    sum(LPS_AMT+LPS_ADJ-LPS_APPL) LPS_arrear,
                    sum(VAT_AMT+VAT_ADJ-VAT_APPL) vat_Arrear from bc_invoice_hdr
                    group by cust_id, rec_status
                    having sum(PRINCIPAL_AMT+PRINCIPAL_ADJ-PRINCIPAL_APPL)+
                    sum(LPS_AMT+LPS_ADJ-LPS_APPL)+sum(VAT_AMT+VAT_ADJ-VAT_APPL)>1) i

                    where
                    a.cust_id not in (select cust_id from EBC.BC_METER_READING_CARD_DTL WHERE BILL_CYCLE_CODE> '" + billCycleCode + @"' GROUP BY Cust_id)
                    and a.customer_status_code<>'D'
                    and a.cust_id=e.cust_id
                    and  a.cust_id=f.cust_id
                    and a.cust_id=i.cust_id and b.CUST_ID=a.CUST_ID
                    and b.cat_id=c.category_id and b.exp_date is null
                    and a.LOCATION_CODE=g.LOCATION_CODE
                    and f.EXP_DATE is null and e.addr_type='B' "
                + conditions +
                @"-- and c.usage_category_code<>'I' "
                + (status != 0 ? " AND i.REC_STATUS " + (status == 1 ? "=" : "<>") + @" 'D'" : " ") + @"
                    and e.ADDR_EXP_DATE is null
                    --and i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear>1
                     "
                 + (locationCodeExist ? @$"   AND A.LOCATION_CODE IN ( {locationCodeList} ) " : "")
                + (isConsumerNo ? @" AND a.CUSTOMER_NUM||a.CHECK_DIGIT =:ConsumerNo" : "")
                + (isTariffExist ? @" AND  c.usage_category_code =:tariff " : "")
                + (isFromAmountExist ? @"  AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear >=:ArrearFrom " : "") +
                (isToAmountExist ? @"  AND  i.Principal_Arrear+i.LPS_arrear+i.vat_Arrear <=:ArrrearTo " : "") +
                @"  group by a.location_code, g.DESCR  order by a.location_code, TotalArrear " + (orderBy == "0" ? "asc" : "desc") ;



                var result = await conn.QueryAsync<PrepaidCustomerArrearBasedOnOffsetDTO>(sQuery, new
                {
                    // Location_Code = Location_Code.ToUpper().ToString(),
                    ConsumerNo = consumerNumber,
                    tariff = Tariff.ToString(),
                    ArrearFrom = FromAmount,
                    ArrrearTo = ToAmount
                });
                ArrearPrepaidCustomerDataList.AddRange(result);
                // return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<IllegalConsumerPenaltyDTO>> GetIllegalConsumerPenalty( string billMonth)
        {
            using var con = new OracleConnection(Connection.ConnectionString());
            //List<ExistingConsumerPenaltyDTO> dto = new List<ExistingConsumerPenaltyDTO>();
          
            OracleDynamicParameters param = new OracleDynamicParameters();
            param.Add(":CUR_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
           // param.Add("P_ZONE_CODE", value: zoneCode, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            param.Add("P_BILL_MONTH", value: billMonth, dbType: (OracleMappingType?)OracleDbType.Varchar2, direction: ParameterDirection.Input);
            var result = con.Query<IllegalConsumerPenaltyDTO>("MISC_REPORT_MINISTRY_PKG.ILLEGAL_CUSTOMER_PENALTY", param: param, commandType: CommandType.StoredProcedure).ToList();
            return result;
            
           
        }


        #endregion

        #endregion All Customer Arrear Based On Offset

    }
}
