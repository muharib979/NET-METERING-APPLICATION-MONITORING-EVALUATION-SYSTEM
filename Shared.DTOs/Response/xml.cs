using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Shared.DTOs.Response
{
    [XmlRoot(ElementName = "xml")]
    public class XML
    {
        [XmlAttribute(AttributeName = "state")]
        public string State { get; set; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }

        [XmlAttribute(AttributeName = "prepaidCustomerNumber")]
        public string PrepaidCustomerNumber { get; set; }
        [XmlAttribute(AttributeName = "transID")]
        public string TransId { get; set; }
    }
}
