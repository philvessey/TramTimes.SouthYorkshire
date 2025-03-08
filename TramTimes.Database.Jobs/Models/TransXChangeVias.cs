using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Vias", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeVias
{
    [UsedImplicitly]
    [XmlElement(elementName: "Via")]
    public string? Via { get; set; }
}