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
        private readonly IMapper _mapper;
        public BillsController(IBillsServices billsServices, ICartServices cartServices, IMapper mapper)
        {
            _billsServices = billsServices;
            _cartServices = cartServices;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddBill(BillCreateDto bill)
        {
            var _bill = _mapper.Map<Bill>(bill);
            _billsServices.AddBill(_bill);
            _billsServices.SaveChanges();
            var billReadDto = _mapper.Map<BillReadDto>(_bill);

            int billId = billReadDto.Id;
            var cartId = _cartServices.getCartId(billReadDto.UserId);

            var cartItems = _cartServices.GetItems(cartId);
            _billsServices.AddBillDetails(billId, cartItems);
            _billsServices.SaveChanges();

            _cartServices.ClearCartItems(cartId);
            _billsServices.SaveChanges();

            return CreatedAtRoute(nameof(GetBill), new { id = billReadDto.Id }, billReadDto);
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
        [HttpGet]
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
        [HttpPatch("state")]
        public ActionResult ChangesState(int billId, int state)
        {
            _billsServices.ChangesState(billId, state);
            _billsServices.SaveChanges();
            return NoContent();
        }

        [HttpPatch("isPay")]
        public ActionResult isPay(int billId, Boolean isPay)
        {
            _billsServices.ChangesPay(billId, isPay);
            _billsServices.SaveChanges();
            return NoContent();
        }

    }
}
