using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Services", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeServices
{
    [UsedImplicitly]
    [XmlElement(elementName: "Service")]
    public TransXChangeService? Service { get; set; }
}