using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class ProductInfoDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        [XmlArrayItem("Product")]
        public ProductDTO[] Products { get; set; }
    }
}
