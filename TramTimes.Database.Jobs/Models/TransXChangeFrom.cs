using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "From", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeFrom
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "SequenceNumber")]
    public string? SequenceNumber { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "StopPointRef")]
    public string? StopPointRef { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "TimingStatus")]
    public string? TimingStatus { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Activity")]
    public string? Activity { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "DynamicDestinationDisplay")]
    public string? DynamicDestinationDisplay { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "WaitTime")]
    public string? WaitTime { get; set; }
}