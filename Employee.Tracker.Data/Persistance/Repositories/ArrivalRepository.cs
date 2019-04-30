namespace EmployeeTracker.Data.Repositories
{
    public class ArrivalRepository : Repository<Arrival>, IArrivalRepository
    {
        public ArrivalRepository(EmployeeTrackerDbContext context) : base(context)
        {
        }

        public EmployeeTrackerDbContext EmployeeTrackerDbContext
        {
            get { return _context as EmployeeTrackerDbContext; }
        }
    }
}
