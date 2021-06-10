using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class BillDetails
    {
     
        [Key]
        public int Id { get; set; }
        [Required]
        public int BillId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Bill Bill { get; set; }
        public Product Product { get; set; }

        public BillDetails(int billId, int productId, int quantity)
        {
            BillId = billId;
            ProductId = productId;
            Quantity = quantity;
        }


    }
}
