using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Untracable
{
    public  class Untraceable
    {
        public int ID { get; set; }
        public int CUST_ID { get; set; }
        public int CUSTOMER_NUMBER { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string CUSTOMER_ADDRESS { get; set; }
        public string BUSINESS_TYPE { get; set; }
        public string TARIFF_ID { get; set; }
        public string TARIFF_DESCRIPTION { get; set; }
        public string AREA_CODE { get; set; }
        public string PREVIOUS_ACC_NO { get; set; }
        public string METER_NUMBER { get; set; }
        public string METER_TYPE_DESC { get; set; }
        public string METER_CONDITION { get; set; }
        public decimal STATUS { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_DATE { get; set; }
        public string UC_TYPE { get; set; }
        public string DB_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAMEBN { get; set; }
        public string LOCATION_NAMEBN { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAMEBN { get; set; }
    }
    public class MergeUntraceable: Untraceable
    {
        public string NAME { get; set; }
        public string ADDR { get; set; }
        public int CON_NO { get; set; }
        public string PV_AC { get; set; }
        public decimal TOTAL_ARREAR { get; set; }
        public string STATUS { get; set; }
        public string METER_STATUS { get; set; }
        public string TEMP_DISCON_DATE { get; set; }
        public string PERM_DISCON_DATE { get; set; }
        public string DISC_DATE { get; set; }
        public int CUST_ID { get; set; }
        public string LAST_BILL_MONTH { get; set; }
    }
    public class UntracebleCustReportDTO
    {
        public string NAME { get; set; }
        public string ADDR { get; set; }
        public int CON_NO { get; set; }
        public string PV_AC { get; set; }
        public decimal TOTAL_ARREAR { get; set; }
        public string STATUS { get; set; }
        public string DISC_DATE { get; set; }
    }

    public class UntracebleCustArrear
    {
        public decimal ID { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string NAME { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_DESC { get; set; }
        public decimal PRINCIPAL { get; set; }
        public decimal CURR_VAT { get; set; }
        public decimal CURR_LPS { get; set; }
        public DateTime CURR_DATE { get; set; }
        public string RUN_BILL_CYCLE_CODE { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public decimal CURR_BILL { get; set; }
        public decimal PREV_MONTH { get; set; }
        public decimal RECEIPT_AMT { get; set; }
        public string LOCATION_DESCBN { get; set; }
        public decimal ARREAR_AMT { get; set; }
        public string ADDRESS { get; set; }
        public decimal ARREAR_PRINCIPAL { get; set; }
        public decimal ARREAR_LPS { get; set; }
        public decimal ARREAR_VAT { get; set; }
        public decimal TOTAL_PRINCIPAL_ARREAR { get; set; }
        public decimal TOTAL_LPS_ARREAR { get; set; }
        public decimal TOTAL_VAT_ARREAR { get; set; }
        public decimal TOTAL_RECEIPT_PRINCIPAL { get; set; }
        public decimal TOTAL_RECEIPT_VAT { get; set; }
        public decimal ARREAR_RECEIPT_AMOUNT { get; set; }
        public decimal FINANCIAL_AMOUNT { get; set; }
        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal CURR_RECEIPT_AMT { get; set; }
        public decimal CURR_RECEIPT_PRINCIPAL { get; set; }
    }

    public class UntracedCustomerArrearModel
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public decimal TOTAL { get; set; }
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
        public decimal ARREAR_RECEIPT_PRINCIPAL { get; set; }
        public decimal ARREAR_RECEIPT_VAT { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAMEBN { get; set; }
		public string LOCATION_NAMEBN { get; set; }
		public string CIRCLE_CODE { get; set; }
		public string CIRCLE_NAMEBN { get; set; }
	}


    public class UntracebleCustArrearReportDTO
    {
        public string LocationCode { get; set; }
        public string LocationDesc { get; set; }
        public string LOCATIONNAMEBN { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string CircleCode { get; set; }
        public string CircleNameBn { get; set; }
        public int TotalUntracedCustCount { get; set; }
        public decimal TotalUntracedCustArrear { get; set; }
        public int PreMonTracedCustCount { get; set; } 
        public decimal PreMonTracedCustArrear { get; set; }
        public decimal PreMonTracedCustReceipt { get; set; }
        public int CurrMonTracedCustCount { get; set; } //TM = This month
        public decimal CurrMonTracedCustArrear { get; set; }
        public decimal CurrMonTracedCustReceipt { get; set; }
        public int TotalCurrMonTracedCustCount { get; set; } //TTM = Total This month
        public decimal TotalCurrMonTracedCustArrear { get; set; }
        public decimal TotalCurrMonTracedCustReceipt { get; set; }
        public int TotalFinalUntracedCustCount { get; set; }
        public decimal TOTALFINALARRERARAMOUNT { get; set; }
    }

    public class UntracebleCustArrearDetailsReportDTO
    {
        public string LocationCode { get; set; }
        public string LocationDesc { get; set; }
        public string LOCATIONNAMEBN { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string CircleCode { get; set; }
        public string CircleName { get; set; }
        public string CircleNameBn { get; set; }
        public int TotalUntracedCustCount { get; set; }
        public decimal TotalUntracedCustArrear { get; set; }
        public int PreMonTracedCustCount { get; set; }
        public decimal PreMonTracedCustArrear { get; set; }
        public decimal PreMonTracedCustReceipt { get; set; }
        public int CurrMonTracedCustCount { get; set; } //TM = This month
        public decimal CurrMonTracedCustArrear { get; set; }
        public decimal CurrMonTracedCustReceipt { get; set; }
        public int TotalCurrMonTracedCustCount { get; set; } //TTM = Total This month
        public decimal TotalCurrMonTracedCustArrear { get; set; }
        public decimal TotalCurrMonTracedCustReceipt { get; set; }
        public int TotalFinalUntracedCustCount { get; set; }
        public decimal TOTALFINALARRERARAMOUNT { get; set; }
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

        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public decimal TOTAL { get; set; }
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
     
        public decimal TOTAL_RECEIPT_PRINCIPAL { get; set; }
        public decimal TOTAL_RECEIPT_VAT { get; set; }
        public decimal ARREAR_RECEIPT_AMOUNT { get; set; }
        public decimal FINANCIAL_AMOUNT { get; set; }
        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal CURR_RECEIPT_PRINCIPAL { get; set; }
        public decimal CURR_RECEIPT_AMT { get; set; }
        public decimal ARREAR_RECEIPT_PRINCIPAL { get; set; }
        public decimal ARREAR_RECEIPT_VAT { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAMEBN { get; set; }
        public string LOCATION_NAMEBN { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAMEBN { get; set; }
    }

    public class UntracedCustomerArrearDetailsModel
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string LOCATION_CODE { get; set; }
        public decimal PRN { get; set; }
        public decimal LPS { get; set; }
        public decimal VAT { get; set; }
        public decimal TOTAL { get; set; }
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
        public decimal ARREAR_RECEIPT_PRINCIPAL { get; set; }
        public decimal ARREAR_RECEIPT_VAT { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAMEBN { get; set; }
        public string LOCATION_NAMEBN { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAMEBN { get; set; }
    }

    public class UntracedCustArrearMergeSummaryDto
    {
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public int PreMonTracedCustCount { get; set; } 
        public decimal PreMonTracedCustArrear { get; set; }
        public decimal PreMonTracedCustReceipt { get; set; }
        public int CurrMonTracedCustCount { get; set; } //TM = This month
        public decimal CurrMonTracedCustArrear { get; set; }
        public decimal CurrMonTracedCustReceipt { get; set; }
        public int TotalCurrMonTracedCustCount { get; set; } //TTM = Total This month
        public decimal TotalCurrMonTracedCustArrear { get; set; }
        public decimal TotalCurrMonTracedCustReceipt { get; set; }
    }

}
