using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "OutboundDescription", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeOutboundDescription
{
    [UsedImplicitly]
    [XmlElement(elementName: "Origin")]
    public string? Origin { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Destination")]
    public string? Destination { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Description")]
    public string? Description { get; set; }
}