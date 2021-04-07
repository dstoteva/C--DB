using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Ticket")]
    public class TicketDTO
    {
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
