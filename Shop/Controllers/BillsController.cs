using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Dtos;
using Shop.Models;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IBillsServices _billsServices;
        private readonly ICartServices _cartServices;
        private readonly IProductsServices _productsServices;
        private readonly IMapper _mapper;
        public BillsController(IBillsServices billsServices, ICartServices cartServices, IProductsServices productsServices,
            IMapper mapper)
        {
            _billsServices = billsServices;
            _cartServices = cartServices;
            _productsServices = productsServices;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddBill(BillCreateDto bill)
        {
            Boolean check = true;

            var _bill = _mapper.Map<Bill>(bill);
            var cartId = _cartServices.getCartId(_bill.UserId);
            var cartItems = _cartServices.GetItems(cartId);

            foreach (CartItem i in cartItems)
            {
                if(!_productsServices.Check(i.ProductId, i.Quantity))
                {
                    check = false;
                }
                
            }

            if (check)
            {
                _billsServices.AddBill(_bill);
                _billsServices.SaveChanges();
                var billReadDto = _mapper.Map<BillReadDto>(_bill);

                int billId = billReadDto.Id;

                _billsServices.AddBillDetails(billId, cartItems);
                _billsServices.SaveChanges();

                foreach (CartItem i in cartItems)
                {
                    _productsServices.decreaseProduct(i.ProductId, i.Quantity);
                    _productsServices.SaveChanges();
                }

                _cartServices.ClearCartItems(cartId);
                _billsServices.SaveChanges();

                return CreatedAtRoute(nameof(GetBill), new { id = billReadDto.Id }, billReadDto);
            }else
            {

                return NoContent();
            }

           
        }

        [Authorize]
        [HttpGet("billDetails/{billId}")]
        public ActionResult<IEnumerable<BillDetailsReadDto>> getBillDetails(int billId)
        {
            var billDetails = _billsServices.getBillDetails(billId);
            return Ok(_mapper.Map<IEnumerable<BillDetailsReadDto>>(billDetails));
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetBill")]
        public ActionResult GetBill(int id)
        {
            var bill = _billsServices.GetBill(id);
            if (bill != null)
            {
                return Ok(_mapper.Map<BillReadDto>(bill));
            }
            return NotFound($"Bill with id: {id} was not found");
        }

        [Authorize]
        [HttpGet("user")]
        public ActionResult<IEnumerable<BillReadDto>> GetBillsByState(int userId, int state)
        {
            var list = _billsServices.GetBillsByState(userId, state);
            return Ok(_mapper.Map<IEnumerable<BillReadDto>>(list));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("state/{state}")]
        public ActionResult<IEnumerable<BillReadDto>> GetBillsByStateForAdmin(int state)
        {
            var list = _billsServices.GetBillsByStateForAdmin(state);
            return Ok(_mapper.Map<IEnumerable<BillReadDto>>(list));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public ActionResult<IEnumerable<BillReadDto>> GetBills()
        {
            var list = _billsServices.GetBills();
            return Ok(_mapper.Map<IEnumerable<BillReadDto>>(list));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-date")]
        public ActionResult<IEnumerable<BillReadDto>> GetBillsDate(DateTime date1, DateTime date2)
        {
            var list = _billsServices.GetBillsDate(date1, date2);
            return Ok(_mapper.Map<IEnumerable<BillReadDto>>(list));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all-date-state")]
        public ActionResult<IEnumerable<BillReadDto>> GetBillsDateState(int state, DateTime date1, DateTime date2)
        {
            var list = _billsServices.GetBillsDateState(state, date1, date2);
            return Ok(_mapper.Map<IEnumerable<BillReadDto>>(list));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pay")]
        public ActionResult GetBillsPay(DateTime date1, DateTime date2)
        {
            var list = _billsServices.GetBillsPay(date1, date2);
            return Ok(_mapper.Map<IEnumerable<BillReadDto>>(list));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("noPay")]
        public ActionResult GetBillsNoPay(DateTime date1, DateTime date2)
        {
            var list = _billsServices.GetBillsNoPay(date1, date2);
            return Ok(_mapper.Map<IEnumerable<BillReadDto>>(list));
        }


        [Authorize]
        [HttpPatch("state")]
        public ActionResult ChangesState(int billId, int state)
        {
            _billsServices.ChangesState(billId, state);
            _billsServices.SaveChanges();
            return NoContent();
        }

        [Authorize]
        [HttpPatch("isPay")]
        public ActionResult isPay(int billId, Boolean isPay)
        {
            _billsServices.ChangesPay(billId, isPay);
            _billsServices.SaveChanges();
            return NoContent();
        }

    }
}
