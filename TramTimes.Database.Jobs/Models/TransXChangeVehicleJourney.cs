using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "VehicleJourney", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeVehicleJourney
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "SequenceNumber")]
    public string? SequenceNumber { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "PrivateCode")]
    public string? PrivateCode { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Operational")]
    public TransXChangeOperational? Operational { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "OperatingProfile")]
    public TransXChangeOperatingProfile? OperatingProfile { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "VehicleJourneyCode")]
    public string? VehicleJourneyCode { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "ServiceRef")]
    public string? ServiceRef { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "OperatorRef")]
    public string? OperatorRef { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "LineRef")]
    public string? LineRef { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "JourneyPatternRef")]
    public string? JourneyPatternRef { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "DepartureTime")]
    public string? DepartureTime { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "DepartureDayShift")]
    public string? DepartureDayShift { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "VehicleJourneyTimingLink")]
    public List<TransXChangeVehicleJourneyTimingLink>? VehicleJourneyTimingLink { get; set; }
}