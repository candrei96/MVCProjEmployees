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

    public class TitleService : ITitleService
    {
        private readonly IRepository<Title> _titleRepository;

        public TitleService(IRepository<Title> titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public IEnumerable<Title> GetTitles()
        {
            return _titleRepository
                .GetAll()
                .AsQueryable()
                .Include(t => t.Employee)
                .AsEnumerable();
        }

        public IEnumerable<Title> GetTitlesByEmployeeNumber(int employeeNumber)
        {
            return _titleRepository.GetAll().Where(prop => prop.EmployeeNumber == employeeNumber);
        }

        public Title GetOneTitle(int employeeNumber, string title, DateTime fromDate)
        {
            return _titleRepository.GetOne(employeeNumber, title, fromDate);
        }

        public void InsertTitle(Title title)
        {
            try
            {
                _titleRepository.Insert(title);
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

        public void UpdateTitle(Title title)
        {
            _titleRepository.Update(title);
        }

        public int DeleteTitle(int employeeNumber, string title, DateTime fromDate)
        {
            Title _title = GetOneTitle(employeeNumber, title, fromDate);
            if (_title == null) return -1;

            _titleRepository.Remove(_title);
            _titleRepository.SaveChanges();
            return 1;
        }

        public IQueryable<Title> GetTitlesByFilters(int page, int pageSize, string sort, string filter)
        {
            return _titleRepository.GetAll()
                .AsQueryable()
                .Include(t => t.Employee)
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize);
        }
    }
}
