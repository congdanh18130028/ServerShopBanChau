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
        public List<Product> GetProductsSearch(string value);
        public List<Product> GetProductsByCategory(string category);
        public List<Product> GetProductsByPrice(int price1, int price2);
        public List<Product> GetProductsDescPrice();
        public List<Product> GetProductsAscPrice();
        public List<Product> GetProductsCategoryDescPrice(string category);
        public List<Product> GetProductsCategoryAscPrice(string category);
        public List<Product> GetProductsCategoryByPrice(string category, int price1, int price2);
        public void AddCaterory(Category category);
        public List<Category> GetCategories();
    }
}
