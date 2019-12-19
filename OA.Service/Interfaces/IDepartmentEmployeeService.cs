using OA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service.Interfaces
{
    public interface IDepartmentEmployeeService
    {
        IEnumerable<DepartmentEmployee> GetDepartmentEmployees();
        IEnumerable<DepartmentEmployee> GetDepartmentEmployeeByEmployeeNumber(int employeeNumber);
        IEnumerable<DepartmentEmployee> GetDepartmentEmployeeByDepartmentNumber(string departmentNumber);
        IQueryable<DepartmentEmployee> GetDepartmentEmployeeByFilters(int page, int pageSize, string sort, string filter);
        DepartmentEmployee GetOneDepartmentEmployee(int employeeNumber, string departmentNumber);
        void InsertDepartmentEmployee(DepartmentEmployee employee);
        void UpdateDepartmentEmployee(DepartmentEmployee employee);
        int DeleteDepartmentEmployee(int employeeNumber, string departmentNumber);
    }
}
