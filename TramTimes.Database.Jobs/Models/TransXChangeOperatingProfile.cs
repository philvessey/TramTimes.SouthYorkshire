using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "OperatingProfile", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeOperatingProfile
{
    [UsedImplicitly]
    [XmlElement(elementName: "RegularDayType")]
    public TransXChangeRegularDayType? RegularDayType { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "BankHolidayOperation")]
    public TransXChangeBankHolidayOperation? BankHolidayOperation { get; set; }
    
    [UsedImplicitly]
    [XmlElement(elementName: "SpecialDaysOperation")]
    public TransXChangeSpecialDaysOperation? SpecialDaysOperation { get; set; }
}