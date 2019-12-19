using Microsoft.EntityFrameworkCore;
using OA.Data.Entities;
using OA.Repo;
using OA.Repo.MySql.Extensions;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<DepartmentEmployee> _departmentEmployeeRepository;
        private readonly IRepository<DepartmentManager> _departmentManagerRepository;

        public DepartmentService
            (
            IRepository<Department> _repository,
            IRepository<DepartmentEmployee> departmentEmployeeRepository,
            IRepository<DepartmentManager> departmentManagerRepository)
        {
            _departmentRepository = _repository;
            _departmentEmployeeRepository = departmentEmployeeRepository;
            _departmentManagerRepository = departmentManagerRepository;
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _departmentRepository.GetAll();
        }

        public Department GetOneDepartment(string departmentNumber)
        {
            return _departmentRepository.GetOne(departmentNumber);
        }

        public void InsertDepartment(Department department)
        {
            try
            {
                _departmentRepository.Insert(department);
            }
            catch (DbUpdateException dbException)
            {
                if (dbException.InnerException != null)
                {
                    GenericServiceException.ConstructGenericException(dbException);
                }
                else
                {
                    throw dbException;
                }
            }
        }

        public void UpdateDepartment(Department department)
        {
            _departmentRepository.Update(department);
        }
        public int DeleteDepartment(string departmentNumber)
        {
            Department department = GetOneDepartment(departmentNumber);
            if (department == null) return -1;

            IEnumerable<DepartmentEmployee> departmentEmployees = _departmentEmployeeRepository.GetAll(departmentNumber);
            if (departmentEmployees != null && departmentEmployees.Count() > 0)
            {
                departmentEmployees.ToList().ForEach(de =>
                {
                    _departmentEmployeeRepository.Delete(de);
                });
            }

            IEnumerable<DepartmentManager> departmentManagers = _departmentManagerRepository.GetAll(departmentNumber);
            if (departmentManagers != null && departmentManagers.Count() > 0)
            {
                departmentManagers.ToList().ForEach(dm =>
                {
                    _departmentManagerRepository.Delete(dm);
                });
            }

            _departmentRepository.Remove(department);
            _departmentRepository.SaveChanges();
            return 1;
        }

        public IQueryable<Department> GetDepartmentsByFilters(int page, int pageSize, string sort, string filter)
        {
            return _departmentRepository.GetAll()
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize);
        }
    }
}
