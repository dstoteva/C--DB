using SoftJail.Data.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FullName { get; set; }
        [Range(typeof(decimal), "0.0", "1000000000000000000")]
        public decimal Salary { get; set; }
        [Required]
        public Position Position { get; set; }
        [Required]
        public Weapon Weapon { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; } = new HashSet<OfficerPrisoner>();
    }
}
