using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public interface IProductsServices
    {
        public List<Product> GetProducts();
        public Product GetProduct(int id);
        public void AddProduct(Product product);
        public void UppdateProduct(Product product);
        public void DeleteProduct(Product product);
        public void SaveChanges();
    }
}
