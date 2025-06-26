using Core.Domain.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.MISCBILL
{
    public class PenaltyExistingBillDto
    {

        public int? BILL_ID{ get; set; }
        public string? BILL_NO { get; set; }
        public string? LocationCode { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddr { get; set; }
        public string? TariffDesc { get; set; }
        public string? BbusinessType { get; set; }
        public string? PrvAcNo { get; set; }
        public string? AreaCode { get; set; }
        public string? NidNumber { get; set; }
        public string? MobileNumber { get; set; }

        public string? MeterTypeDesc { get; set; }
        public string? MeterNum { get; set; }
        public string? MeterConditionDesc { get; set; }
        public string? MeterTypeCode { get; set; }
        public string? LastReadingOffPeak { get; set; }
        public string? LastReadingPeak { get; set; }
        public string? LDate { get; set; }

        public string? BillReasonCode { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public string? ImposedByCode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? DDate { get; set; }
        public string? CreateBy { get; set; }
        public int? CustId { get; set; }
        public string? LastReadingDate { get; set; }
        public string? DueDate { get; set; }

        //public int SANC_LOAD { get; set; }
        //public int CONN_LOAD { get; set; }
        //public int TOTAL_BILL_AMOUNT { get; set; }
        //public int TOT_PRINCPAL_AMOUNT { get; set; }
        //public int TOT_VAT_AMOUNT { get; set; }
        //public int TOT_LPS_AMOUNT { get; set; }
        //public string REMARKS { get; set; }
        //public string STATUS { get; set; }
        //public string CREATE_BY { get; set; }
        //public string CREATE_DATE { get; set; }
        //public string UPDATE_BY { get; set; }
        //public string UPDATE_DATE { get; set; }


        //public int BILL_ID { get; set; }
        //public string BILL_NO { get; set; }
        //public string BILL_CHECK_DIGIT { get; set; }
        //public string CUSTOMER_NUM { get; set; }
        //public string CUST_NUM_CHEK_DIGIT { get; set; }
        //public string WALKING_SEQUENCE { get; set; }
        //public string CONS_EXTG_NUM { get; set; }
        //public string CUST_NAME { get; set; }
        //public string CUST_ADDRESS { get; set; }
        //public string PHONE { get; set; }
        //public string NID { get; set; }
        //public string BUS_CODE { get; set; }
        //public string LOCATION_CODE { get; set; }
        //public string AREA_CODE { get; set; }
        //public int CUST_ID { get; set; }
        //public string BILL_TYPE_CODE { get; set; }
        //public string BILL_REASON_CODE { get; set; }
        //public string USAGE_CAT_CODE { get; set; }
        //public string READING_DATE { get; set; }
        //public string DUE_DATE { get; set; }
        //public string METER_CODE { get; set; }
        //public string METER_NUM { get; set; }
        //public int METER_DIGIT { get; set; }
        //public string METER_COND { get; set; }
        //public string METER_TYPE { get; set; }
        //public string METER_STATUS { get; set; }
        //public int SANC_LOAD { get; set; }
        //public int CONN_LOAD { get; set; }
        //public int TOTAL_BILL_AMOUNT { get; set; }
        //public int TOT_PRINCPAL_AMOUNT { get; set; }
        //public int TOT_VAT_AMOUNT { get; set; }
        //public int TOT_LPS_AMOUNT { get; set; }
        //public string REMARKS { get; set; }
        //public string STATUS { get; set; }
        //public string CREATE_BY { get; set; }
        //public string CREATE_DATE { get; set; }
        //public string UPDATE_BY { get; set; }
        //public string UPDATE_DATE { get; set; }

    }
}
