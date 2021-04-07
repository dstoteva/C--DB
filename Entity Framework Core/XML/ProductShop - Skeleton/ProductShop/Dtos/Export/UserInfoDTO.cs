using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class UserInfoDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        [XmlArrayItem("User")]
        public UserDTO[] Users { get; set; }
    }
}
