using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Operational", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeOperational
{
    [UsedImplicitly]
    [XmlElement(elementName: "VehicleType")]
    public TransXChangeVehicleType? VehicleType { get; set; }
}