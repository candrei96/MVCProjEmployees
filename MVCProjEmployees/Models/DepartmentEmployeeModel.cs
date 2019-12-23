using MVCProjEmployees.Utils;
using OA.Data.Entities;
using OA.Web.Exceptions;
using System;

namespace MVCProjEmployees.Models
{
    public class DepartmentEmployeeModel
    {
        public int EmployeeNumber
        {
            get; set;
        }
        public string DepartmentNumber
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
            get;
            set;
        }

        public Department Department
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"Employee {EmployeeNumber}, Department {DepartmentNumber}, from {FromDate}, to {ToDate}.";
        }
    }
}
