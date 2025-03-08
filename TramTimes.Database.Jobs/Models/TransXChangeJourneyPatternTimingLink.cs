using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "JourneyPatternTimingLink", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeJourneyPatternTimingLink
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "From")]
    public TransXChangeFrom? From { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "To")]
    public TransXChangeTo? To { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "RouteLinkRef")]
    public string? RouteLinkRef { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "RunTime")]
    public string? RunTime { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Direction")]
    public string? Direction { get; set; }
}