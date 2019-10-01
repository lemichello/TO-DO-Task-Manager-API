using System;
using System.Linq;
using API.Helpers;
using AutoMapper;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DLL.Services;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        public ProjectsController(IRepository<ToDoItem> repository, Profile mapperProfile)
        {
            _itemsRepository = repository;
            _dtoMapper       = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile)));
        }

        [HttpGet]
        public IActionResult GetItems([FromBody] ProjectDto project)
        {
            string cookieValue;

            try
            {
                cookieValue = CookieHelper.GetCookieValue(Request);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }

            var userId  = int.Parse(cookieValue);
            var items   = _itemsRepository.GetAll();
            var minDate = DateTime.MinValue.AddYears(1753);

            if (project.Id == null)
            {
                items = items.Where(i => i.UserId == userId);
                var projectItems = ProjectsService.GetItemsFromDefaultProject(project, items, _dtoMapper);

                return Ok(projectItems);
            }
            else
            {
                var projectItems = items.Where(i => i.ProjectId == project.Id &&
                                                    i.Project.Name == project.Name &&
                                                    i.CompleteDate == minDate).ToList();

                return Ok(projectItems);
            }
        }

        private readonly Mapper                _dtoMapper;
        private readonly IRepository<ToDoItem> _itemsRepository;
    }
}