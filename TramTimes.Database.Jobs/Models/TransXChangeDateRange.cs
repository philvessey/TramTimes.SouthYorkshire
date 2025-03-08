using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "DateRange", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeDateRange
{
    [UsedImplicitly]
    [XmlElement(elementName: "StartDate")]
    public string? StartDate { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "EndDate")]
    public string? EndDate { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Note")]
    public string? Note { get; set; }
}