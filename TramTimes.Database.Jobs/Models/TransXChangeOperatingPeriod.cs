using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "OperatingPeriod", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeOperatingPeriod
{
    [UsedImplicitly]
    [XmlElement(elementName: "StartDate")]
    public string? StartDate { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "EndDate")]
    public string? EndDate { get; set; }
}