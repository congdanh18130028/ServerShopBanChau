using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public List<CartItem> CartItems { get; set; }
        public User User { get; set; }

    }
}
