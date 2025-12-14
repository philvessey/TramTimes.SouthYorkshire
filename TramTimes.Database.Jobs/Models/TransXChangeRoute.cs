using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Route", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeRoute
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "PrivateCode")]
    public string? PrivateCode { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Description")]
    public string? Description { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "RouteSectionRef")]
    public List<string>? RouteSectionRef { get; set; }
}