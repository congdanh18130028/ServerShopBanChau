using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public interface ICartServices
    {
        public void CreateCart(int UId);
        public void DropCart(int UId);
        public int TotalPrice(int cartId, int ship, int discount);
        public void ClearCartItems(int cartId);
        public List<CartItem> GetItems(int cartId);
        public CartItem GetItem(int id, int cartId);
        public void AddItem(CartItem item);
        public void RemoveItem(int itemId);
        public void IncreaseItem(int itemId);
        public void DecreaseItem(int itemId);
        public void SaveChanges();
        public int getCartId(int userId);
    }
}
