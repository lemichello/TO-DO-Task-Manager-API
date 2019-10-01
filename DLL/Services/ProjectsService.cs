using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.Entities;
using DTO;

namespace DLL.Services
{
    public static class ProjectsService
    {
        public static List<ToDoItemDto> GetItemsFromDefaultProject(ProjectDto project, IQueryable<ToDoItem> items,
            Mapper mapper)
        {
            var minDate = DateTime.MinValue.AddYears(1753);

            switch (project.Name)
            {
                case "Inbox":
                    items = items.Where(i =>
                        i.Date == minDate &&
                        i.CompleteDate == minDate &&
                        i.ProjectId == null);
                    break;

                case "Today":
                    items = items.Where(i => (i.Date <= DateTime.Today && i.Date != minDate ||
                                              i.Deadline <= DateTime.Today && i.Deadline != minDate) &&
                                             i.CompleteDate == minDate);
                    break;

                case "Upcoming":
                    items = items.Where(i => i.CompleteDate == minDate);
                    break;

                case "Logbook":
                    items = items.Where(i => i.CompleteDate != minDate).OrderByDescending(i => i.CompleteDate);
                    break;

                default:
                    return new List<ToDoItemDto>();
            }

            return items.Select(i => mapper.Map<ToDoItemDto>(i)).ToList();
        }
    }
}