using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Line", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeLine
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "LineName")]
    public string? LineName { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "OutboundDescription")]
    public TransXChangeOutboundDescription? OutboundDescription { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "InboundDescription")]
    public TransXChangeInboundDescription? InboundDescription { get; set; }
}