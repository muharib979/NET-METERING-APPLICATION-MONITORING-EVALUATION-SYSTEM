using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.PaymentConfirmation;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.PaymentConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.PaymentConfirmation
{
    public class PaymentRepositories : IPaymentConfirmationRepository
    {

        private readonly IDatabaseConfigRepository _dbConfigRepo;
        private readonly ICommonRepository _commRepo;
        public PaymentRepositories(IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        {
            _dbConfigRepo = dbConfigRepo;
            _commRepo = commRepo;
        }

        public async Task<PaymentConfirmationDto> PaymentConfirmation(string billNumber, string locationCode, bool isPaid, DateTime paymentDate)
        {
            //using var con = new OracleConnection(Connection.ConnectionString());
            var db = await _dbConfigRepo.GetDatabaseDataBylocCodeAsync(locationCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var con = new OracleConnection(connectionString);

            string sQuery = @"
            SELECT 
                bi.customer_num AS CustomerNumber,
                r.total_bill_amount AS TotalBillAmount,
                bi.location_code AS LocationCode,
                bi.INVOICE_NUM || bi.INVOICE_CHK_DGT AS BillNumber,
                bi.RCPT_DATE_1 AS PaymentDate
            FROM 
                EBC.bc_bill_image bi
            INNER JOIN 
                bc_invoice_hdr r ON r.invoice_num = bi.invoice_num
            WHERE 
                (bi.INVOICE_NUM || bi.INVOICE_CHK_DGT) = :BillNumber 
                AND bi.location_code = :LocationCode 
                AND r.bill_cycle_code = :BillCycleCode";

            var parameters = new
            {
                BillNumber = billNumber,
                LocationCode = locationCode,
                BillCycleCode = paymentDate.ToString("yyyyMM")
            };

            var payment = await con.QueryFirstOrDefaultAsync<PaymentConfirmationDto>(sQuery, parameters);
            return payment;
        }

        public async Task<PaymentStatusDto> PaymentStatus(string billNumber,string locationCode)
        {
            var db = await _dbConfigRepo.GetDatabaseDataBylocCodeAsync(locationCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var con = new OracleConnection(connectionString);

            string sQuery = @"
           SELECT 
               bi.INVOICE_NUM || bi.INVOICE_CHK_DGT AS BillNumber,
               TO_CHAR(bi.RCPT_DATE_1, 'YYYY-MM-DD""T""HH24:MI:SS') AS PaymentDate,
                CASE 
                    WHEN  bi.RCPT_DATE_1 IS NOT NULL THEN 1
                    ELSE 0
                END AS IsPaid
                  FROM 
                     EBC.bc_bill_image bi
                 INNER JOIN 
                     bc_invoice_hdr r ON r.invoice_num = bi.invoice_num
                     WHERE (bi.INVOICE_NUM || bi.INVOICE_CHK_DGT) = :BillNumber";

            var parameters = new
            {
                BillNumber = billNumber
            };

            var payment = await con.QueryFirstOrDefaultAsync<PaymentStatusDto>(sQuery, parameters);
            return payment;
        }
    }
}
