using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace OA.Data.Entities
{
    public class BaseEntity
    {
        public int EmployeeNumber { get; set; }
        public string DepartmentNumber { get; set; }
        public string Title { get; set; }
        public DateTime FromDate { get; set; }
    }
}
