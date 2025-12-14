using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "StopClassification", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeStopClassification
{
    [UsedImplicitly]
    [XmlElement(elementName: "StopType")]
    public string? StopType { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "OffStreet")]
    public TransXChangeOffStreet? OffStreet { get; set; }
}