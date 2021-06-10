using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public interface IBillsServices
    {
        public void AddBill(Bill bill);
        public Bill GetBill(int id);
        public List<Bill> GetBillsByStateForAdmin(int state);
        public List<Bill> GetBillsByState(int userId, int state);
        public void ChangesState(int billId, int state);
        public void ChangesPay(int billId, Boolean isPay);
        public void AddBillDetails(int billId, List<CartItem> cartItems);
        public void SaveChanges();
    }
}
