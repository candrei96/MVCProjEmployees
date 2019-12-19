using MVCProjEmployees.Exceptions;
using MVCProjEmployees.Utils;
using OA.Data.Entities;
using System;

namespace MVCProjEmployees.Models
{
    public class TitleModel
    {
        public int EmployeeNumber
        {
            get; set;
        }
        public string EmployeeTitle
        {
            get; set;
        }
        public DateTime FromDate 
        { 
            get; set;
        }
        public DateTime ToDate 
        { 
            get; set;
        }
        public Employee Employee
        {
            get; set;
        }

        public override string ToString()
        {
            return $"Employee {EmployeeNumber}, title {EmployeeTitle}, from {FromDate}, to {ToDate}.";
        }
    }
}
