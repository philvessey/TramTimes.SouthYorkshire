using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "StandardService", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeStandardService
{
    [UsedImplicitly]
    [XmlElement(elementName: "Origin")]
    public string? Origin { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Destination")]
    public string? Destination { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Vias")]
    public TransXChangeVias? Vias { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "JourneyPattern")]
    public List<TransXChangeJourneyPattern>? JourneyPattern { get; set; }
}