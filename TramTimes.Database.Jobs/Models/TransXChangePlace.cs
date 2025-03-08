using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Place", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangePlace
{
    [UsedImplicitly]
    [XmlElement(elementName: "NptgLocalityRef")]
    public string? NptgLocalityRef { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "Location")]
    public TransXChangeLocation? Location { get; set; }
}