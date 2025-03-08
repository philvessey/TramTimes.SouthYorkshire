using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "StopPoints", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeStopPoints
{
    [UsedImplicitly]
    [XmlElement(elementName: "AnnotatedStopPointRef")]
    public List<TransXChangeAnnotatedStopPointRef>? AnnotatedStopPointRef { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "StopPoint")]
    public List<TransXChangeStopPoint>? StopPoint { get; set; }
}