using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTracker.Data.Repositories
{
    public interface IArrivalRepository : IRepository<Arrival>
    {
        //In case we want to extend in the future with some Arrival specific data
    }
}
