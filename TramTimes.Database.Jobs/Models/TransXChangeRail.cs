using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Rail", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeRail
{
    [UsedImplicitly]
    [XmlElement(elementName: "Platform")]
    public string? Platform { get; set; }
}