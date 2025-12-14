using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "VehicleEquipment", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeVehicleEquipment
{
    [UsedImplicitly]
    [XmlElement(elementName: "AccessVehicleEquipment")]
    public string? AccessVehicleEquipment { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "WheelchairVehicleEquipment")]
    public TransXChangeWheelchairVehicleEquipment? WheelchairVehicleEquipment { get; set; }
}