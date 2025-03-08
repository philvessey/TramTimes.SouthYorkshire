using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "OffStreet", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeOffStreet
{
    [UsedImplicitly]
    [XmlElement(elementName: "Rail")]
    public TransXChangeRail? Rail { get; set; }
}