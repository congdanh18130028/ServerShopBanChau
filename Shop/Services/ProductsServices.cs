﻿using Microsoft.EntityFrameworkCore;
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
            var _products = _context.Products.OrderBy(p => p.Price).ToList();
            return _products;
        }

        public List<Product> GetProductsByCategory(string category)
        {
            var _products = _context.Products.Where(p => p.Category == category).ToList();
            return _products;
        }

        public List<Product> GetProductsByPrice(int price1, int price2)
        {
            var _products = _context.Products.Where(p => p.Price >= price1 && p.Price <= price2).ToList();
            return _products;
        }

        public List<Product> GetProductsDescPrice()
        {
            var _products = _context.Products.OrderByDescending(p => p.Price).ToList();
            return _products;
        }

        public List<Product> GetProductsSearch(string value)
        {
            var _products = _context.Products.Where(p => p.Name.Contains(value)).ToList();
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
