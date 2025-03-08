using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "VehicleType", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeVehicleType
{
    [UsedImplicitly]
    [XmlElement(elementName: "VehicleEquipment")]
    public TransXChangeVehicleEquipment? VehicleEquipment { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "WheelchairAccessible")]
    public string? WheelchairAccessible { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "VehicleTypeCode")]
    public string? VehicleTypeCode { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Description")]
    public string? Description { get; set; }
}