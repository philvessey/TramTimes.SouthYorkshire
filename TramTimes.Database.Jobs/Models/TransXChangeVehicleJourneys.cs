using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "VehicleJourneys", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeVehicleJourneys
{
    [UsedImplicitly]
    [XmlElement(elementName: "VehicleJourney")]
    public List<TransXChangeVehicleJourney>? VehicleJourney { get; set; }
}