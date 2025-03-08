using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "RouteSections", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeRouteSections
{
    [UsedImplicitly]
    [XmlElement(elementName: "RouteSection")]
    public List<TransXChangeRouteSection>? RouteSection { get; set; }
}