using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Dtos
{
    public class ProductCreateDto
    {
       
        public string Name { get; set; }

        public string Category { get; set; }

        public List<FilePath> ImgPath { get; set; }
  
        public string Description { get; set; }
   
        public int Quantity { get; set; }

        public int Price { get; set; }

        public ProductCreateDto(string name, string category, List<FilePath> imgPath, string description, int quantity, int price)
        {
            Name = name;
            Category = category;
            ImgPath = imgPath;
            Description = description;
            Quantity = quantity;
            Price = price;
        }
    }

}
