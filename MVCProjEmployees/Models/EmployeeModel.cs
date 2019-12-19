using MVCProjEmployees.Exceptions;
using MVCProjEmployees.Utils;
using Newtonsoft.Json;
using System;

namespace MVCProjEmployees.Models
{
    public class EmployeeModel
    {
        public int EmployeeNumber
        {
            get; set;
        }
        public DateTime BirthDate
        {
            get; set;
        }
        public string FirstName
        {
            get; set;

        }
        public string LastName
        {
            get; set;

        }
        public string EmployeeGender
        {
            get; set;

        }
        public DateTime HireDate
        {
            get; set;

        }

        public override string ToString()
        {
            return $"Employee {EmployeeNumber}, {LastName} {FirstName}, {EmployeeGender}, {BirthDate}";
        }
    }
}
