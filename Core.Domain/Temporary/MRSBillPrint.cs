using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Temporary
{
    public class MRSBillPrint
    {
        public string? LOCATION_CODE { get; set; }
        public string? LOCATION_NAME { get; set; }
        public string? BILL_MONTH { get; set; }
        public string? BILL_NUM { get; set; }
        public string? BILL_NUM_CHK_DIGIT { get; set; }
        public string? CUSTOMER_NUM { get; set; }
        public string? BILL_ISSUE_DATE { get; set; }
        public string? CUST_NAME { get; set; }
        public string? CUST_ADDRESS { get; set; }
        public string? AREA { get; set; }
        public string? BILL_GROUP { get; set; }
        public string? WALK_ORDER { get; set; }
        public string? PREV_ACC_NO { get; set; }

        public string? DUE_DATE { get; set; }
        public string? TARIFF { get; set; }
        public string? BUS_CODE { get; set; }
        public string? METER_NUM { get; set; }
        public string? METER_TYPE { get; set; }
        public string? METER_COND { get; set; }
        public string? PHONE { get; set; }
        public string? NID { get; set; }
        public string? POWER_FACTOR { get; set; }
        public int? UNIT_SR { get; set; }
        public int? UNIT_PEAK { get; set; }
        public int? UNIT_OFFPEAK { get; set; }
        public int? TOTAL_UNIT { get; set; }
        public int? OPN_UNIT_SR { get; set; }
        public int? OPN_UNIT_PEAK { get; set; }
        public int? OPN_UNIT_OFFPEAK { get; set; }
        public int? CLS_UNIT_SR { get; set; }
        public int? CLS_UNIT_PEAK { get; set; }
        public int? CLS_UNIT_OFFPEAK { get; set; }
        public decimal? ENERGY_AMOUNT_SR { get; set; }
        public decimal? ENERGY_AMOUNT_PEAK { get; set; }
        public decimal? ENERGY_AMOUNT_OFFPEAK { get; set; }
        public decimal? PFC_AMOUNT { get; set; }
        public string? DEMAND_CHARGE { get; set; }
        public string? CURRENT_LPS { get; set; }
        public string? CURRENT_VAT { get; set; }
        public string? CURRENT_PRN { get; set; }
        public string? ARR_PRN { get; set; }
        public string? ARR_VAT { get; set; }
        public string? ARR_LPS { get; set; }
        public decimal? TOT_PRINCPAL_AMOUNT { get; set; }
        public decimal? TOT_VAT_AMOUNT { get; set; }
        public decimal? TOT_LPS_AMOUNT { get; set; }
        public decimal? VAT_PERCENT { get; set; }
        public decimal? TOTAL_BILL_AMOUNT { get; set; }
        public string? PRESENT_DATE { get; set; }
        public string? PRESENT_RDG { get; set; }
        public string? PREVIOUS_RDG { get; set; }


    }
}
