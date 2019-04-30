using System;

namespace EmployeeTracker.Services
{
    public class ValidationToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}