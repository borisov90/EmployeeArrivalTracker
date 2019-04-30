using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace JsonEmployeeGenerator
{
    class Program
    {
        private static int employeeMinimumAge = 18;
        private static int employeeMaximumAge = 66;
        private static int firstTenEmployees = 10;
        private static int numberOfRandomManagerIds = 11;
        private static string managerPosition = "Manager";
        private static Array roles = Enum.GetValues(typeof(Enumerations.RoleKeyValues));
        private static Array teams = Enum.GetValues(typeof(Enumerations.TeamKeyValues));
        private static Random generator = new Random();

        static void Main(string[] args)
        {
            var employeesFromFile = File.ReadAllLines("employees.txt").ToArray();
            List<JsonEmployee> employees = GetJsonEmployees(employeesFromFile);

            CreateEmployeeJsonFile(employees);
        }

        private static List<JsonEmployee> GetJsonEmployees(string[] employeesFromFile)
        {
            
            var employees = new List<JsonEmployee>();

            for (int index = 0; index < employeesFromFile.Length; index++)
            {
                JsonEmployee employee = FillEmployeeData(generator, employeesFromFile, index);
                SetEmployeeRole(roles, generator, index, employee);
                SetEmployeeTeams(teams, generator, index, employee);

                employees.Add(employee);
            }

            return employees;
        }

        private static void CreateEmployeeJsonFile(List<JsonEmployee> employees)
        {
            using (var jsonFile = File.CreateText("employees.json"))
            {
                jsonFile.WriteLine("[");
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var employee in employees)
                {
                    stringBuilder.Clear();

                    stringBuilder.Append(string.Format("{{\"Id\":{0}", employee.Id));
                    stringBuilder.Append(",");
                    stringBuilder.Append(string.Format("\"ManagerId\":{0}", employee.ManagerId.HasValue ? employee.ManagerId.ToString() : "null"));
                    stringBuilder.Append(",");
                    stringBuilder.Append(string.Format("\"Age\":{0}", employee.Age));
                    stringBuilder.Append(",");
                    stringBuilder.Append(string.Format("\"Teams\":{0}", string.Join(",", employee.Teams.Select(x => "\"" + x + "\""))));
                    stringBuilder.Append(",");
                    stringBuilder.Append(string.Format("\"Role\":{0}", employee.Role));
                    stringBuilder.Append(",");
                    stringBuilder.Append(string.Format("\"Email\":{0}", employee.Email));
                    stringBuilder.Append(",");
                    stringBuilder.Append(string.Format("\"SurName\":{0}", employee.Surname));
                    stringBuilder.Append(",");
                    stringBuilder.Append(string.Format("\"Name\":{0}}}", employee.Name));
                    stringBuilder.Append(",");

                    var formattedEmployeed = stringBuilder.ToString();

                    jsonFile.WriteLine(formattedEmployeed);
                }
                jsonFile.WriteLine("]");
                jsonFile.Flush();
            }

            ////Another way to do this

            //var json = JsonConvert.SerializeObject(employees, Formatting.Indented);

            //using (StreamWriter file = File.CreateText("employees.json"))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    //serialize object directly into file stream
            //    serializer.Serialize(file, json);
            //}
        }

        private static void SetEmployeeTeams(Array teams, Random generator, int index, JsonEmployee employee)
        {
            if (index <= firstTenEmployees)
            {
                employee.Teams = new HashSet<string>();
            }
            else
            {
                int teamMembersCount = GetNumberOfTeamMembers(generator);
                SetEmployeeNumberOfTeams(teams, generator, employee, teamMembersCount);
            }
        }

        private static void SetEmployeeRole(Array roles, Random generator, int index, JsonEmployee employee)
        {
            if (index <= firstTenEmployees)
            {
                employee.Role = managerPosition;
            }
            else
            {
                employee.ManagerId = generator.Next(numberOfRandomManagerIds);
                SetRandomRole(roles, generator, employee);
            }
        }

        private static void SetRandomRole(Array roles, Random generator, JsonEmployee employee)
        {
            var currentRole = (Enumerations.RoleKeyValues)roles.GetValue(generator.Next(roles.Length));
            employee.Role = Enum.GetName(typeof(Enumerations.RoleKeyValues), currentRole);
        }

        private static void SetEmployeeNumberOfTeams(Array teams, Random generator, JsonEmployee employee, int teamMembersCount)
        {
            var employeeTeams = new HashSet<string>();

            for (int currentTeamPosition = 0; currentTeamPosition < teamMembersCount; ++currentTeamPosition)
            {
                AddRandomTeam(teams, generator, employeeTeams);
            }
            employee.Teams = employeeTeams;
        }

        private static void AddRandomTeam(Array teams, Random generator, HashSet<string> employeeTeams)
        {
            var randomTeam = (Enumerations.TeamKeyValues)teams.GetValue(generator.Next(teams.Length));
            var teamName = Enum.GetName(typeof(Enumerations.TeamKeyValues), randomTeam);
            employeeTeams.Add(teamName);
        }

        private static int GetNumberOfTeamMembers(Random generator)
        {
            return generator.Next(1, 4);
        }

        private static JsonEmployee FillEmployeeData(Random generator, string[] employeesFromFile, int i)
        {
            JsonEmployee employee = new JsonEmployee();
            employee.Id = i;
            employee.Name = employeesFromFile[i].Split('\t')[0];
            employee.Surname = employeesFromFile[i].Split('\t')[1];
            employee.Email = employeesFromFile[i].Split('\t')[2];
            employee.Age = generator.Next(employeeMinimumAge, employeeMaximumAge);
            return employee;
        }
    }

    
}
