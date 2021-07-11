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
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
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
        [HttpGet("{id}", Name = "GetUser")]
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

        [HttpPost("createAdmin")]
        public ActionResult AddAdmin()
        {
            var user = new UserCreateDto();
            user.Name = "Admin";
            user.Email = "admin";
            user.Password = "admin";
            user.Role = "Admin";
            user.Address = "";
            user.Phone = "";

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


        [HttpGet("forgotPassword/{email}")]
        public ActionResult getOTP(String email)
        {

            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };


            for (int i = 0; i < 6; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }


            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(sOTP));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }

            var user = _usersServices.GetUserByEmail(email);

            user.Password = hash.ToString();


            _usersServices.UpdateUser(user);
            _usersServices.SaveChanges();



            string subject = "ShopBanChau";
            string body = "Your password: "+ sOTP;
            string FromMail = "congdanh04092000@gmail.com";
            string emailTo = email;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(FromMail);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = false;
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential("congdanh04092000@gmail.com", "danh123###");

            SmtpServer.Send(mail);

            return NoContent();
        }


    }


}
