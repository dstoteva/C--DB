using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("customer")]
    public class CustomerDTO
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }

        [XmlAttribute("bought-cars")]
        public int SalesCount { get; set; }

        [XmlAttribute("spent-money")]
        public decimal Price { get; set; }
    }
}
