using System.Xml.Serialization;
using JetBrains.Annotations;

namespace TramTimes.Database.Jobs.Models;

[XmlRoot(ElementName = "Operator", Namespace = "http://www.transxchange.org.uk/")]
public class TransXChangeOperator
{
    [UsedImplicitly]
    [XmlAttribute(attributeName: "id")]
    public string? Id { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "NationalOperatorCode")]
    public string? NationalOperatorCode { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "OperatorCode")]
    public string? OperatorCode { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "OperatorShortName")]
    public string? OperatorShortName { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "OperatorNameOnLicence")]
    public string? OperatorNameOnLicence { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "TradingName")]
    public string? TradingName { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "LicenceNumber")]
    public string? LicenceNumber { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "LicenceClassification")]
    public string? LicenceClassification { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "EnquiryTelephoneNumber")]
    public TransXChangeEnquiryTelephoneNumber? EnquiryTelephoneNumber { get; set; }

    [UsedImplicitly]
    [XmlElement(elementName: "ContactTelephoneNumber")]
    public TransXChangeContactTelephoneNumber? ContactTelephoneNumber { get; set; }
}