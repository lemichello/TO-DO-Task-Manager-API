using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class EfContext : DbContext
    {
        public EfContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<ItemTags> ItemTags { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectsUsers> ProjectsUsers { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<ToDoItem> ToDoItems { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoItem>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<ProjectsUsers>()
                .HasKey(pu => new {pu.ProjectId, pu.UserId});
            modelBuilder.Entity<ProjectsUsers>()
                .HasOne(pu => pu.Inviter);
            modelBuilder.Entity<ProjectsUsers>()
                .HasOne(pu => pu.User);
            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Project>()
                .HasMany(p => p.ToDoItems)
                .WithOne(i => i.Project);
            modelBuilder.Entity<ItemTags>()
                .HasKey(it => new {it.ToDoItemId, it.TagId});
            modelBuilder.Entity<MigrationHistory>()
                .HasKey(mh => mh.MigrationId);

            base.OnModelCreating(modelBuilder);
        }
    }
}