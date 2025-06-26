using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.NonBengali
{
    public class NonBengalis
    {
        public string NON_BANGLI_NAME_CODE { get; set; }
        public string CODE { get; set; }
        public string NAMEBN { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAMEBN { get; set; }
        public int METER_COUNT { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }

        //public decimal PREV_ARREAR_AMT { get; set; }
        //public decimal CURR_MONTH_BILL { get; set; }
        //public decimal COLLECTION_AMOUNT { get; set; }
        //public decimal TOTAL_ARREAR_AMOUNT { get; set; }

        public decimal TOTAL_ARREAR { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public int CUSTOMER_NO { get; set; }
    }

    public class NonBengaliSummary 
    {
        public string NON_BANGLI_NAME_CODE { get; set; }
        public string NAMEBN { get; set; }
        public string LOCATION_CODE { get; set; }
        public string METER_COUNT { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal TOTAL_ARREAR { get; set; }
        public string LOCATION_NAMEBN { get; set; }
        public int IS_SATATIC { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
    }

    public class NonBengaliCustId 
    {
        public int CUST_ID { get; set; }
        public string NON_BENGALI_CAMP_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public string NONBENGALI_NAMEBN { get; set; }
        public string NONBENGALI_NAME { get; set; }
        public string LOCATION_NAMEBN { get; set; }
    }

    public class OnlineNonBengaliSummary 
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string ZONE_NAME { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public string METER_COUNT { get; set; }
        public decimal RECEIVE_PRN { get; set; }
        public decimal RECEIVE_LPS { get; set; }
        public decimal RECEIVE_VAT { get; set; }
        public decimal RECEIVE_TOTAL { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public decimal ARREAR_AMT { get; set; }
        public decimal RECEIPT_AMT { get; set; }
        public decimal CURR_BILL { get; set; }
        public decimal PREV_MONTH { get; set; }
        public string LOCATION_DESC { get; set; }
        public decimal ARREAR_PRICIPAL { get; set; }
        public decimal ARREAR_LPS { get; set; }
        public decimal ARREAR_VAT { get; set; }
        public decimal CURR_PRINCIPAL { get; set; }
        public decimal CURR_LPS { get; set; }
        public decimal CURR_VAT { get; set; }
        public decimal TOTAL_PRINCIPAL_ARREAR { get; set; }
        public decimal TOTAL_LPS_ARREAR { get; set; }
        public decimal TOTAL_VAT_ARREAR { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
        public decimal TOTAL_RECEIPT_PRINCIPAL { get; set; }
        public decimal TOTAL_RECEIPT_VAT { get; set; }
        public decimal ARREAR_RECEIPT_AMOUNT { get; set; }
        public decimal FINANCIAL_AMOUNT { get; set; }
        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal CURR_RECEIPT_PRINCIPAL { get; set; }
        public decimal CURR_RECEIPT_AMT { get; set; }
    }

    public class OnlineNonBengaliSummaryMerge 
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string NON_BENGALI_CAMP_CODE { get; set; }
        public string NONBENGALI_NAMEBN { get; set; }
        public string NONBENGALI_NAME { get; set; }
        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal CURR_RECEIPT_PRINCIPAL { get; set; }
        public decimal CURR_RECEIPT_AMT { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public decimal TOTAL { get; set; }
        public decimal RECEIVE_PRN { get; set; }
        public decimal RECEIVE_LPS { get; set; }
        public decimal RECEIVE_VAT { get; set; }
        public decimal RECEIVE_TOTAL { get; set; }
        public string ADDRESS { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public decimal ARREAR_AMT { get; set; }
        public decimal RECEIPT_AMT { get; set; }
        public decimal CURR_BILL { get; set; }
        public string LOCATION_DESC { get; set; }
        public decimal TOTAL_PRINCIPAL_ARREAR { get; set; }
        public decimal TOTAL_LPS_ARREAR { get; set; }
        public decimal TOTAL_VAT_ARREAR { get; set; }
        public decimal TOTAL_AMOUNT { get; set; }
        public decimal TOTAL_RECEIPT_PRINCIPAL { get; set; }
        public decimal TOTAL_RECEIPT_VAT { get; set; }
        public decimal ARREAR_RECEIPT_AMOUNT { get; set; }
        public decimal ARREAR_PRICIPAL { get; set; }
        public decimal ARREAR_LPS { get; set; }
        public decimal ARREAR_VAT { get; set; }
        public decimal CURR_PRINCIPAL { get; set; }
        public decimal CURR_LPS { get; set; }
        public decimal CURR_VAT { get; set; }
    }

    public class FinancialYearModel
    {
        public string? START_BILLCYCLE { get; set; }
        public string? END_BILLCYCLE { get; set; }

    }

}
