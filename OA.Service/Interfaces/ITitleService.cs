using OA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Service.Interfaces
{
    public interface ITitleService
    {
        IEnumerable<Title> GetTitles();
        IEnumerable<Title> GetTitlesByEmployeeNumber(int employeeNumber);
        IQueryable<Title> GetTitlesByFilters(int page, int pageSize, string sort, string filter);
        Title GetOneTitle(int employeeNumber, string title, DateTime fromDate);
        void InsertTitle(Title title);
        void UpdateTitle(Title title);
        int DeleteTitle(int employeeNumber, string title, DateTime fromDate);
        IQueryable<Title> GetTitlesByFiltersAndEmployeeNumber(int employeeNumber, int page, int pageSize, string sort, string filter);
    }
}
