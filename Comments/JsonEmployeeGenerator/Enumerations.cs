using System.ComponentModel.DataAnnotations;

namespace JsonEmployeeGenerator
{
    public class Enumerations
    {
        public enum KeyTypes
        {
            Roles,
            Teams,
        }

        public enum RoleKeyValues
        {
            [Display(Name = "Junior Developer")]
            JuniorDeveloper,

            [Display(Name = "Semi Senior Developer")]
            SemiSeniorDeveloper,

            [Display(Name = "Senior Developer")]
            SeniorDeveloper,

            [Display(Name = "Principal")]
            Principal,

            [Display(Name = "Team Leader")]
            TeamLeader
        }

        public enum TeamKeyValues
        {
            [Display(Name = "Platform")]
            Platform,

            [Display(Name = "Sales")]
            Sales,

            [Display(Name = "Billing")]
            Billing,

            [Display(Name = "Mirage")]
            Mirage
        }
    }
}
