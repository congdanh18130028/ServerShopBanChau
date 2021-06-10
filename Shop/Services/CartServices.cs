using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;
using Shop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public class CartServices : ICartServices
    {
        private readonly ShopContext _context;
        public CartServices(ShopContext context)
        {
            _context = context;
        }
        public void AddItem(CartItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            CartItem cartItem = _context.CartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (cartItem == null)
            {
                _context.CartItems.Add(item);
            }
            else
            {
                IncreaseItem(cartItem.Id);
                SaveChanges();
            }


        }

        public void ClearCartItems(int cartId)
        {
            var listItem = GetItems(cartId);
            _context.CartItems.RemoveRange(listItem);

        }

        public void CreateCart(int UId)
        {
            if (UId == 0)
            {
                throw new ArgumentNullException(nameof(UId));
            }
            Cart c = new Cart();
            c.UserID = UId;
            _context.Carts.Add(c);

        }

        public void DecreaseItem(int itemId)
        {
            if (itemId == 0)
            {
                throw new ArgumentNullException(nameof(itemId));
            }
            var _cartItem = _context.CartItems.FirstOrDefault(i => i.Id == itemId);
            if (_cartItem == null)
            {
                throw new ArgumentNullException(nameof(_cartItem));
            }
            int quantity = _cartItem.Quantity;
            if (quantity > 1)
            {
                _cartItem.Quantity -= 1;
                _context.CartItems.Update(_cartItem);
            }
            else
            {
                RemoveItem(itemId);
                SaveChanges();
            }


        }

        public void DropCart(int UId)
        {
            if (UId == 0)
            {
                throw new ArgumentNullException(nameof(UId));
            }
            var _cart = _context.Carts.FirstOrDefault(c => c.UserID == UId);
            if (_cart == null)
            {
                throw new ArgumentNullException(nameof(UId));
            }
            if (_cart.CartItems != null)
            {
                _context.CartItems.RemoveRange(_cart.CartItems);

            }
            _context.Carts.Remove(_cart);
        }

        public int getCartId(int userId)
        {
            var cart = _context.Carts.Where(c => c.UserID == userId).FirstOrDefault();
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart));
            }

            return cart.Id;



        }

        public CartItem GetItem(int id, int cartId)
        {
            return _context.CartItems.Where(i => i.CartId == cartId && i.Id == id)
                                     .FirstOrDefault();
        }

        public List<CartItem> GetItems(int cartId)
        {

            return _context.CartItems.Where(i => i.CartId == cartId).ToList();

        }

        public void IncreaseItem(int itemId)
        {
            if (itemId == 0)
            {
                throw new ArgumentNullException(nameof(itemId));
            }
            var _cartItem = _context.CartItems.FirstOrDefault(i => i.Id == itemId);
            if (_cartItem == null)
            {
                throw new ArgumentNullException(nameof(_cartItem));
            }
            _cartItem.Quantity += 1;
            _context.CartItems.Update(_cartItem);

        }

        public void RemoveItem(int itemId)
        {
            if (itemId == 0)
            {
                throw new ArgumentNullException(nameof(itemId));
            }
            var _cartItem = _context.CartItems.FirstOrDefault(i => i.Id == itemId);
            if (_cartItem == null)
            {
                throw new ArgumentNullException(nameof(_cartItem));
            }
            _context.CartItems.Remove(_cartItem);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public int TotalPrice(int cartId, int ship, int discount)
        {
            var result = from i in _context.CartItems
                         join p in _context.Products
                         on i.ProductId equals p.Id 
                         where i.CartId == cartId
                         select new Total(){price = p.Price, quantity = i.Quantity};
            return result.Sum(r => r.quantity * r.price) + ship + discount;
           
        }
    
}}