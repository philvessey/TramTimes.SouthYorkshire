using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "RouteSection", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeRouteSection
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "RouteLink")]
    public List<TransXChangeRouteLink>? RouteLink { get; set; }
}