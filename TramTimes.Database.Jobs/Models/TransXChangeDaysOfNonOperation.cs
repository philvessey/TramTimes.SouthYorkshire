using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "DaysOfNonOperation", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeDaysOfNonOperation
{
	[UsedImplicitly]
	[XmlElement(elementName: "AllBankHolidays")]
	public string? AllBankHolidays { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "AllHolidaysExceptChristmas")]
	public string? AllHolidaysExceptChristmas { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "Christmas")]
	public string? Christmas { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "DisplacementHolidays")]
	public string? DisplacementHolidays { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "EarlyRunOff")]
	public string? EarlyRunOff { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "HolidayMondays")]
	public string? HolidayMondays { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "Holidays")]
	public string? Holidays { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "NewYearsDay")]
	public string? NewYearsDay { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "NewYearsDayHoliday")]
	public string? NewYearsDayHoliday { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "Jan2ndScotland")]
	public string? JanSecondScotland { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "Jan2ndScotlandHoliday")]
	public string? JanSecondScotlandHoliday { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "GoodFriday")]
	public string? GoodFriday { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "EasterMonday")]
	public string? EasterMonday { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "MayDay")]
	public string? MayDay { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "SpringBank")]
	public string? SpringBank { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "AugustBankHolidayScotland")]
	public string? AugustBankHolidayScotland { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "LateSummerBankHolidayNotScotland")]
	public string? LateSummerBankHolidayNotScotland { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "StAndrewsDay")]
	public string? StAndrewsDay { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "StAndrewsDayHoliday")]
	public string? StAndrewsDayHoliday { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "ChristmasEve")]
	public string? ChristmasEve { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "ChristmasDay")]
	public string? ChristmasDay { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "ChristmasDayHoliday")]
	public string? ChristmasDayHoliday { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "BoxingDay")]
	public string? BoxingDay { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "BoxingDayHoliday")]
	public string? BoxingDayHoliday { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "NewYearsEve")]
	public string? NewYearsEve { get; set; }
	
	[UsedImplicitly]
	[XmlElement(elementName: "DateRange")]
	public TransXChangeDateRange? DateRange { get; set; }
}