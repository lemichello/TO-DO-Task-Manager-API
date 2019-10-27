using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using AutoMapper;
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
    public class ToDoItemsController : ControllerBase
    {
        public ToDoItemsController(IRepository<ToDoItem> repository, IRepository<ProjectsUsers> projectUsersRepository,
            Profile mapperProfile, IDataProtectionProvider provider)
        {
            _repository             = repository;
            _projectUsersRepository = projectUsersRepository;
            _dtoMapper              = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile)));
            _protector              = provider.CreateProtector(nameof(LoginController));
        }

        [HttpGet]
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
                .AsNoTracking()
                .Select(i => _dtoMapper.Map<ToDoItemDto>(i))
                .ToList();

            return Ok(items);
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] BaseToDoItemDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string unprotectedId;

            try
            {
                unprotectedId = _protector.Unprotect(item.Credential);
            }
            catch (CryptographicException)
            {
                return Unauthorized();
            }

            var userId  = int.Parse(unprotectedId);
            var newItem = _dtoMapper.Map<ToDoItem>(item);

            newItem.UserId       = userId;
            newItem.CompleteDate = DateTime.MinValue.AddYears(1753);

            _repository.Add(newItem);

            return Ok();
        }

        [HttpPut]
        public HttpResponseMessage UpdateItem([FromBody] ToDoItemDto item)
        {
            int userId;

            if (!ModelState.IsValid)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            try
            {
                var unprotectedId = _protector.Unprotect(item.Credential);
                userId = int.Parse(unprotectedId);
            }
            catch (CryptographicException)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            try
            {
                var editedItem = _dtoMapper.Map<ToDoItem>(item);

                var projectsUsers = _projectUsersRepository.GetAll(p => p.UserId == userId && p.IsAccepted)
                    .AsNoTracking();

                // User is modifying the item, which is not owned by him.
                if (!_repository.GetAll().AsNoTracking().Any(i =>
                    i.Id == editedItem.Id && i.UserId == userId && (i.ProjectId == null || projectsUsers.Any(p =>
                                                                        i.ProjectId == p.ProjectId))))
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                editedItem.UserId = userId;

                _repository.Edit(editedItem);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NotModified);
            }
        }

        [HttpPost("{id}")]
        public HttpResponseMessage DeleteItem([FromBody] BaseDto user, int id)
        {
            int userId;

            if (!ModelState.IsValid)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            try
            {
                var unprotectedId = _protector.Unprotect(user.Credential);
                userId = int.Parse(unprotectedId);
            }
            catch (CryptographicException)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var deletingItem = _repository
                .GetAll().FirstOrDefault(i => i.Id == id && i.UserId == userId);

            if (deletingItem == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            try
            {
                _repository.Remove(deletingItem);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.NotModified);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private readonly Mapper                     _dtoMapper;
        private readonly IRepository<ToDoItem>      _repository;
        private readonly IRepository<ProjectsUsers> _projectUsersRepository;
        private readonly IDataProtector             _protector;
    }
}