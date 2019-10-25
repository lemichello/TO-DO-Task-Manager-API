using System.Linq;
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
    public class ProjectsController : ControllerBase
    {
        public ProjectsController(IRepository<ProjectsUsers> projectsUsersRepository, Profile mapperProfile,
            IDataProtectionProvider provider)
        {
            _projectsUsersRepository = projectsUsersRepository;
            _dtoMapper               = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile)));
            _protector               = provider.CreateProtector(nameof(LoginController));
        }

        [HttpGet]
        [Route("{protectedId}")]
        public IActionResult GetSharedProjects(string protectedId)
        {
            string userIdStr;

            try
            {
                userIdStr = _protector.Unprotect(protectedId);
            }
            catch (CryptographicException)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdStr);
            var projects = _projectsUsersRepository
                .GetAll(i => i.UserId == userId && i.IsAccepted)
                .AsNoTracking()
                .Select(i => _dtoMapper.Map<ProjectDto>(i.Project))
                .ToList();

            return Ok(projects);
        }

        private readonly Mapper                     _dtoMapper;
        private readonly IRepository<ProjectsUsers> _projectsUsersRepository;
        private readonly IDataProtector             _protector;
    }
}