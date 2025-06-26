using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Shared.DTOs.Response
{
    [XmlRoot(ElementName = "result")]
    public class CustomerInformation
    {
        [XmlAttribute(AttributeName = "state")]
        public string State { get; set; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }


        [XmlElement(ElementName = "customerNo")]
        public string CustomerNo { get; set; }

        [XmlElement(ElementName = "customerName")]
        public string CustomerName { get; set; }

        [XmlElement(ElementName = "customerType")]
        public string CustomerType { get; set; }

        [XmlElement(ElementName = "division")]
        public string Division { get; set; }

        [XmlElement(ElementName = "fatherName")]
        public string FatherName { get; set; }

        [XmlElement(ElementName = "motherName")]
        public string MotherName { get; set; }

        [XmlElement(ElementName = "spouseName")]
        public string SpouseName { get; set; }

        [XmlElement(ElementName = "sex")]
        public string Sex { get; set; }

        [XmlElement(ElementName = "nid")]
        public string Nid { get; set; }

        [XmlElement(ElementName = "tinno")]
        public string TinNo { get; set; }

        [XmlElement(ElementName = "email")]
        public string Email { get; set; }

        [XmlElement(ElementName = "isGovtCustomer")]
        public string IsGovtCustomer { get; set; }

        [XmlElement(ElementName = "instituteName")]
        public string InstituteName { get; set; }

        [XmlElement(ElementName = "isFreedomFighter")]
        public string IsFreedomFighter { get; set; }

        [XmlElement(ElementName = "oldCustomerNo")]
        public string OldCustomerNo { get; set; }

        [XmlElement(ElementName = "oldMeterNo")]
        public string OldMeterNo { get; set; }

        [XmlElement(ElementName = "overdue")]
        public string Overdue { get; set; }

        [XmlElement(ElementName = "customerAddress")]
        public string CustomerAddress { get; set; }

        [XmlElement(ElementName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [XmlElement(ElementName = "district")]
        public string District { get; set; }

        [XmlElement(ElementName = "thana")]
        public string Thana { get; set; }

        [XmlElement(ElementName = "powerUtility")]
        public string PowerUtility { get; set; }

        [XmlElement(ElementName = "powerCapacity")]
        public string PowerCapacity { get; set; }

        [XmlElement(ElementName = "electricalProperty")]
        public string ElectricalProperty { get; set; }

        [XmlElement(ElementName = "industry")]
        public string Industry { get; set; }

        [XmlElement(ElementName = "powerSupplyVoltage")]
        public string PowerSupplyVoltage { get; set; }

        [XmlElement(ElementName = "meterReadingDate")]
        public string MeterReadingDate { get; set; }

        [XmlElement(ElementName = "meterReadingTime")]
        public string MeterReadingTime { get; set; }

        [XmlElement(ElementName = "temporaryPower")]
        public string TemporaryPower { get; set; }

        [XmlElement(ElementName = "icCardNo")]
        public string IcCardNo { get; set; }

        [XmlElement(ElementName = "remark")]
        public string Remark { get; set; }

        [XmlElement(ElementName = "maxPower")]
        public string MaxPower { get; set; }

        [XmlElement(ElementName = "meterOwnedBy")]
        public string MeterOwnedBy { get; set; }

        [XmlElement(ElementName = "contacts")]
        public ContactList ContactList { get; set; }

        [XmlElement(ElementName = "transID")]
        public string TransID { get; set; }

    }

    public class ContactList
    {
        [XmlElement(ElementName = "contact")]
        public List<Contacts> ContactsList { get; set; }
    }
    public class Contacts
    {
        [XmlElement(ElementName = "contactName")]
        public string ContactName { get; set; }

        [XmlElement(ElementName = "telephone")]
        public string Telephone { get; set; }

        [XmlElement(ElementName = "mobile")]
        public string Mmobile { get; set; }

        [XmlElement(ElementName = "serviceProviders")]
        public string ServiceProviders { get; set; }
    }
}
