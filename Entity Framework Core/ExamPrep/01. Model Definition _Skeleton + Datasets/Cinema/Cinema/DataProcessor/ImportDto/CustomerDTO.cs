using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class CustomerDTO
    {
        [XmlElement("FirstName")]
        public string FirstName { get; set; }
        [XmlElement("LastName")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }
        [XmlElement("Age")]
        [Range(12, 110)]
        public int Age { get; set; }
        [XmlElement("Balance")]
        [Range(0.01, double.MaxValue)]
        public decimal Balance { get; set; }
        [XmlArray("Tickets")]
        public TicketDTO[] Tickets { get; set; }
    }
}
