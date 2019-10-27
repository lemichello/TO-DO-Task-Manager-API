using System.Linq;
using DAL.Entities;
using DAL.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public static class ToDoItemsHelper
    {
        public static bool IsItemOwnedByUser(IRepository<ToDoItem> repository, ToDoItem item, int userId,
            IQueryable<ProjectsUsers> projectsUsers)
        {
            return repository.GetAll().AsNoTracking().Any(i =>
                i.Id == item.Id && (i.UserId == userId || i.ProjectId != null && projectsUsers.Any(p =>
                                        i.ProjectId == p.ProjectId)));
        }
    }
}