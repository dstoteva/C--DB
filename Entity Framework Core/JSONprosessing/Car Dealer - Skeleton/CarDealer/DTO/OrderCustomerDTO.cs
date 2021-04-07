using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class OrderCustomerDTO
    {
        public string name { get; set; }

        public DateTime birthDate { get; set; }

        public bool isYoungDriver { get; set; }
    }
}
