﻿using System;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class CustomerExportTO
    {
        [XmlAttribute("FirstName")]
        public string FirstName { get; set; }
        [XmlAttribute("LastName")]
        public string LastName { get; set; }
        [XmlElement("SpentMoney")]
        public string SpetMoney { get; set; }
        [XmlElement("SpentTime")]
        public string SpentTime { get; set; }
    }
}
