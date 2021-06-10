using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Dtos
{
    public class ProductUpdateDto
    {
        public String Name { get; set; }

        public String Category { get; set; }

        public List<FilePath> ImgPath { get; set; }

        public String Description { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
