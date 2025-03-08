using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "EnquiryTelephoneNumber", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeEnquiryTelephoneNumber
{
    [UsedImplicitly]
    [XmlElement(elementName: "TelNationalNumber")]
    public string? TelNationalNumber { get; set; }
}