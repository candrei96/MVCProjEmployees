using OA.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service.Interfaces
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetDepartments();
        IQueryable<Department> GetDepartmentsByFilters(int page, int pageSize, string sort, string filter);
        Department GetOneDepartment(string departmentNumber);
        void InsertDepartment(Department department);
        void UpdateDepartment(Department department);
        int DeleteDepartment(string departmentNumber);
    }
}
