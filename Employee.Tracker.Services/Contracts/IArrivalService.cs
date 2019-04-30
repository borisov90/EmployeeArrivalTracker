using EmployeeTracker.Data;
using EmployeeTracker.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Employee.Tracker.Services.Contracts
{
    public interface IArrivalService
    {
        Task<ValidationToken> GetArrivalsFromApi(DateTime date, string baseUrl);
        IEnumerable<Arrival> Parse(HttpRequestBase request);
        bool IsValid(HttpRequestBase request, string validationToken);
    }
}
