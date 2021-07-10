using Shop.DataAccess;
using Shop.Dtos;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public class BillsServices : IBillsServices
    {
        private readonly ShopContext _context;
        public BillsServices(ShopContext context)
        {
            _context = context;
        }

        public void AddBill(Bill bill)
        {
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }
            _context.Bills.Add(bill);
        }

        public void AddBillDetails(int billId, List<CartItem> cartItems)
        {
            var _list = createListBillDetails(billId, cartItems);
            _context.BillDetails.AddRange(_list);
        }

        public void ChangesPay(int billId, bool isPay)
        {
            var bill = _context.Bills.Where(b => b.Id == billId).FirstOrDefault();
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }
            bill.IsPay = isPay;
            _context.Bills.Update(bill);

        }

        public void ChangesState(int billId, int state)
        {
            var bill = _context.Bills.Where(b => b.Id == billId).FirstOrDefault();
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }
            bill.State = state;
            _context.Bills.Update(bill);

        }

        public List<BillDetails> createListBillDetails(int billId, List<CartItem> cartItems)
        {
            List<BillDetails> list = new List<BillDetails>();
            if (billId == 0 && cartItems.Count == 0)
            {
                throw new ArgumentNullException("No bill details");
            }
            foreach (var cartItem in cartItems)
            {
                list.Add(new BillDetails(billId, cartItem.ProductId, cartItem.Quantity));
            }
            return list;
        }

        public Bill GetBill(int id)
        {
            var bill = _context.Bills.Where(b => b.Id == id).FirstOrDefault();
            if(bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }
            return bill;
        }

        public List<BillDetails> getBillDetails(int billId)
        {
            var billDetails = _context.BillDetails.Where(b => b.BillId == billId).ToList();
            return billDetails;
        }

        public List<Bill> GetBillsByPhone(String phone)
        {
            var result = (from b in _context.Bills
                         join u in _context.Users
                         on b.UserId equals u.Id
                         where u.Phone == phone
                         select new Bill()
                         {
                             Id = b.Id,
                             Date = b.Date,
                             UserId = b.UserId,
                             TotalPrice = b.TotalPrice,
                             State = b.State,
                             IsPay = b.IsPay,

                         }).ToList();

            return result;
        }

        public List<Bill> GetBillsByState(int userId, int state)
        {
            if(userId ==0)
            {
                throw new ArgumentNullException();
            }
            var list = _context.Bills.Where(b => b.State == state && b.UserId == userId).ToList();
            return list;
        }

        public List<Bill> GetBillsByStateForAdmin(int state)
        {
            var list = _context.Bills.Where(b => b.State == state).ToList();
            return list;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
