using AdminSystem.Infrastructure.Persistence.Context;
using Core.Application.Interfaces.Common.Repository;
using Core.Application.Interfaces.DatabaseConfig;
using Core.Application.Interfaces.PaymentConfirmation;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.PaymentConfirmation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.PaymentConfirmation
{
    public class BillPaymentMiscRepositories : IBillPaymentMiscConfirmationRepository
    {

        //private readonly IDatabaseConfigRepository _dbConfigRepo;
        //private readonly ICommonRepository _commRepo;
        //public PaymentRepositories(IDatabaseConfigRepository dbConfigRepo, ICommonRepository commRepo)
        //{
        //    _dbConfigRepo = dbConfigRepo;
        //    _commRepo = commRepo;
        //}

        public async Task<BillPaymentMiscConfirmationDto> BillPaymentMiscConfirmation(string billNumber,  bool isPaid, DateTime paymentDate)
        {
            using var con = new OracleConnection(Connection.ConnectionString());


            string sQuery = @"
                      SELECT 
                        M.BILL_NO || M.BILL_CHECK_DIGIT AS BillNumber,
                        M.CUSTOMER_NUM || M.CUST_NUM_CHEK_DIGIT AS CustomerNumber,
                        M.CUST_NAME AS CustomerName,
                        M.TOTAL_BILL_AMOUNT AS TotalBillAmount,
                        TO_CHAR(PGT.PAY_DATETIME, 'YYYY-MM-DD""T""HH24:MI:SS') AS PaymentDate
                      FROM 
                        MISCBILL_BILL_MST M
                      LEFT JOIN 
                        MISCBILL_PAY_GTWAY_TRANSACTION PGT 
                        ON (PGT.CUSTOMER_NUM = M.CUSTOMER_NUM || M.CUST_NUM_CHEK_DIGIT)
                      WHERE 
                        (M.BILL_NO || M.BILL_CHECK_DIGIT) = :BillNumber
                        AND (
                            (:IsPaid = 1 AND SUBSTR(PGT.PAY_DATE, 1, 10) = :PaymentDate)
                         OR (:IsPaid = 0 AND PGT.CUSTOMER_NUM IS NULL)
                        )";

            var parameters = new
            {
                BillNumber = billNumber,
                PaymentDate = paymentDate.ToString("yyyy-MM-dd"),
                IsPaid = isPaid ? 1 : 0
            };

            var payment = await con.QueryFirstOrDefaultAsync<BillPaymentMiscConfirmationDto>(sQuery, parameters);
            return payment;
        }

        public async Task<BillPaymentMiscStatusDto> BillPaymentMiscStatus(string billNumber)
        {

            using var con = new OracleConnection(Connection.ConnectionString()); 

            string sQuery = @"
                            SELECT 
                            M.BILL_NO || M.BILL_CHECK_DIGIT AS BillNumber,
                            TO_CHAR(PGT.PAY_DATETIME, 'YYYY-MM-DD""T""HH24:MI:SS') AS PaymentDate,
                            CASE 
                                WHEN PGT.CUSTOMER_NUM IS NOT NULL THEN 1
                                ELSE 0
                            END AS IsPaid
                        FROM 
                            MISCBILL_BILL_MST M
                        LEFT JOIN 
                            MISCBILL_PAY_GTWAY_TRANSACTION PGT 
                            ON (PGT.CUSTOMER_NUM = M.CUSTOMER_NUM || M.CUST_NUM_CHEK_DIGIT)
                        WHERE 
                            (M.BILL_NO || M.BILL_CHECK_DIGIT) = :BillNumber";

            var parameters = new
            {
                BillNumber = billNumber
            };

            var payment = await con.QueryFirstOrDefaultAsync<BillPaymentMiscStatusDto>(sQuery, parameters);
            return payment;
        }

        
    }
}
