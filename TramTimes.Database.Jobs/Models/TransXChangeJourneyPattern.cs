using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "JourneyPattern", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeJourneyPattern
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "DestinationDisplay")]
    public string? DestinationDisplay { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Direction")]
    public string? Direction { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Operational")]
    public TransXChangeOperational? Operational { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "RouteRef")]
    public string? RouteRef { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "JourneyPatternSectionRefs")]
    public List<string>? JourneyPatternSectionRefs { get; set; }
}