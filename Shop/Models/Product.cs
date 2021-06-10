using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public String Name { get; set; }
        [Required]
        public String Category { get; set; }
        [Required]
        public List<FilePath> ImgPath { get; set; }
        [Required]
        public String Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }

    }
}
