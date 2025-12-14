using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "JourneyPatternSection", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeJourneyPatternSection
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "JourneyPatternTimingLink")]
    public List<TransXChangeJourneyPatternTimingLink>? JourneyPatternTimingLink { get; set; }
}