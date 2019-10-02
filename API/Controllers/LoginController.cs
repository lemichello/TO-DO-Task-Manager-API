using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DTO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public LoginController(IRepository<User> repository, IDataProtectionProvider provider)
        {
            _repository = repository;
            _protector  = provider.CreateProtector(nameof(LoginController));
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
                Path    = "/",
                Secure  = true
            };

            Response.Cookies.Append("taskManagerUserId",
                _protector.Protect(foundUser.Id.ToString()),
                cookieOptions);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private readonly IRepository<User> _repository;
        private readonly IDataProtector    _protector;
    }
}