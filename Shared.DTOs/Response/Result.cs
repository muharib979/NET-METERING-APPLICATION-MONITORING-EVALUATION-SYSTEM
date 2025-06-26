using System.Xml.Serialization;

namespace Shared.DTOs.Response
{

    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlAttribute(AttributeName = "state")]
        public string State { get; set; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }

        [XmlAttribute(AttributeName = "transID")]
        public string TransId { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }

        [XmlElement(ElementName = "tariffs")]
        public Tariffs Tariffs { get; set; }
        [XmlElement(ElementName = "customerList")]
        public CustomerList CustomerList { get; set; }
    }

    public class Tariffs
    {
        [XmlElement(ElementName = "tariff")]
        public List<Tariff> TariffList { get; set; }
    }

    public class CustomerList
    {
        [XmlElement(ElementName = "customer")]
        public List<Customers> CustomersList { get; set; }
    }

    public class Tariff
    {

        [XmlElement(ElementName = "tariffName")]
        public string TariffName { get; set; }

        [XmlElement(ElementName = "noOfCustomer")]
        public int NoOfCustomer { get; set; }

        //[XmlElement(ElementName = "vendCust")]
        //public int VendCust { get; set; }

        [XmlElement(ElementName = "vendingCustomer")]
        public int VendingCustomer { get; set; }

        [XmlElement(ElementName = "soldUnit")]
        public decimal SoldUnit { get; set; }

        [XmlElement(ElementName = "soldAmountVat")]
        public decimal SoldAmountVat { get; set; }

        [XmlElement(ElementName = "meterRent1p")]
        public decimal MeterRent1p { get; set; }

        [XmlElement(ElementName = "meterRent3p")]
        public decimal MeterRent3p { get; set; }


        [XmlElement(ElementName = "soldAmountWithVatMeterRent")]
        public decimal SoldAmountWithVatMeterRent { get; set; }

        [XmlElement(ElementName = "soldAmountWithoutVatMeterRent")]
        public decimal SoldAmount { get; set; }

        [XmlElement(ElementName = "collectionAmountVat")]
        public decimal CollectionAmountVat { get; set; }

        [XmlElement(ElementName = "collectionAmountWithoutVatMeterRent")]
        public decimal CollectionAmount { get; set; }
    }

    public class Customers
    {

        [XmlElement(ElementName = "transId")]
        public string TransId { get; set; }

        [XmlElement(ElementName = "prepaidCustomerNumber")]
        public string PrepaidCustomerNumber { get; set; }

        [XmlElement(ElementName = "oldCustomerNo")]
        public string CustomerNumber { get; set; }

        [XmlElement(ElementName = "customerName")]
        public string CustomerName { get; set; }

        [XmlElement(ElementName = "nidNumber")]
        public string NidNumber { get; set; }

        [XmlElement(ElementName = "mobileNo")]
        public string MobileNo { get; set; }

        [XmlElement(ElementName = "powerUtility")]
        public string PowerUtility { get; set; }

        [XmlElement(ElementName = "MAXPower")]
        public string MaxPower { get; set; }

        [XmlElement(ElementName = "oldMeterNo")]
        public string OldMeterNo { get; set; }

        [XmlElement(ElementName = "customercreatedDate")]
        public string CustomerCreatedDate { get; set; }
        public string LocationName { get; set; }

        

    }
    public class DeptCodeModel
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
    }
}