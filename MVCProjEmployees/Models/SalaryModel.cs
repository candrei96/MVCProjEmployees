using MVCProjEmployees.Exceptions;
using MVCProjEmployees.Utils;
using OA.Data.Entities;
using System;

namespace MVCProjEmployees.Models
{
    public class SalaryModel
    {
        public int EmployeeNumber
        {
            get; set;
        }
        public int EmployeeSalary
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
            return $"Employee {EmployeeNumber}, salary {EmployeeSalary}, from {FromDate}, to {ToDate}.";
        }
    }
}
