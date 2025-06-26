using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CityCorporation
{
    public class OnlineCityCorporationAndPourshovaBn
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
        public decimal CURR_RECEIPT_AMT { get; set; }
        public string POUROSHOVA_NAME { get; set; }
        public string POUROSHOVA_CODE { get; set; }
        public string CITY_COROPRATION_NAME { get; set; }
        public string CITY_COROPRATION_CODE { get; set; }
        public string ZONE_CODE { get; set; }

        

    }
    public class CityCorporationCustIdModel
    {
        public int CUST_ID { get; set; }
        public string POUROSHOVA_CODE { get; set; }
        public string CITYCORPORATION_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string DB_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public string City_Cor_NAME { get; set; }
        public string POUR_NAME { get; set; }
        public string POUR_NAMEBN { get; set; }
        public string City_Cor_NAMEBN { get; set; }
        public int ORDER_NO { get; set; }



    }
    public class CityCorporationArrearModel
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
        public string RECEIPT_BILLMONTH { get; set; }
        public decimal PRV_RECEIPT_PRINCIPAL { get; set; }
       
        public decimal PRV_RECEIPT_VAT { get; set; }
        public decimal Total_Receipt_Arrear { get; set; }
        public decimal PRV_RECEIPT_AMT { get; set; }
        public decimal TOTAL_RECEIPT_ARREAR { get; set; }
        public string DB_CODE { get; set; }


    }

    public class OnlineCityPourMergeData
    {
        public int CUST_ID { get; set; }
        public string CONSUMER_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string LOCATION_CODE { get; set; }

        public decimal CURR_RECEIPT_VAT { get; set; }
        public decimal CURR_RECEIPT_PRINCIPAL { get; set; }
        public decimal CURR_RECEIPT_AMT { get; set; }

        public string POUROSHOVA_CODE { get; set; }
        public string CITYCORPORATION_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public string City_Cor_NAME { get; set; }
        public string POUR_NAME { get; set; }
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
        public string POUR_NAMEBN { get; set; }
        public string City_Cor_NAMEBN { get; set; }
        public string RECEIPT_BILLMONTH { get; set; }
        public decimal PRV_RECEIPT_PRINCIPAL { get; set; }

        public decimal PRV_RECEIPT_VAT { get; set; }
        public decimal PRV_RECEIPT_AMT { get; set; }
        public decimal Total_Receipt_Arrear { get; set; }
        
        public int ORDER_NO { get; set; }
    }

    public class OnlineCityPourMergeDTO
    {

        //public int CustId { get; set; }
        public string ConsumerNo { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount{ get; set; }
        public string CityCorporationCode{ get; set; }
        public string PouroshovaCode{ get; set; }
        public string CityCorporationName{ get; set; }
        public string PouroName{ get; set; }
        //public string LocationCode{ get; set; }

        // public int CustId { get; set; }
        public string Name { get; set; }
         public string CustomerName{ get; set; }
        public string LocationCode { get; set; }
        public string ZoneCode{ get; set; }
        public string ZoneName{ get; set; }
        public decimal ReceiptAmt { get; set; }
        public decimal ArrearReceiptAmount { get; set; }
        public decimal CurrReceiptVat { get; set; }
        public decimal CurrReceiptAmt { get; set; }
        public decimal TotalArrearAmount { get; set; }
        public double TotalReceiptAmount { get; set; }
        public double Total_ReceiptAmount { get; set; }
        public decimal CurrReceiptPrincipal { get; set; }
        public decimal CurrPrincipal { get; set; }
        public decimal ArrearLps { get; set; }
        public decimal ArrearPrincipal { get; set; }
        public decimal ArrearVat { get; set; }
        public decimal CurrPrin { get; set; }
        public decimal CurrLps { get; set; }
        public decimal CurrVat { get; set; }
        public string PouroNameBn { get; set; }
        public string CityCorporationNameBn { get; set; }
        public decimal TotalReceiptPrincipal { get; set; }
        public decimal TotalReceiptVat { get; set; }
        public string ReceiptBillMonth { get; set; }
        public decimal PrvReceiptPrincipal { get; set; }
        public decimal PrvReceiptVat { get; set; }
        public decimal PrvReceiptAmt { get; set; }
        public decimal TotalReceiptArrear { get; set; }
        public int OrderNo { get; set; }

    }
    public class OnlineCityPourDataListModel
    {
        public List<OnlineCityPourMergeDTO> CityCoporationDataList { get; set; }
        public List<OnlineCityPourMergeDTO> PouroshovaDataList { get; set; }
        public List<OnlineCityPourMergeDTO> CityPouroshovaDetailsDataList { get; set; }
    }
}
