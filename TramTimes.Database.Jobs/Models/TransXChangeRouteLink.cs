using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "RouteLink", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeRouteLink
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
    [XmlElement(elementName: "Distance")]
    public string? Distance { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Direction")]
    public string? Direction { get; set; }
}