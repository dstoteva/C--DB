using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }
        [Range(12, 110)]
        public int Age { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Balance { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
