using System;

namespace MVCProjEmployees.Utils
{
    public class Constants
    {
        public static readonly int PageDefaultOffset = 0;
        public static readonly int PageDefaultLimit = 15;
        public static readonly string PageDefaultSort = "DESC";

        public static readonly string EmployeeModelDefaultFilter = "HireDate";
        public static readonly string DepartmentModelDefaultFilter = "DepartmentNumber";
        public static readonly string TitleModelDefaultFilter = "EmployeeTitle";
        public static readonly string SalaryModelDefaultFilter = "EmployeeSalary";
        public static readonly string DepartmentEmplyoeeDefaultFilter = "ToDate";
        public static readonly string DepartmentManagerDefaultFilter = "ToDate";

        public static readonly string NotFoundMessage = "The searched entity was not found in the database. Please review your search criteria.";
        public static readonly string BadRequestMessage = "Request data is invalid. Please change to valid data.";

        public static readonly int MySqlDuplicateErrorCode = 1062;
        public static readonly int MySqlInvalidConstraintsErrorCode = 1452;

        public static readonly DateTime DatabaseDefaultDate = new DateTime().AddYears(9998);
    }
}
