using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "TransXChange", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChange
{
    [UsedImplicitly]
    [XmlElement(elementName: "StopPoints")]
    public TransXChangeStopPoints? StopPoints { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "RouteSections")]
    public TransXChangeRouteSections? RouteSections { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Routes")]
    public TransXChangeRoutes? Routes { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "JourneyPatternSections")]
    public TransXChangeJourneyPatternSections? JourneyPatternSections { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Operators")]
    public TransXChangeOperators? Operators { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Services")]
    public TransXChangeServices? Services { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "VehicleJourneys")]
    public TransXChangeVehicleJourneys? VehicleJourneys { get; set; }
}