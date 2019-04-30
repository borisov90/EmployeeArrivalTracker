using System.Data.Entity;

namespace EmployeeTracker.Data
{
    public class EmployeeTrackerDbContext : DbContext
    {
        public EmployeeTrackerDbContext() : base(nameOrConnectionString: "EmployeeTracker") { }

        public DbSet<Arrival> Arrivals { get; set; }
    }
}
