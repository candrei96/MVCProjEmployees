using OA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service.Interfaces
{
    public interface ISalaryService
    {
        IEnumerable<Salary> GetSalaries();
        IEnumerable<Salary> GetSalariesByEmployeeNumber(int employeeNumber);
        IQueryable<Salary> GetSalariesByFilters(int page, int pageSize, string sort, string filter);
        Salary GetOneSalary(int employeeNumber, DateTime fromDate);
        void InsertSalary(Salary salary);
        void UpdateSalary(Salary salary);
        int DeleteSalary(int employeeNumber, DateTime fromDate);
    }
}
