using System;

namespace API.Models
{
    public class Employee
    {
        public long Id {get; set;}
        public string FirstName {get;set;}
        public string MiddleName {get;set;}
        public string LastName {get;set;}
        public DateTimeOffset HireDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}