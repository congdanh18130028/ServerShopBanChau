using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Category
    {
       

        [Key]
        public int Id { get; set; }
        public String CategoryName { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
