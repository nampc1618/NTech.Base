using NTech.Base.Commons.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NTech.Xm.Station.Models
{
    [XmlRoot(ElementName = "IPPrinterList")]
    public class IPPrinterList
    {
        [XmlElement(ElementName = "IPAddressPrinterList")]
        public List<string> IPAddressPrinterList { get; set; }
    }
}
