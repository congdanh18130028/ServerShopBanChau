using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shop.DataAccess;
using Shop.Dtos;
using Shop.Models;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _usersServices;
        private readonly ICartServices _cartServices;
        private readonly IMapper _mapper;

        public UsersController(IUsersServices userServices, ICartServices cartServices, IMapper mapper)
        {
            _usersServices = userServices;
            _cartServices = cartServices;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetUsers()
        {
            var users = _usersServices.GetUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
        }

        [Authorize]
        [HttpGet("{id}", Name ="GetUser")]
        public ActionResult GetUser(int id)
        {
            var user = _usersServices.GetUser(id);
            if (user != null)
            {
                return Ok(_mapper.Map<UserReadDto>(user));
            }
            return NotFound($"Customer with id: {id} was not found");
        }

        [HttpGet("email/{email}")]
        public ActionResult GetUserByEmail(String email)
        {
            var user = _usersServices.GetUserByEmail(email);
            if (user != null)
            {
                return Ok(_mapper.Map<UserReadDto>(user));
            }
            return NotFound($"Customer with id: {email} was not found");
        }

        [HttpPost]
        public ActionResult AddUser(UserCreateDto user)
        {
            var _user = _mapper.Map<User>(user);
           _usersServices.AddUser(_user);
           _usersServices.SaveChanges();
            var userReadDto = _mapper.Map<UserReadDto>(_user);
            _cartServices.CreateCart(userReadDto.Id);
            _cartServices.SaveChanges();
            return CreatedAtRoute(nameof(GetUser), new { id = userReadDto.Id }, userReadDto);
        }

        [Authorize]
        [HttpPatch("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] JsonPatchDocument<UserUpdateDto> patch)
        {
            var _user = _usersServices.GetUser(id);

            if (_user == null)
            {
                return NotFound();
            }
            var user = _mapper.Map<UserUpdateDto>(_user);
            patch.ApplyTo(user, ModelState);
            if (!TryValidateModel(user))
            {
                return ValidationProblem();
            }
            _mapper.Map(user, _user);
            _usersServices.UpdateUser(_user);
            _usersServices.SaveChanges();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var _user = _usersServices.GetUser(id);
            if (_user == null)
            {
            return NotFound($"Customer with id: {id} was not found");
            }
            _cartServices.DropCart(_user.Id);
            _cartServices.SaveChanges();
            _usersServices.DeleteUser(_user);
            _usersServices.SaveChanges();
            return NoContent();
        }



    }


}
