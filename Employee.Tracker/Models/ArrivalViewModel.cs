using EmployeeTracker.Data;
using System;
using System.Collections.Generic;

namespace EmployeeTracker.Models
{
    public class ArrivalViewModel
    {
       public IEnumerable<Arrival> Arrivals { get; set; }
    } 
}