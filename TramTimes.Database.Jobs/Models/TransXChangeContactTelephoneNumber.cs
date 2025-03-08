using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "ContactTelephoneNumber", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeContactTelephoneNumber
{
    [UsedImplicitly]
    [XmlElement(elementName: "TelNationalNumber")]
    public string? TelNationalNumber { get; set; }
}