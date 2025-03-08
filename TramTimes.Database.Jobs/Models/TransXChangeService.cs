using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Service", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeService
{
    [UsedImplicitly]
    [XmlElement(elementName: "ServiceCode")]
    public string? ServiceCode { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "PrivateCode")]
    public string? PrivateCode { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Lines")]
    public TransXChangeLines? Lines { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "OperatingPeriod")]
    public TransXChangeOperatingPeriod? OperatingPeriod { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "OperatingProfile")]
    public TransXChangeOperatingProfile? OperatingProfile { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "VehicleType")]
    public TransXChangeVehicleType? VehicleType { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "RegisteredOperatorRef")]
    public string? RegisteredOperatorRef { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "StopRequirements")]
    public TransXChangeStopRequirements? StopRequirements { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Mode")]
    public string? Mode { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "PublicUse")]
    public string? PublicUse { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Description")]
    public string? Description { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "StandardService")]
    public TransXChangeStandardService? StandardService { get; set; }
}