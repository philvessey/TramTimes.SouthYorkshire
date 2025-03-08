using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "BankHolidayOperation", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeBankHolidayOperation
{
    [UsedImplicitly]
    [XmlElement(elementName: "DaysOfOperation")]
    public TransXChangeDaysOfOperation? DaysOfOperation { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "DaysOfNonOperation")]
    public TransXChangeDaysOfNonOperation? DaysOfNonOperation { get; set; }
}