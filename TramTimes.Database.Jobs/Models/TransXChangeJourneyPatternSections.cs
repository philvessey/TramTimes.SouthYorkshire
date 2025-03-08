using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "JourneyPatternSections", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeJourneyPatternSections
{
    [UsedImplicitly]
    [XmlElement(elementName: "JourneyPatternSection")]
    public List<TransXChangeJourneyPatternSection>? JourneyPatternSection { get; set; }
}