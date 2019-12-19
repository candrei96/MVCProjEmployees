using OA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetEmployees();
        IQueryable<Employee> GetEmployeesByFilters(int page, int pageSize, string sort, string filter);
        Employee GetOneEmployee(int employeeNumber);
        void InsertEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        int DeleteEmployee(int employeeNumber);
    }
}
