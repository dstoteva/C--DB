using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Projection")]
    public class ProjectionDTO
    {
        [XmlElement("MovieId")]
        public int MovieId { get; set; }
        [XmlElement("HallId")]
        public int HallId { get; set; }
        [XmlElement("DateTime")]
        [Required]
        public string DateTime { get; set; }
    }
}
