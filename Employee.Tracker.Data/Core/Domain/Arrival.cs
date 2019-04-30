using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTracker.Data
{
    public class Arrival
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public System.DateTime When { get; set; }
    }
}
