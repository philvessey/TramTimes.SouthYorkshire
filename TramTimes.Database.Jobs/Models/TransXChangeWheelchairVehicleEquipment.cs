using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "WheelchairVehicleEquipment", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeWheelchairVehicleEquipment
{
    [UsedImplicitly]
    [XmlElement(elementName: "SuitableFor")]
    public string? SuitableFor { get; set; }
}