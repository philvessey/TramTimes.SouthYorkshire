using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "AnnotatedStopPointRef", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeAnnotatedStopPointRef
{
    [UsedImplicitly]
    [XmlElement(elementName: "StopPointRef")]
    public string? StopPointRef { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "CommonName")]
    public string? CommonName { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Indicator")]
    public string? Indicator { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "LocalityName")]
    public string? LocalityName { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "LocalityQualifier")]
    public string? LocalityQualifier { get; set; }
}