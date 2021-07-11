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
        public List<BillDetails> getBillDetails(int billId);
        public List<Bill> GetBills();
        public List<Bill> GetBillsDate(DateTime date1, DateTime date2);
        public List<Bill> GetBillsDateState(int state, DateTime date1, DateTime date2);
        public List<Bill> GetBillsPay(DateTime date1, DateTime date2);
        public List<Bill> GetBillsNoPay(DateTime date1, DateTime date2);
    }
}
