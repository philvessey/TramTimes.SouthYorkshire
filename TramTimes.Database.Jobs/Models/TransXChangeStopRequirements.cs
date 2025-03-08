using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "StopRequirements", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeStopRequirements
{
    [UsedImplicitly]
    [XmlElement(elementName: "NoNewStopsRequired")]
    public string? NoNewStopsRequired { get; set; }
}