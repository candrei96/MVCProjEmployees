using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OA.Data.Entities
{
    public class Salary : BaseEntity
    {
        public int EmployeeSalary
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
    }
}
