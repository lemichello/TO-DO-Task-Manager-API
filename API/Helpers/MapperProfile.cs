using AutoMapper;
using DAL.Entities;
using DTO;

namespace API.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ToDoItem, ToDoItemDto>();
            CreateMap<ToDoItemDto, ToDoItem>();
            CreateMap<BaseToDoItemDto, ToDoItem>();
            CreateMap<Project, ProjectDto>();
        }
    }
}