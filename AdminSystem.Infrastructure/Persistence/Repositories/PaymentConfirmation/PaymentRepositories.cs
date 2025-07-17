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
            var db = await _dbConfigRepo.GetDatabaseDataBylocCodeAsync(locationCode);
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);

            using var con = new OracleConnection(connectionString);

            // Convert paymentDate to just date part in "yyyy-MM-dd" format
            string payments = paymentDate.ToString("yyyy-MM-dd");

            string sQuery = @"
        SELECT  
            bi.customer_num AS CustomerNumber,
            inv.total_bill_amount AS TotalBillAmount,
            (bi.INVOICE_NUM || bi.INVOICE_CHK_DGT) AS BillNumber,
            bi.location_code AS LocationCode,
            r.receipt_date AS PaymentDate
        FROM 
            bc_receipt_hdr r
        INNER JOIN 
            EBC.bc_bill_image bi ON r.invoice_num = bi.invoice_num
        INNER JOIN  
            BC_INVOICE_HDR inv ON inv.invoice_num = bi.invoice_num
        WHERE 
            (bi.INVOICE_NUM || bi.INVOICE_CHK_DGT) = :billNumber
            AND bi.location_code = :locationCode
            AND TRUNC(r.receipt_date) = TO_DATE(:paymentDate, 'YYYY-MM-DD')
    ";

            var payment = await con.QueryFirstOrDefaultAsync<PaymentConfirmationDto>(
                sQuery,
                new
                {
                    billNumber = billNumber,
                    locationCode = locationCode,
                    paymentDate = payments // Pass as string
                });

            return payment;
        }



        public async Task<PaymentStatusDto> PaymentStatus(string billNumber,string locationCode)
        {
            var db = await _dbConfigRepo.GetDatabaseDataBylocCodeAsync(locationCode); // Getting database info by id.
            string connectionString = _commRepo.CreateConnectionString(db.HOST, db.PORT, db.SERVICE_NAME, db.USER_ID, db.PASSWORD);
            using var con = new OracleConnection(connectionString);

            string sQuery = $@"
                    select '{billNumber}' BillNumber,TO_CHAR(Receipt_DATE, 'YYYY-MM-DD""T""HH24:MI:SS') AS PaymentDate,
                CASE 
                    WHEN  Receipt_DATE IS NOT NULL THEN 1
                    ELSE 0
                END AS IsPaid  from bc_receipt_hdr where invoice_num = subStr('{billNumber}',1,8)";

            var payment = await con.QueryFirstOrDefaultAsync<PaymentStatusDto>(sQuery);
            return payment;
        }
    }
}
