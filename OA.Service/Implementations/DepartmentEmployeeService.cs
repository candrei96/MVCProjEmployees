using Microsoft.EntityFrameworkCore;
using MVCProjEmployees.Utils;
using MySql.Data.MySqlClient;
using OA.Data.Entities;
using OA.Repo;
using OA.Repo.MySql.Extensions;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OA.Service.Implementations
{
    public class DepartmentEmployeeService : IDepartmentEmployeeService
    {
        private readonly IRepository<DepartmentEmployee> _departmentEmployeeRepository;

        public DepartmentEmployeeService(IRepository<DepartmentEmployee> departmentEmployeeRepository)
        {
            _departmentEmployeeRepository = departmentEmployeeRepository;
        }

        public int DeleteDepartmentEmployee(int employeeNumber, string departmentNumber)
        {
            DepartmentEmployee deptEmployee = GetOneDepartmentEmployee(employeeNumber, departmentNumber);
            if (deptEmployee == null) return -1;

            _departmentEmployeeRepository.Remove(deptEmployee);
            _departmentEmployeeRepository.SaveChanges();
            return 1;
        }

        public IEnumerable<DepartmentEmployee> GetDepartmentEmployeeByDepartmentNumber(string departmentNumber)
        {
            return _departmentEmployeeRepository
                .GetAll()
                .AsQueryable()
                .Include(e => e.Employee)
                .Include(e => e.Department)
                .Where(prop => prop.DepartmentNumber == departmentNumber)
                .AsEnumerable();
        }

        public IEnumerable<DepartmentEmployee> GetDepartmentEmployeeByEmployeeNumber(int employeeNumber)
        {
            return _departmentEmployeeRepository
                .GetAll()
                .AsQueryable()
                .Include(e => e.Employee)
                .Include(e => e.Department)
                .Where(prop => prop.EmployeeNumber == employeeNumber)
                .AsEnumerable();
        }

        public IQueryable<DepartmentEmployee> GetDepartmentEmployeeByFilters(int page, int pageSize, string sort, string filter)
        {
            return _departmentEmployeeRepository.GetAll()
                .AsQueryable()
                .Include(e => e.Employee)
                .Include(e => e.Department)
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<DepartmentEmployee> GetDepartmentEmployees()
        {
            return _departmentEmployeeRepository.GetAll();
        }

        public DepartmentEmployee GetOneDepartmentEmployee(int employeeNumber, string departmentNumber)
        {
            return _departmentEmployeeRepository.GetOne(employeeNumber, departmentNumber);
        }

        public void InsertDepartmentEmployee(DepartmentEmployee employee)
        {
            try
            {
                _departmentEmployeeRepository.Insert(employee);
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

        public void UpdateDepartmentEmployee(DepartmentEmployee employee)
        {
            _departmentEmployeeRepository.Update(employee);
        }
    }
}
