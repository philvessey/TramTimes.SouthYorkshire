using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Operators", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeOperators
{
    [UsedImplicitly]
    [XmlElement(elementName: "Operator")]
    public TransXChangeOperator? Operator { get; set; }
}