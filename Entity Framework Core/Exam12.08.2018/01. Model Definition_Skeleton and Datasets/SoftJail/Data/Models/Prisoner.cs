﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }
        [Required]
        [RegularExpression(@"The [A-Z][a-z]*")]
        public string Nickname { get; set; }
        [Range(18, 65)]
        public int Age { get; set; }
        [Required]
        public DateTime IncarcerationDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        [Range(typeof(decimal), "0.0", "1000000000000000000")]
        public decimal Bail { get; set; }
        public int CellId { get; set; }
        public Cell Cell { get; set; }
        public ICollection<Mail> Mails { get; set; } = new HashSet<Mail>();
        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; } = new HashSet<OfficerPrisoner>();
    }
}