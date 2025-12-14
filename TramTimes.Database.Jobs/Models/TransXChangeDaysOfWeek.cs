using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "DaysOfWeek", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeDaysOfWeek
{
    [UsedImplicitly]
    [XmlElement(elementName: "MondayToFriday")]
    public string? MondayToFriday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "MondayToSaturday")]
    public string? MondayToSaturday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "MondayToSunday")]
    public string? MondayToSunday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Weekend")]
    public string? Weekend { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NotMonday")]
    public string? NotMonday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NotTuesday")]
    public string? NotTuesday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NotWednesday")]
    public string? NotWednesday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NotThursday")]
    public string? NotThursday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NotFriday")]
    public string? NotFriday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NotSaturday")]
    public string? NotSaturday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NotSunday")]
    public string? NotSunday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Monday")]
    public string? Monday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Tuesday")]
    public string? Tuesday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Wednesday")]
    public string? Wednesday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Thursday")]
    public string? Thursday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Friday")]
    public string? Friday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Saturday")]
    public string? Saturday { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "Sunday")]
    public string? Sunday { get; set; }
}