using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Routes", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeRoutes
{
    [UsedImplicitly]
    [XmlElement(elementName: "Route")]
    public List<TransXChangeRoute>? Route { get; set; }
}