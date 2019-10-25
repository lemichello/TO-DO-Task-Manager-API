using System.Linq;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DTO;
using Microsoft.AspNetCore.DataProtection;
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
        public IActionResult Login([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var foundUser = _repository
                .GetAll(u => u.Login == user.Login
                             && u.Password == user.Password)
                .FirstOrDefault();

            if (foundUser == null)
                return NotFound();

            var userId = _protector.Protect(foundUser.Id.ToString());
            // var options = new CookieOptions
            // {
            //     Expires     = DateTimeOffset.Now.AddDays(4),
            //     IsEssential = true
            // };

            // Response.Cookies.Append("taskManagerUserId", userId, options);

            return Ok(userId);
        }

        private readonly IRepository<User> _repository;
        private readonly IDataProtector    _protector;
    }
}