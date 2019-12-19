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
    public class SalaryService : ISalaryService
    {
        private readonly IRepository<Salary> _salaryRepository;

        public SalaryService(IRepository<Salary> salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }

        public int DeleteSalary(int employeeNumber, DateTime fromDate)
        {
            Salary salary = GetOneSalary(employeeNumber, fromDate);
            if (salary == null) return -1;

            _salaryRepository.Remove(salary);
            _salaryRepository.SaveChanges();
            return 1;
        }

        public IEnumerable<Salary> GetSalaries()
        {
            return _salaryRepository
                .GetAll()
                .AsQueryable()
                .Include(s => s.Employee)
                .AsEnumerable();
        }

        public IEnumerable<Salary> GetSalariesByEmployeeNumber(int employeeNumber)
        {
            return _salaryRepository.GetAll().Where(prop => prop.EmployeeNumber == employeeNumber);
        }

        public Salary GetOneSalary(int employeeNumber, DateTime fromDate)
        {
            return _salaryRepository.GetOne(employeeNumber, fromDate);
        }

        public void InsertSalary(Salary salary)
        {
            try
            {
                _salaryRepository.Insert(salary);
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

        public void UpdateSalary(Salary salary)
        {
            _salaryRepository.Update(salary);
        }

        public IQueryable<Salary> GetSalariesByFilters(int page, int pageSize, string sort, string filter)
        {
            return _salaryRepository.GetAll()
                .AsQueryable()
                .Include(s => s.Employee)
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize);
        }
    }
}
