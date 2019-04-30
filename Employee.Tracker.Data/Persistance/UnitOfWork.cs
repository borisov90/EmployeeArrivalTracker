using System;
using System.Threading.Tasks;
using EmployeeTracker.Data.Repositories;

namespace EmployeeTracker.Data.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeTrackerDbContext _context;

        public UnitOfWork(EmployeeTrackerDbContext context)
        {
            _context = context;
            Arrivals = new ArrivalRepository(_context);
        }

        public IArrivalRepository Arrivals { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
