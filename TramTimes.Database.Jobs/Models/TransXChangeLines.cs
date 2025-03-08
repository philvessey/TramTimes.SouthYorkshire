using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Lines", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeLines
{
    [UsedImplicitly]
    [XmlElement(elementName: "Line")]
    public TransXChangeLine? Line { get; set; }
}