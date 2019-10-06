using System;
using AutoMapper;
using DAL.Entities;
using DTO;

namespace API.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<string, DateTime>().ConvertUsing<StringToDateTimeConverter>();
            CreateMap<ToDoItem, ToDoItemDto>()
                .ForMember(dest => dest.ProjectName,
                    opt => opt.MapFrom(src => src.ProjectId == null ? "" : src.Project.Name));
            CreateMap<ToDoItemDto, ToDoItem>();
            CreateMap<BaseToDoItemDto, ToDoItem>();
            CreateMap<Project, ProjectDto>();
        }
    }
}