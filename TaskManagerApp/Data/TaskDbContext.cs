// Data/TaskDbContext.cs
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Models;
using TaskManagerAppNotes.Models;

namespace TaskManagerApp.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserTask> UserTasks { get; set; } // Ensure this matches your model name
        public DbSet<Note> Notes { get; set; }
    }
}
