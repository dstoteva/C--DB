﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Projection
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int HallId { get; set; }
        public Hall Hall { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
