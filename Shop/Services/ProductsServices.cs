using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public class ProductsServices : IProductsServices
    {
        private readonly ShopContext _context;

        public ProductsServices(ShopContext context)
        {
            _context = context;
        }

        public void AddCaterory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Add(category);
        }

        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Add(product);

            _context.FilePaths.AddRange(product.ImgPath);
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.FilePaths.RemoveRange(product.ImgPath);
            _context.Products.Remove(product);
        }

        public List<Category> GetCategories()
        {
            var categories = _context.Categories.ToList<Category>();
            return categories;
        }

        public Product GetProduct(int id)
        {
            var product = _context.Products
                .Where(p => p.Id == id)
                .Include(p => p.ImgPath)
                .FirstOrDefault();
            return product;
        }

        public List<Product> GetProducts()
        {
            return _context.Products
                .Include(p =>p.ImgPath)
                .ToList<Product>();
        }

        public List<Product> GetProductsAscPrice()
        {
            var _products = _context.Products.OrderBy(p => p.Price).Include(p => p.ImgPath).ToList();
            return _products;
        }

        public List<Product> GetProductsByCategory(string category)
        {
            var _products = _context.Products.Where(p => p.Category == category).Include(p => p.ImgPath).ToList();
            return _products;
        }

        public List<Product> GetProductsByPrice(int price1, int price2)
        {
            var _products = _context.Products.Where(p => p.Price >= price1 && p.Price <= price2).Include(p => p.ImgPath).ToList();
            return _products;
        }

        public List<Product> GetProductsCategoryAscPrice(string category)
        {
            var _products = _context.Products.Where(p => p.Category == category).OrderBy(p => p.Price).Include(p => p.ImgPath).ToList();
            return _products;
        }

        public List<Product> GetProductsCategoryByPrice(string category, int price1, int price2)
        {
            var _products = _context.Products.Where(p => p.Price >= price1 && p.Price <= price2 && p.Category == category).Include(p => p.ImgPath).ToList();
            return _products;
        }

        public List<Product> GetProductsCategoryDescPrice(string category)
        {
            var _products = _context.Products.Where(p => p.Category == category).OrderByDescending(p => p.Price).Include(p => p.ImgPath).ToList();
            return _products;
        }

       
        public List<Product> GetProductsDescPrice()
        {
            var _products = _context.Products.OrderByDescending(p => p.Price).Include(p => p.ImgPath).ToList();
            return _products;
        }


        public List<Product> GetProductsSearch(string value)
        {
            var _products = _context.Products.Where(p => p.Name.Contains(value)).Include(p => p.ImgPath).ToList();
            return _products;
            
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void UppdateProduct(Product product)
        {
            _context.FilePaths.UpdateRange(product.ImgPath);
            _context.Products.Update(product);
        }
    }
}
