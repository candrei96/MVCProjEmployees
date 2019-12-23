using OA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service.Interfaces
{
    public interface IDepartmentManagerService
    {
        IQueryable<DepartmentManager> GetDepartmentManagerQuery();
        IEnumerable<DepartmentManager> GetDepartmentManagers();
        IEnumerable<DepartmentManager> GetDepartmentManagerByEmployeeNumber(int employeeNumber);
        IEnumerable<DepartmentManager> GetDepartmentManagerByDepartmentNumber(string departmentNumber);
        IQueryable<DepartmentManager> GetDepartmentManagerByFilters(int page, int pageSize, string sort, string filter);
        DepartmentManager GetOneDepartmentManager(int employeeNumber, string departmentNumber);
        void InsertDepartmentManager(DepartmentManager manager);
        void UpdateDepartmentManager(DepartmentManager manager);
        int DeleteDepartmentManager(int employeeNumber, string departmentNumber);
    }
}
