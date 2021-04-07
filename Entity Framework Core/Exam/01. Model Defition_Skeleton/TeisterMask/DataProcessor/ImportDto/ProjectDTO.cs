using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ProjectDTO
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }
        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }
        [XmlElement("DueDate")]
        public string DueDate { get; set; }
        [XmlArray("Tasks")]
        public TaskDTO[] Tasks { get; set; }
    }
}
