  using Core.Application.Commands.ProsoftDataSync.CreateUser;
using Core.Domain.Agricultures;
using Core.Domain.Building;
using Core.Domain.DatabaseConfig;
using Core.Domain.Location;
using Core.Domain.Ministry;
using Core.Domain.Nem;
using Core.Domain.OfficeStuff;
using Core.Domain.ProsoftDataSync;
using Shared.DTOs.Agriculture;
using Shared.DTOs.Building;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.DatabaseConfig;
using Shared.DTOs.Location;
using Shared.DTOs.Ministry;
using Shared.DTOs.OffiecStuff;
using Shared.DTOs.ProsoftDataSync;
using Shared.DTOs.ReplicationStatus;

namespace Core.Application.Common.Mapper;

public class AdminMapperProfile : Profile
{
    public AdminMapperProfile()
    {
        CreateMap<DatabaseConfig, DropdownResultForStringKey>()
            .ForMember(d => d.Key, O => O.MapFrom(S => S.CODE))
            .ForMember(d => d.Value, O => O.MapFrom(S => S.NAME))
            .ReverseMap();
        CreateMap<Locations, DropdownResultForStringKey>()
            .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
            .ForMember(d => d.Value, o => o.MapFrom(s => string.Format("{0}({1})", s.CODE,s.NAME)))
            .ReverseMap();

        

        CreateMap<User, UserDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.USER_NAME))
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.ID))
            .ReverseMap();

        CreateMap<Role, RoleDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.ROLE_NAME))
            .ForMember(d => d.MenuFkId, o => o.MapFrom(s => s.MENU_ID_FK))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IS_ACTIVE))
            .ForMember(d => d.Priority, o => o.MapFrom(s => s.PRIORITY))
            .ForMember(d => d.TotalRowCount, o => o.MapFrom(s => s.TOTAL_ROW_COUNT))
            .ReverseMap();

        CreateMap<Menu, MenuDto>()
            .ForMember(d => d.MenuId, o => o.MapFrom(s => s.ID))
            .ForMember(d => d.MenuName, o => o.MapFrom(s => s.MENU_NAME))
            .ForMember(d => d.Url, o => o.MapFrom(s => s.URL))
            .ForMember(d => d.Icon, o => o.MapFrom(s => s.ICON))
            .ForMember(d => d.ParentId, o => o.MapFrom(s => s.PARENT_ID))
            .ForMember(d => d.IsParent, o => o.MapFrom(s => s.IS_PARENT))
            .ForMember(d => d.IsGroup, o => o.MapFrom(s => s.IS_GROUP))
            .ForMember(d => d.GroupId, o => o.MapFrom(s => s.GROUP_ID))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IS_ACTIVE))
            .ForMember(d => d.TimeStamp, o => o.MapFrom(s => s.TIMESTAMP))
            .ForMember(d => d.TotalRowCount, o => o.MapFrom(s => s.TOTAL_ROW_COUNT))
            .ForMember(d => d.OrderNo, o => o.MapFrom(s => s.ORDER_NO))
            .ReverseMap();

        CreateMap<Menu, NavItemDto>()
            .ForMember(d => d.ItemId, o => o.MapFrom(s => s.ID))
            .ForMember(d => d.ParentId, o => o.MapFrom(s => s.PARENT_ID))
            .ForMember(d => d.ItemName, o => o.MapFrom(s => s.MENU_NAME))
            .ForMember(d => d.Icon, o => o.MapFrom(s => s.ICON))
            .ForMember(d => d.Url, o => o.MapFrom(s => s.URL))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IS_ACTIVE))
            .ForMember(d => d.IsCreated, o => o.MapFrom(s => s.IS_CREATED))
            .ForMember(d => d.IsEdited, o => o.MapFrom(s => s.IS_EDITED))
            .ForMember(d => d.IsDeleted, o => o.MapFrom(s => s.IS_DELETED))
            .ReverseMap();

        CreateMap<RoleToMenuDto, NavItemDto>()
            .ForMember(d => d.ItemName, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Icon, o => o.MapFrom(s => s.Icon))
            .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
            .ReverseMap();


        CreateMap<Menu, SideBarItemDto>()
            .ForMember(d => d.ItemId, o => o.MapFrom(s => s.ID))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.MENU_NAME))
            .ForMember(d => d.Link, o => o.MapFrom(s => s.URL))
            .ForMember(d => d.Group, o => o.MapFrom(s => s.IS_GROUP))
            .ForMember(d => d.GroupId, o => o.MapFrom(s => s.GROUP_ID))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IS_ACTIVE))
            .ForMember(d => d.Icon, o => o.MapFrom(s => s.ICON == String.Empty ? null : new IconSideBar { Icon = s.ICON }))
            .ForMember(d => d.Icon, o => o.MapFrom(s => s.ICON == null ? null : new IconSideBar { Icon = s.ICON }))
            .ReverseMap();

        CreateMap<RoleToMenuDto, SideBarItemDto>()
            .ForMember(d => d.ItemId, o => o.MapFrom(s => s.MenuFkId))
            .ForMember(d => d.ParentId, o => o.MapFrom(s => s.ParentId))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Icon, o => o.MapFrom(s => new IconSideBar { Icon  = s.Icon }))
            .ForMember(d => d.Link, o => o.MapFrom(s => s.Url))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
            .ReverseMap();

        CreateMap<Building, BuildingDto>()
             .ForMember(d => d.BuildingId, o => o.MapFrom(s => s.ID))
             .ForMember(d => d.SiteNbr, o => o.MapFrom(s => s.SITE_NBR))
             .ForMember(d => d.AddressCode, o => o.MapFrom(s => s.ADDRESS_CODE))
             .ForMember(d => d.BuildingTitle, o => o.MapFrom(s => s.BUILDING_TITLE))
             .ForMember(d => d.Address, o => o.MapFrom(s => s.ADDRESS))
             .ForMember(d => d.PostalCode, o => o.MapFrom(s => s.POSTAL_CODE))
             .ForMember(d => d.BuildingType, o => o.MapFrom(s => s.BUILDING_TYPE))
             .ForMember(d => d.AssetNo, o => o.MapFrom(s => s.ASSET_NO))
             .ForMember(d => d.LtaId, o => o.MapFrom(s => s.LTA_ID))
             .ForMember(d => d.IssInstallDate, o => o.MapFrom(s => s.ISS_INSTALL_DATE))
             .ForMember(d => d.IssInstallDate, o => o.MapFrom(s => s.ISS_INSTALL_DATE))
             .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IS_ACTIVE))
             .ForMember(d => d.IsOnTest, o => o.MapFrom(s => s.IS_ON_TEST))
             .ForMember(d => d.TotalRowCount, o => o.MapFrom(s => s.TOTAL_ROW_COUNT))
             .ReverseMap();

        CreateMap<Employee, EmployeeDTO>()
            .ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.col_employee_id))
            .ForMember(d => d.SvcNo, o => o.MapFrom(s => s.col_svc_no))
            .ForMember(d => d.Rank, o => o.MapFrom(s => s.col_rank))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.col_name))
            //.ForMember(d => d.Date, o => o.MapFrom(s => s.col_date))
            .ForMember(d => d.ClockType, o => o.MapFrom(s => s.col_clock_type))
            .ForMember(d => d.ClockTime, o => o.MapFrom(s => s.col_clock_time))
            .ForMember(d => d.SiteId, o => o.MapFrom(s => s.col_site_id))
            .ForMember(d => d.ShiftCode, o => o.MapFrom(s => s.col_shift_code))
            .ForMember(d => d.Deployment, o => o.MapFrom(s => s.col_deployment))
            .ReverseMap();

        CreateMap<ProsoftUsers, CreateUserCommand>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.USER_NAME))
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FULL_NAME))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.EMAIL))
            .ReverseMap();

        CreateMap<DatabaseConfig, DatabaseConfigDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
            .ForMember(d => d.DbName, o => o.MapFrom(s => s.NAME))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.ISACTIVE))
            .ForMember(d => d.UpdateBy, o => o.MapFrom(s => s.UPDATE_BY))
            .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.UPDATE_DATE))
            .ForMember(d => d.CreateBy, o => o.MapFrom(s => s.CREATE_BY))
            .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CREATE_DATE))
            .ForMember(d => d.Host, o => o.MapFrom(s => s.HOST))
            .ForMember(d => d.Port, o => o.MapFrom(s => s.PORT))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.SERVICE_NAME))
            .ForMember(d => d.Password, o => o.MapFrom(s => s.PASSWORD))
            .ForMember(d => d.OrderCol, o => o.MapFrom(s => s.ORDER_NO))
            .ForMember(d => d.DbNameBn, o => o.MapFrom(s => s.DB_NAME_BN))
            .ForMember(d => d.Code,o=>o.MapFrom(s=>s.CODE))
            .ReverseMap();

        //CreateMap<Locations, LocationsDto>()
        //    .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
        //    .ForMember(d => d.DbconfigId, o => o.MapFrom(s => s.DBCONFIG_ID))
        //    .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
        //    .ForMember(d => d.UpdateBy, o => o.MapFrom(s => s.UPDATE_BY))
        //    .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.UPDATE_DATE))
        //    .ForMember(d => d.CreateBy, o => o.MapFrom(s => s.CREATE_BY))
        //    .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CREATE_DATE))
        //    .ForMember(d => d.DbName, o => o.MapFrom(s => s.DB_NAME))
        //    .ForMember(d => d.LocationDescription, o => o.MapFrom(s => s.LOCATION_DESCRIPTION))
        //    .ReverseMap();

        CreateMap<Locations, LocationsDto>()
    .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
    .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
    .ForMember(d => d.NameBn, o => o.MapFrom(s => s.NAMEBN))
    .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
    .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
    .ForMember(d => d.CircleCode, o => o.MapFrom(s => s.CIRCLE_CODE))
    .ForMember(d => d.DbCode, o => o.MapFrom(s => s.DB_CODE))
    .ForMember(d => d.DeptCode, o => o.MapFrom(s => s.DEPTCODE))
    .ReverseMap();

        CreateMap<UserCreateByCenterLocation, CreateUserToRoleDto>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.ID))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.USER_NAME))
            .ForMember(d => d.RoleId, o => o.MapFrom(s => s.ROLE_ID))
            .ForMember(d => d.UserPassword, o => o.MapFrom(s => s.PASSWORD))
            .ForMember(d => d.Db, o => o.MapFrom(s => s.DB))
            .ForMember(d => d.Location, o => o.MapFrom(s => s.Location))
            .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.ENTRY_BY))
            .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.ENTRY_DATE))
            .ReverseMap();

        CreateMap<Agriculture, AgricultureDto>()
            .ForMember(d=>d.CustomerNo, o => o.MapFrom(s=>s.CUSTOMER_NO ))
            .ForMember(d => d.DeptNameBn, o => o.MapFrom(s => s.DEPT_NAMEBN))
            .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
            .ForMember(d =>d.LocationDescBn, o => o.MapFrom(s => s.LOCATION_DESCBN))
            .ForMember(d => d.IsKrishi, o => o.MapFrom(s => s.IS_KRISHI))
            .ForMember(d => d.IsPoultry, o => o.MapFrom(s => s.IS_POULTRY))
            .ForMember(d => d.ConExtgNum, o => o.MapFrom(s => s.CON_EXTG_NUM))
            .ForMember(d => d.ArrearAmt, o => o.MapFrom(s => s.ARREAR_AMT))
             .ForMember(d => d.CustomerNameBn, o => o.MapFrom(s => s.CUSTOMER_NAMEBN))
            .ReverseMap();

        CreateMap<GetUserCreateByCenterLocationModel, GetUserCreateByCenterLocationModelDto>()
             .ForMember(d => d.RoleId, o => o.MapFrom(s => s.ROLE_ID))
             .ForMember(d => d.UserId, o => o.MapFrom(s => s.USERID))
             .ForMember(d => d.UserName, o => o.MapFrom(s => s.USER_NAME))
             .ForMember(d => d.FullAccess, o => o.MapFrom(s => s.FULLACCESS))
             .ForMember(d => d.DbCode, o => o.MapFrom(s => s.DB_CODE))
             .ForMember(d => d.Status, o => o.MapFrom(s => s.STATUS))
              .ForMember(d => d.LocationId, o => o.MapFrom(s => s.LOCATION_ID))
             .ReverseMap();


         CreateMap<ConsumerModal, ConsumerDto>()
            .ForMember(d => d.AccountNumber, o => o.MapFrom(s => s.ACCOUNTNUMBER))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
            .ForMember(d => d.MeterNumber, o => o.MapFrom(s => s.METERNUMBER))
            .ForMember(d => d.OfficeCode, o => o.MapFrom(s => s.OFFICECODE))
            .ForMember(d => d.Tariff, o => o.MapFrom(s => s.TARIFFNAME))
            .ForMember(d => d.VoltageLevel, o => o.MapFrom(s => s.VOLTAGELEVEL))
            .ForMember(d => d.SiteAddress, o => o.MapFrom(s => s.SITEADDRESS))
            .ForMember(d => d.Load, o => o.MapFrom(s => s.LOAD))
            .ReverseMap();




        #region MinistrySummary, MinistrySummaryDto
        CreateMap<MinistrySummary, MinistrySummaryDto>()
             .ForMember(d => d.DbCode, o => o.MapFrom(s => s.DB_CODE))
             .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
             .ForMember(d=>d.DbName, o=>o.MapFrom(s => s.DB_NAME))
             .ForMember(d=>d.MinistryCode, o=>o.MapFrom(s=>s.MINISTRY_CODE))
             .ForMember(d => d.MinistryName, o => o.MapFrom(s => s.MINISTRY_NAME))
             .ForMember(d => d.MinistryNameBn, o => o.MapFrom(s => s.MINISTRY_NAMEBN))
             .ForMember(d => d.Noc, o => o.MapFrom(s => s.NOC))
             .ForMember(d => d.Lps, o => o.MapFrom(s => s.LPS))
             .ForMember(d => d.Vat, o => o.MapFrom(s => s.VAT))
             .ForMember(d => d.Prn, o => o.MapFrom(s => s.PRN))
             .ForMember(d => d.Total, o => o.MapFrom(s => s.TOTAL))

             .ForMember(d => d.ChittagongCount, o => o.MapFrom(s => s.CHITTAGONG_COUNT))
             .ForMember(d => d.ComillaCount, o => o.MapFrom(s => s.COMILLA_COUNT))
             .ForMember(d => d.JamalpurCount, o => o.MapFrom(s => s.JAMALPUR_COUNT))
             .ForMember(d => d.TangailCount, o => o.MapFrom(s => s.TANGAIL_COUNT))
             .ForMember(d => d.MymensinghCount, o => o.MapFrom(s => s.MYMENSINGH_COUNT))
             .ForMember(d => d.SylhetCount, o => o.MapFrom(s => s.SYLHET_COUNT))
             .ForMember(d => d.MoulvibazarCount, o => o.MapFrom(s => s.MOULVIBAZAR_COUNT))
             .ForMember(d => d.KishoreganjCount, o => o.MapFrom(s => s.KISHOREGANJ_COUNT))

             .ForMember(d => d.ChittagongPrevArrearAmt, o => o.MapFrom(s => s.CHITTAGONG_PREV_ARREAR_AMT))
             .ForMember(d => d.ChittagongCurrMonthBill, o => o.MapFrom(s => s.CHITTAGONG_CURR_MONTH_BILL))
             .ForMember(d => d.ChittagongCollectionAmt, o => o.MapFrom(s => s.CHITTAGONG_COLLECTION_AMOUNT))
             .ForMember(d => d.ChittagongTotalArrearAmt, o => o.MapFrom(s => s.CHITTAGONG_TOTAL_ARREAR_AMOUNT))
             
             .ForMember(d => d.ComillaPrevArrearAmt, o => o.MapFrom(s => s.COMILLA_PREV_ARREAR_AMT))
             .ForMember(d => d.ComillaCurrMonthBill, o => o.MapFrom(s => s.COMILLA_CURR_MONTH_BILL))
             .ForMember(d => d.ComillaCollectionAmt, o => o.MapFrom(s => s.COMILLA_COLLECTION_AMOUNT))
             .ForMember(d => d.ComillaTotalArrearAmt, o => o.MapFrom(s => s.COMILLA_TOTAL_ARREAR_AMOUNT))

             .ForMember(d => d.SylhetPrevArrearAmt, o => o.MapFrom(s => s.SYLHET_PREV_ARREAR_AMT))
             .ForMember(d => d.SylhetCurrMonthBill, o => o.MapFrom(s => s.SYLHET_CURR_MONTH_BILL))
             .ForMember(d => d.SylhetCollectionAmt, o => o.MapFrom(s => s.SYLHET_COLLECTION_AMOUNT))
             .ForMember(d => d.SylhetTotalArrearAmt, o => o.MapFrom(s => s.SYLHET_TOTAL_ARREAR_AMOUNT))

             .ForMember(d => d.MymensinghPrevArrearAmt, o => o.MapFrom(s => s.MYMENSINGH_PREV_ARREAR_AMT))
             .ForMember(d => d.MymensinghCurrMonthBill, o => o.MapFrom(s => s.MYMENSINGH_CURR_MONTH_BILL))
             .ForMember(d => d.MymensinghCollectionAmt, o => o.MapFrom(s => s.MYMENSINGH_COLLECTION_AMOUNT))
             .ForMember(d => d.MymensinghTotalArrearAmt, o => o.MapFrom(s => s.MYMENSINGH_TOTAL_ARREAR_AMOUNT))

             .ForMember(d => d.JamalpurPrn, o => o.MapFrom(s => s.JAMALPUR_PRN))
             .ForMember(d => d.TangailPrn, o => o.MapFrom(s => s.TANGAIL_PRN))
             .ForMember(d => d.MoulvibazarPrn, o => o.MapFrom(s => s.MOULVIBAZAR_PRN))
             .ForMember(d => d.KishoreganjPrn, o => o.MapFrom(s => s.KISHOREGANJ_PRN))


             .ReverseMap();

        #endregion MinistrySummary, MinistrySummaryDto

        #region ReplicationStatus

        CreateMap<ReplicationStatus, ReplicationStatusDto>()
            .ForMember(d => d.CardGenDate, o => o.MapFrom(s => s.CARD_GEN_DATE))
            .ForMember(d => d.MrsEntryDate, o => o.MapFrom(s => s.MRS_ENTRY_DATE))
            .ForMember(d => d.OverallProcDate, o => o.MapFrom(s => s.OVERALL_PROC_DATE))
            .ForMember(d => d.OverallFinalDate, o => o.MapFrom(s => s.OVERALL_FINAL_DATE))
            .ForMember(d => d.BillGenDate, o => o.MapFrom(s => s.BILL_GEN_DATE))
            .ForMember(d => d.BillFinalDate, o => o.MapFrom(s => s.BILL_FINAL_DATE))
            .ForMember(d => d.BillDespatchDate, o => o.MapFrom(s => s.BILL_DESPATCH_DATE))
            .ForMember(d => d.DatabaseStatus, o => o.MapFrom(s => s.DATABASESTATUS))
            .ForMember(d => d.Database, o => o.MapFrom(s => s.DATABASE))
            .ForMember(d => d.Color, o => o.MapFrom(s => s.COLOR))
            .ReverseMap();

        #endregion ReplicationStatus
    }

}
