using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "RegularDayType", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeRegularDayType
{
    [UsedImplicitly]
    [XmlElement(elementName: "DaysOfWeek")]
    public TransXChangeDaysOfWeek? DaysOfWeek { get; set; }
}