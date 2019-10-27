using System;
using System.Linq;
using System.Security.Cryptography;
using API.Helpers;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DTO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckItemController : ControllerBase
    {
        public CheckItemController(IRepository<ToDoItem> repository, IRepository<ProjectsUsers> projectUsersRepository,
            IDataProtectionProvider provider)
        {
            _repository             = repository;
            _projectUsersRepository = projectUsersRepository;
            _protector              = provider.CreateProtector(nameof(LoginController));
        }

        [HttpPost]
        [Route("{itemId}")]
        public IActionResult CheckItem([FromBody] BaseDto user, int itemId)
        {
            string unprotectedId;

            try
            {
                unprotectedId = _protector.Unprotect(user.Credential);
            }
            catch (CryptographicException)
            {
                return Unauthorized();
            }

            var userId = int.Parse(unprotectedId);
            var projectsUsers = _projectUsersRepository
                .GetAll(p => p.UserId == userId && p.IsAccepted)
                .AsNoTracking();
            var checkingItem = _repository.GetAll(i => i.Id == itemId).First();

            // User is modifying the item, which is not owned by him.
            if (!ToDoItemsHelper.IsItemOwnedByUser(_repository, checkingItem, userId, projectsUsers))
                return NotFound();

            checkingItem.CompleteDate = DateTime.UtcNow;

            _repository.Edit(checkingItem);

            return Ok();
        }

        private readonly IRepository<ToDoItem>      _repository;
        private readonly IRepository<ProjectsUsers> _projectUsersRepository;
        private readonly IDataProtector             _protector;
    }
}