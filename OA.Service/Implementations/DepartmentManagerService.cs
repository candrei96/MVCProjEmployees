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
    public class DepartmentManagerService : IDepartmentManagerService
    {
        private readonly IRepository<DepartmentManager> _deptManagerRepository;

        public DepartmentManagerService(IRepository<DepartmentManager> deptManagerRepository)
        {
            _deptManagerRepository = deptManagerRepository;
        }

        public int DeleteDepartmentManager(int employeeNumber, string departmentNumber)
        {
            DepartmentManager deptManager = GetOneDepartmentManager(employeeNumber, departmentNumber);
            if (deptManager == null) return -1;

            _deptManagerRepository.Remove(deptManager);
            _deptManagerRepository.SaveChanges();
            return 1;
        }

        public IEnumerable<DepartmentManager> GetDepartmentManagerByDepartmentNumber(string departmentNumber)
        {
            return _deptManagerRepository
                .GetAll()
                .AsQueryable()
                .Include(e => e.Employee)
                .Include(e => e.Department)
                .Where(prop => prop.DepartmentNumber == departmentNumber)
                .AsEnumerable();
        }

        public IEnumerable<DepartmentManager> GetDepartmentManagerByEmployeeNumber(int employeeNumber)
        {
            return _deptManagerRepository
                .GetAll()
                .AsQueryable()
                .Include(e => e.Employee)
                .Include(e => e.Department)
                .Where(prop => prop.EmployeeNumber == employeeNumber)
                .AsEnumerable(); ;
        }

        public IQueryable<DepartmentManager> GetDepartmentManagerByFilters(int page, int pageSize, string sort, string filter)
        {
            return _deptManagerRepository.GetAll()
                .AsQueryable()
                .Include(e => e.Employee)
                .Include(e => e.Department)
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<DepartmentManager> GetDepartmentManagers()
        {
            return _deptManagerRepository.GetAll();
        }

        public DepartmentManager GetOneDepartmentManager(int employeeNumber, string departmentNumber)
        {
            return _deptManagerRepository.GetOne(employeeNumber, departmentNumber);
        }

        public void InsertDepartmentManager(DepartmentManager manager)
        {
            try
            {
                _deptManagerRepository.Insert(manager);
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

        public void UpdateDepartmentManager(DepartmentManager manager)
        {
            _deptManagerRepository.Update(manager);
        }
    }
}
