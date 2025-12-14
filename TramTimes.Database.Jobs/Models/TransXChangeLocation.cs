using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Location", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeLocation
{
    [UsedImplicitly]
    [XmlElement(elementName: "Easting")]
    public string? Easting { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Northing")]
    public string? Northing { get; set; }
}