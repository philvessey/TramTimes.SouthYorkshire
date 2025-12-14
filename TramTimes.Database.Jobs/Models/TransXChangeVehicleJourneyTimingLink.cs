using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "VehicleJourneyTimingLink", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeVehicleJourneyTimingLink
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "JourneyPatternTimingLinkRef")]
    public string? JourneyPatternTimingLinkRef { get; set; }
}