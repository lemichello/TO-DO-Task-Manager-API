using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRepository<User> _repository;

        public LoginController(IRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public HttpResponseMessage Login([Bind("Login,Password")] UserDto user)
        {
            if (!ModelState.IsValid)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var foundUser = _repository
                .GetAll(u => u.Login == user.Login
                             && u.Password == user.Password)
                .FirstOrDefault();

            if (foundUser == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                Path    = "/"
            };

            Response.Cookies.Append("taskManagerUserId", foundUser.Id.ToString(), cookieOptions);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}