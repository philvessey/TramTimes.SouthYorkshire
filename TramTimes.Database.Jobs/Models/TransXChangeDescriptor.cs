using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Descriptor", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeDescriptor
{
    [UsedImplicitly]
    [XmlElement(elementName: "CommonName")]
    public string? CommonName { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "ShortCommonName")]
    public string? ShortCommonName { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Landmark")]
    public string? Landmark { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Street")]
    public string? Street { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Crossing")]
    public string? Crossing { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Indicator")]
    public string? Indicator { get; set; }
}