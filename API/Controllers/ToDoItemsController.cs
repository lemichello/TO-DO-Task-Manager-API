using System;
using System.Collections.Generic;
using System.Linq;
using API.Helpers;
using AutoMapper;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        public ToDoItemsController(IRepository<ToDoItem> repository, Profile mapperProfile)
        {
            _repository = repository;
            _dtoMapper  = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile)));
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            var cookie = Request.Cookies.FirstOrDefault(i => i.Key == "taskManagerUserId");

            if (cookie.Equals(default(KeyValuePair<string, string>)))
                return Unauthorized();

            var userId = int.Parse(cookie.Value);
            var items = _repository
                .GetAll(i => i.UserId == userId)
                .Select(i => _dtoMapper.Map<ToDoItemDto>(i))
                .ToList();

            return Ok(items);
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] BaseToDoItemDto item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cookie = Request.Cookies.FirstOrDefault(i => i.Key == "taskManagerUserId");

            if (cookie.Equals(default(KeyValuePair<string, string>)))
                return Unauthorized();

            var userId  = int.Parse(cookie.Value);
            var newItem = _dtoMapper.Map<ToDoItem>(item);

            newItem.UserId       = userId;
            newItem.CompleteDate = DateTime.MinValue.AddYears(1753);

            _repository.Add(newItem);

            return Ok();
        }

        private readonly IRepository<ToDoItem> _repository;
        private readonly Mapper                _dtoMapper;
    }
}