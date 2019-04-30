using System.Collections.Generic;


namespace JsonEmployeeGenerator
{
    public class JsonEmployee
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
        public int? ManagerId { get; set; }
        public HashSet<string> Teams { get; set; }
        public int Id { get; set; }
    }
}
