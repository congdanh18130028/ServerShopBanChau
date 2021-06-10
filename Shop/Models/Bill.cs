using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime Date { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TotalPrice { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        public Boolean IsPay { get; set; }
        public User User { get; set; }

    }
}
