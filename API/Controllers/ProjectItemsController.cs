using System;
using System.Linq;
using API.Helpers;
using AutoMapper;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using DLL.Services;
using DTO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectItemsController : ControllerBase
    {
        public ProjectItemsController(IRepository<ToDoItem> itemsRepository, Profile mapperProfile,
            IDataProtectionProvider provider)
        {
            _itemsRepository = itemsRepository;
            _dtoMapper       = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile)));
            _protector       = provider.CreateProtector(nameof(LoginController));
        }

        [HttpPost]
        public IActionResult GetItemsForProject([FromBody] ProjectDto project)
        {
            string cookieValue;

            try
            {
                cookieValue = CookieHelper.GetCookieValue(Request, _protector);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }

            var userId  = int.Parse(cookieValue);
            var items   = _itemsRepository.GetAll().Include(i => i.Project).AsNoTracking();
            var minDate = DateTime.MinValue.AddYears(1753);

            if (project.Id == null)
            {
                items = items.Where(i => i.UserId == userId);
                var projectItems = ProjectsService.GetItemsFromDefaultProject(project, items, _dtoMapper).ToList();

                return Ok(projectItems);
            }
            else
            {
                var projectItems = items
                    .Where(i => i.ProjectId == project.Id &&
                                i.Project.Name == project.Name &&
                                i.CompleteDate == minDate)
                    .Select(i => _dtoMapper.Map<ToDoItemDto>(i))
                    .ToList();

                return Ok(projectItems);
            }
        }

        private readonly Mapper                _dtoMapper;
        private readonly IRepository<ToDoItem> _itemsRepository;
        private readonly IDataProtector        _protector;
    }
}