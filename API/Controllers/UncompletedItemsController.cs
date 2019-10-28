using System;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DTO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UncompletedItemsController : ControllerBase
    {
        public UncompletedItemsController(IRepository<ToDoItem> repository, Profile mapperProfile,
            IDataProtectionProvider provider)
        {
            _repository = repository;
            _dtoMapper  = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile)));
            _protector  = provider.CreateProtector(nameof(LoginController));
        }

        [HttpPost]
        public IActionResult GetUncompletedItems([FromBody] BaseDto user)
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

            var minDate = DateTime.MinValue.AddYears(1753);
            var userId  = int.Parse(unprotectedId);
            var items = _repository
                .GetAll(i => i.UserId == userId && i.CompleteDate == minDate)
                .Include(i => i.Project)
                .AsNoTracking()
                .Select(i => _dtoMapper.Map<ToDoItemDto>(i))
                .ToList();
            var json = JsonConvert.SerializeObject(items);

            return Content(json, "application/json");
        }

        private readonly Mapper                _dtoMapper;
        private readonly IRepository<ToDoItem> _repository;
        private readonly IDataProtector        _protector;
    }
}