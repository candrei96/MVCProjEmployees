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
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Title> _titleRepository;
        private readonly IRepository<Salary> _salaryRepository;
        private readonly IRepository<DepartmentEmployee> _departmentEmployeeRepository;
        private readonly IRepository<DepartmentManager> _departmentManagerRepository;

        public EmployeeService
            (IRepository<Employee> employeeRepository,
            IRepository<Title> titleRepository,
            IRepository<Salary> salaryRepository,
            IRepository<DepartmentEmployee> departmentEmployeeRepository,
            IRepository<DepartmentManager> departmentManagerRepository
            )
        {
            _employeeRepository = employeeRepository;
            _titleRepository = titleRepository;
            _salaryRepository = salaryRepository;
            _departmentEmployeeRepository = departmentEmployeeRepository;
            _departmentManagerRepository = departmentManagerRepository;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employeeRepository.GetAll();
        }

        public IQueryable<Employee> GetEmployeesQuery()
        {
            return _employeeRepository.GetQuery();
        }

        public Employee GetOneEmployee(int employeeNumber)
        {
            return _employeeRepository.GetOne(employeeNumber);
        }

        public void InsertEmployee(Employee employee)
        {
            try
            {
                _employeeRepository.Insert(employee);
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

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.Update(employee);
        }

        public int DeleteEmployee(int employeeNumber)
        {
            Employee employee = GetOneEmployee(employeeNumber);
            if (employee == null) return -1;

            IEnumerable<Title> titles = _titleRepository.GetAll(employeeNumber);
            if (titles != null && titles.Count() > 0)
            {
                titles.ToList().ForEach(t =>
                {
                    _titleRepository.Delete(t);
                });
            }

            IEnumerable<Salary> salaries = _salaryRepository.GetAll(employeeNumber);
            if (salaries != null && salaries.Count() > 0)
            {
                salaries.ToList().ForEach(s =>
                {
                    _salaryRepository.Delete(s);
                });
            }

            IEnumerable<DepartmentEmployee> departmentEmployees = _departmentEmployeeRepository.GetAll(employeeNumber);
            if (departmentEmployees != null && departmentEmployees.Count() > 0)
            {
                departmentEmployees.ToList().ForEach(de =>
                {
                    _departmentEmployeeRepository.Delete(de);
                });
            }

            IEnumerable<DepartmentManager> departmentManagers = _departmentManagerRepository.GetAll(employeeNumber);
            if (departmentManagers != null && departmentManagers.Count() > 0)
            {
                departmentManagers.ToList().ForEach(dm =>
                {
                    _departmentManagerRepository.Delete(dm);
                });
            }

            _employeeRepository.Remove(employee);
            _employeeRepository.SaveChanges();
            return 1;
        }

        public IQueryable<Employee> GetEmployeesByFilters(int page, int pageSize, string sort, string filter)
        {
            return _employeeRepository.GetAll()
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize);
        }
    }
}
