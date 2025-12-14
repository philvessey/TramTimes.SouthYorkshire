using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "StopPoint", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeStopPoint
{
    [UsedImplicitly]
    [XmlElement(elementName: "AtcoCode")]
    public string? AtcoCode { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Descriptor")]
    public TransXChangeDescriptor? Descriptor { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Place")]
    public TransXChangePlace? Place { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "StopClassification")]
    public TransXChangeStopClassification? StopClassification { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "AdministrativeAreaRef")]
    public string? AdministrativeAreaRef { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Notes")]
    public string? Notes { get; set; }
}