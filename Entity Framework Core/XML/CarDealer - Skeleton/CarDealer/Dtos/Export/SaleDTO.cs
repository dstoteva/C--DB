using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("sale")]
    public class SaleDTO
    {
        [XmlElement("car")]
        public CarDTO Car { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string Customer { get; set; }

        [XmlElement("price")]
        public double Price { get; set; }

        [XmlElement("price-with-discount")]
        public double PriceWithDiscount { get; set; }
    }
}
