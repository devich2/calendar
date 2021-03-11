using Calendar.Entities.Tables;
using Calendar.Seed;
using Microsoft.EntityFrameworkCore;

namespace Calendar.DAL
{
    public class CalendarDbContext : DbContext
    {

        public CalendarDbContext(DbContextOptions<CalendarDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Key
            
            builder.Entity<ToDoTask>().HasKey(x => x.TaskId);
            #endregion

            #region ValueGenerations
            
            builder.Entity<ToDoTask>()
                .Property(p => p.TaskId)
                .ValueGeneratedOnAdd();
            #endregion

            #region RelationShips
            
            #endregion
            DatabaseInitializer.SeedDatabase(builder);
        }
        
        public DbSet<ToDoTask> ToDoTasks { get; set; }
    }
}
