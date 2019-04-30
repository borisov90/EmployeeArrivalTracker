namespace EmployeeTracker.Data.Common
{
    using EmployeeTracker.Data.Repositories;
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        IArrivalRepository Arrivals { get; }
        int SaveChanges();
        Task SaveChangesAsync();
    }
}
