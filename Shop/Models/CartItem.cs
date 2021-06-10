using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public Product Product { get; set; }

    }
}