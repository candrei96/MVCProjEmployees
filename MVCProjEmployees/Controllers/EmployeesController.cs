using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MVCProjEmployees.Models;
using MVCProjEmployees.Utils;
using MySql.Data.MySqlClient;
using OA.Data.Entities;
using OA.Repo.MySql.Extensions;
using OA.Service;
using OA.Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCProjEmployees.Controllers
{
    [Route("api/employees")]
    [ApiController]
    [Produces("application/json")]
    public class EmployeesController : Controller
    {
        private IEmployeeService _employeeService { get; set; }
        private ITitleService _titleService { get; set; }
        private ISalaryService _salaryService { get; set; }

        public EmployeesController(IEmployeeService employeeService, ITitleService titleService, ISalaryService salaryService)
        {
            _employeeService = employeeService;
            _titleService = titleService;
            _salaryService = salaryService;
        }

        #region SALARIES

        [HttpGet("salaries")]
        public IActionResult GetAllSalaries(
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "filter")] string filter
            )
        {
            try
            {
                page = (page <= 0) ? Constants.PageDefaultOffset : page;
                pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
                sort = sort ?? Constants.PageDefaultSort;
                filter = filter ?? Constants.SalaryModelDefaultFilter;

                List<SalaryModel> salaryList = new List<SalaryModel>();

                _salaryService.GetSalariesByFilters(page, pageSize, sort, filter)
                    .ToList()
                    .ForEach(foundSalary =>
                    {
                        SalaryModel s = new SalaryModel
                        {
                            Employee = foundSalary.Employee,
                            EmployeeNumber = foundSalary.EmployeeNumber,
                            EmployeeSalary = foundSalary.EmployeeSalary,
                            FromDate = foundSalary.FromDate,
                            ToDate = foundSalary.ToDate
                        };
                        salaryList.Add(s);
                    });

                if (salaryList.Count <= 0)
                {
                    return NoContent();
                }

                return Ok(salaryList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpGet("{empNo}/salaries")]
        public IActionResult GetSalariesByEmployeeNumber(
            [FromQuery] bool currentSalaries,
            int empNo
            )
        {
            try
            {
                List<SalaryModel> salaryModels = new List<SalaryModel>();

                _salaryService
                    .GetSalaries()
                    .Where(s => s.EmployeeNumber == empNo)
                    .ToList()
                    .ForEach(foundSalary =>
                    {
                        SalaryModel s = new SalaryModel
                        {
                            Employee = foundSalary.Employee,
                            EmployeeNumber = foundSalary.EmployeeNumber,
                            EmployeeSalary = foundSalary.EmployeeSalary,
                            FromDate = foundSalary.FromDate,
                            ToDate = foundSalary.ToDate
                        };
                        if (currentSalaries)
                        {
                            if (foundSalary.ToDate.Equals(Constants.DatabaseDefaultDate))
                            {
                                salaryModels.Add(s);
                            }
                        }
                        else
                        {
                            salaryModels.Add(s);
                        }
                    });

                if (salaryModels.Count <= 0)
                {
                    return NoContent();
                }

                return Ok(salaryModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpGet("{empNo}/salaries/startDate/{fromDate}")]
        public IActionResult GetOneSalary(int empNo, DateTime fromDate)
        {
            try
            {
                Salary s = _salaryService.GetOneSalary(empNo, fromDate);

                if (s == null) return NoContent();

                SalaryModel resultSalary = new SalaryModel
                {
                    Employee = s.Employee,
                    EmployeeNumber = s.EmployeeNumber,
                    EmployeeSalary = s.EmployeeSalary,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate
                };

                return Ok(resultSalary);

            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpGet("salaries/search")]
        public IActionResult SearchSalaries(
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "filter")] string filter,
            [FromQuery(Name = "searchString")]string searchString
            )
        {
            try
            {
                page = (page <= 0) ? Constants.PageDefaultOffset : page;
                pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
                sort = sort ?? Constants.PageDefaultSort;
                filter = filter ?? Constants.SalaryModelDefaultFilter;

                List<SalaryModel> salaryList = new List<SalaryModel>();

                _salaryService
                .GetSalaries()
                    .Where(s =>
                    s.EmployeeNumber.ToString().Contains(searchString)
                    || s.EmployeeSalary.ToString().Contains(searchString)
                    || s.FromDate.Day.ToString().Contains(searchString) || s.FromDate.Month.ToString().Contains(searchString) || s.FromDate.Year.ToString().Contains(searchString)
                    || s.ToDate.Day.ToString().Contains(searchString) || s.ToDate.Month.ToString().Contains(searchString) || s.ToDate.Year.ToString().Contains(searchString)
                    )
                    .AsQueryable()
                    .OrderByDynamic(filter, sort)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList()
                    .ForEach(s =>
                    {
                        SalaryModel salary = new SalaryModel
                        {
                            Employee = s.Employee,
                            EmployeeNumber = s.EmployeeNumber,
                            EmployeeSalary = s.EmployeeSalary,
                            FromDate = s.FromDate,
                            ToDate = s.ToDate
                        };
                        salaryList.Add(salary);
                    });

                return Ok(salaryList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpPost("salaries")]
        [Consumes("application/json")]
        public IActionResult PostSalary(
            [FromBody]SalaryModel salaryModel
            )
        {
            try
            {
                Salary salaryToInsert = new Salary
                {
                    EmployeeNumber = salaryModel.EmployeeNumber,
                    EmployeeSalary = salaryModel.EmployeeSalary,
                    FromDate = salaryModel.FromDate,
                    ToDate = salaryModel.ToDate
                };

                _salaryService.InsertSalary(salaryToInsert);

                return Created($"/api/employees/{salaryModel.EmployeeNumber}/salaries/startDate/{salaryModel.FromDate}", salaryModel);
            }
            catch (GenericServiceException genericServiceException)
            {
                if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.ConflictException))
                {
                    return StatusCode(genericServiceException.StatusCode, APIResponse.ApiConflict(genericServiceException.Message));
                }
                else if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.BadConstraintsException))
                {
                    return StatusCode(genericServiceException.StatusCode, APIResponse.BadRequest());
                }
                else if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.InternalErrorException))
                {
                    return StatusCode(500, APIResponse.DefaultErrorMessage(genericServiceException.Message, 500));
                }

                return StatusCode(500, APIResponse.DefaultErrorMessage(genericServiceException.Message, 500));
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpPut("{empNo}/salaries/startDate/{fromDate}")]
        [Consumes("application/json")]
        public IActionResult PutSalary(
            int empNo,
            DateTime fromDate,
            [FromBody]SalaryModel salaryModel
            )
        {
            try
            {
                Salary salaryToUpdate = new Salary
                {
                    EmployeeNumber = empNo,
                    EmployeeSalary = salaryModel.EmployeeSalary,
                    FromDate = fromDate,
                    ToDate = salaryModel.ToDate
                };

                _salaryService.UpdateSalary(salaryToUpdate);

                SalaryModel updatedSalary = new SalaryModel
                {
                    EmployeeNumber = salaryToUpdate.EmployeeNumber,
                    EmployeeSalary = salaryToUpdate.EmployeeSalary,
                    FromDate = salaryToUpdate.FromDate,
                    ToDate = salaryToUpdate.ToDate
                };

                return Ok(updatedSalary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpDelete("{empNo}/salaries/startDate/{fromDate}")]
        public IActionResult DeleteSalary(
            int empNo,
            DateTime fromDate
            )
        {
            try
            {
                int deleteCode = _salaryService.DeleteSalary(empNo, fromDate);

                if (deleteCode <= 0)
                {
                    return NotFound(APIResponse.ApiNotFound());
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }


        #endregion SALARIES

        #region TITLES

        [HttpGet("titles")]
        public IActionResult GetAllTitles(
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "filter")] string filter
            )
        {
            try
            {
                page = (page <= 0) ? Constants.PageDefaultOffset : page;
                pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
                sort = sort ?? Constants.PageDefaultSort;
                filter = filter ?? Constants.TitleModelDefaultFilter;

                List<TitleModel> titleList = new List<TitleModel>();

                _titleService.GetTitlesByFilters(page, pageSize, sort, filter)
                    .ToList()
                    .ForEach(foundTitle =>
                    {
                        TitleModel t = new TitleModel
                        {
                            Employee = foundTitle.Employee,
                            EmployeeNumber = foundTitle.EmployeeNumber,
                            EmployeeTitle = foundTitle.Title,
                            FromDate = foundTitle.FromDate,
                            ToDate = foundTitle.ToDate
                        };
                        titleList.Add(t);
                    });

                if (titleList.Count <= 0)
                {
                    return NoContent();
                }

                return Ok(titleList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpGet("{empNo}/titles")]
        public IActionResult GetTitlesByEmployeeNumber(
            [FromQuery] bool currentTitles,
            int empNo
            )
        {
            try
            {
                List<TitleModel> titleModels = new List<TitleModel>();

                _titleService
                    .GetTitles()
                    .Where(t => t.EmployeeNumber == empNo)
                    .ToList().ForEach(foundTitle =>
                    {
                        TitleModel t = new TitleModel
                        {
                            Employee = foundTitle.Employee,
                            EmployeeNumber = foundTitle.EmployeeNumber,
                            EmployeeTitle = foundTitle.Title,
                            FromDate = foundTitle.FromDate,
                            ToDate = foundTitle.ToDate
                        };
                        if (currentTitles)
                        {
                            if (foundTitle.ToDate.Equals(Constants.DatabaseDefaultDate))
                            {
                                titleModels.Add(t);
                            }
                        }
                        else
                        {
                            titleModels.Add(t);
                        }
                    });

                if (titleModels.Count <= 0)
                {
                    return NoContent();
                }

                return Ok(titleModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpGet("{empNo}/titles/{title}/startDate/{fromDate}")]
        public IActionResult GetOneTitle(int empNo, string title, DateTime fromDate)
        {
            try
            {
                Title t = _titleService.GetOneTitle(empNo, title, fromDate);

                if (t == null) return NoContent();

                TitleModel resultTitle = new TitleModel
                {
                    Employee = t.Employee,
                    EmployeeNumber = t.EmployeeNumber,
                    EmployeeTitle = t.Title,
                    FromDate = t.FromDate,
                    ToDate = t.ToDate
                };

                return Ok(resultTitle);

            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpPost("titles")]
        [Consumes("application/json")]
        public IActionResult PostTitle(
            [FromBody]TitleModel titleModel
            )
        {
            try
            {
                Title titleToInsert = new Title
                {
                    EmployeeNumber = titleModel.EmployeeNumber,
                    Title = titleModel.EmployeeTitle,
                    FromDate = titleModel.FromDate,
                    ToDate = titleModel.ToDate
                };

                _titleService.InsertTitle(titleToInsert);

                return Created($" /api/employees/{titleModel.EmployeeNumber}/titles/{titleModel.EmployeeTitle}/startDate/{titleModel.FromDate}", titleModel);
            }
            catch (GenericServiceException genericServiceException)
            {
                if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.ConflictException))
                {
                    return StatusCode(genericServiceException.StatusCode, APIResponse.ApiConflict(genericServiceException.Message));
                }
                else if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.BadConstraintsException))
                {
                    return StatusCode(genericServiceException.StatusCode, APIResponse.BadRequest());
                }
                else if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.InternalErrorException))
                {
                    return StatusCode(500, APIResponse.DefaultErrorMessage(genericServiceException.Message, 500));
                }

                return StatusCode(500, APIResponse.DefaultErrorMessage(genericServiceException.Message, 500));
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpPut("{empNo}/titles/{title}/startDate/{fromDate}")]
        [Consumes("application/json")]
        public IActionResult PutTitle(
            int empNo,
            string title,
            DateTime fromDate,
            [FromBody]TitleModel titleModel
            )
        {
            try
            {
                Title titleToUpdate = new Title
                {
                    EmployeeNumber = empNo,
                    Title = title,
                    FromDate = fromDate,
                    ToDate = titleModel.ToDate
                };

                _titleService.UpdateTitle(titleToUpdate);

                TitleModel updatedTitle = new TitleModel
                {
                    EmployeeNumber = titleToUpdate.EmployeeNumber,
                    EmployeeTitle = titleToUpdate.Title,
                    FromDate = titleToUpdate.FromDate,
                    ToDate = titleToUpdate.ToDate
                };

                return Ok(updatedTitle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpDelete("{empNo}/titles/{title}/startDate/{fromDate}")]
        public IActionResult DeleteTitle(
            int empNo,
            string title,
            DateTime fromDate
            )
        {
            try
            {
                int deleteCode = _titleService.DeleteTitle(empNo, title, fromDate);

                if (deleteCode <= 0)
                {
                    return NotFound(APIResponse.ApiNotFound());
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        #endregion TITLES


        #region EMPLOYEES
        // GET: api/employees
        [HttpGet]
        public IActionResult GetAllEmployees(
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "filter")] string filter
            )
        {
            try
            {
                page = (page <= 0) ? Constants.PageDefaultOffset : page;
                pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
                sort = sort ?? Constants.PageDefaultSort;
                filter = filter ?? Constants.EmployeeModelDefaultFilter;

                List<EmployeeModel> employeeList = new List<EmployeeModel>();

                _employeeService.GetEmployeesByFilters(page, pageSize, sort, filter)
                    .ToList()
                    .ForEach(e =>
                    {
                        EmployeeModel employee = new EmployeeModel
                        {
                            EmployeeNumber = e.EmployeeNumber,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            EmployeeGender = e.Gender,
                            BirthDate = e.BirthDate,
                            HireDate = e.HireDate
                        };
                        employeeList.Add(employee);
                    });

                if (employeeList.Count <= 0)
                {
                    return NoContent();
                }

                return Ok(employeeList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpGet("{empNo}")]
        public IActionResult GetOneEmployee(int empNo)
        {
            try
            {
                Employee e = _employeeService.GetOneEmployee(empNo);

                if (e == null) return NoContent();

                EmployeeModel employeeModel = new EmployeeModel
                {
                    EmployeeNumber = e.EmployeeNumber,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    EmployeeGender = e.Gender,
                    BirthDate = e.BirthDate,
                    HireDate = e.HireDate
                };

                return Ok(employeeModel);

            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpGet("search")]
        public IActionResult SearchEmployees(
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "filter")] string filter,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "searchString")]string searchString
            )
        {
            try
            {
                page = (page <= 0) ? Constants.PageDefaultOffset : page;
                pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
                sort = sort ?? Constants.PageDefaultSort;
                filter = filter ?? Constants.EmployeeModelDefaultFilter;

                List<EmployeeModel> employeeList = new List<EmployeeModel>();

                if (int.TryParse(searchString, out _))
                {
                    _employeeService
                    .GetEmployees()
                        .Where(e =>
                        e.EmployeeNumber.ToString().Contains(searchString)
                        || (e.BirthDate.Year.ToString() + "-" + e.BirthDate.Month.ToString() + "-" + e.BirthDate.Day.ToString()).Contains(searchString)
                        || (e.HireDate.Year.ToString() + "-" + e.HireDate.Month.ToString() + "-" + e.HireDate.Day.ToString()).Contains(searchString)
                        )
                        .AsQueryable()
                        .OrderByDynamic(filter, sort)
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .ToList()
                        .ForEach(e =>
                        {
                            EmployeeModel employee = new EmployeeModel
                            {
                                EmployeeNumber = e.EmployeeNumber,
                                FirstName = e.FirstName,
                                LastName = e.LastName,
                                EmployeeGender = e.Gender,
                                BirthDate = e.BirthDate,
                                HireDate = e.HireDate
                            };
                            employeeList.Add(employee);
                        });
                }
                else
                {
                    _employeeService
                    .GetEmployees()
                        .Where(e =>
                        e.FirstName.ToUpper().Contains(searchString.ToUpper())
                        || e.LastName.ToUpper().Contains(searchString.ToUpper())
                        || e.Gender.ToUpper().Contains(searchString.ToUpper())
                        || (e.BirthDate.Year.ToString() + "-" + e.BirthDate.Month.ToString() + "-" + e.BirthDate.Day.ToString()).Contains(searchString)
                        || (e.HireDate.Year.ToString() + "-" + e.HireDate.Month.ToString() + "-" + e.HireDate.Day.ToString()).Contains(searchString)
                        )
                        .AsQueryable()
                        .OrderByDynamic(filter, sort)
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .ToList()
                        .ForEach(e =>
                        {
                            EmployeeModel employee = new EmployeeModel
                            {
                                EmployeeNumber = e.EmployeeNumber,
                                FirstName = e.FirstName,
                                LastName = e.LastName,
                                EmployeeGender = e.Gender,
                                BirthDate = e.BirthDate,
                                HireDate = e.HireDate
                            };
                            employeeList.Add(employee);
                        });
                }

                return Ok(employeeList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult PostEmployee(
            [FromBody]EmployeeModel employee
            )
        {
            try
            {
                Employee lastEmployee = _employeeService.GetEmployees().OrderByDescending(prop => prop.EmployeeNumber).First();

                int newEmployeeId = lastEmployee.EmployeeNumber + 1;

                employee.EmployeeNumber = newEmployeeId;

                Employee employeeToInsert = new Employee
                {
                    EmployeeNumber = newEmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.EmployeeGender,
                    BirthDate = employee.BirthDate,
                    HireDate = employee.HireDate
                };

                _employeeService.InsertEmployee(employeeToInsert);

                return Created($"/api/employees/{newEmployeeId}", employee);
            }
            catch (GenericServiceException genericServiceException)
            {
                if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.ConflictException))
                {
                    return StatusCode(genericServiceException.StatusCode, APIResponse.ApiConflict(genericServiceException.Message));
                }
                else if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.BadConstraintsException))
                {
                    return StatusCode(genericServiceException.StatusCode, APIResponse.BadRequest());
                }
                else if (genericServiceException.GenericExceptionResponse.Equals(GenericExceptionResponse.InternalErrorException))
                {
                    return StatusCode(500, APIResponse.DefaultErrorMessage(genericServiceException.Message, 500));
                }

                return StatusCode(500, APIResponse.DefaultErrorMessage(genericServiceException.Message, 500));
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpPut("{employeeNumber}")]
        [Consumes("application/json")]
        public IActionResult PutEmployee(
            int employeeNumber,
            [FromBody]EmployeeModel employee
            )
        {
            try
            {
                Employee employeeToUpdate = new Employee
                {
                    EmployeeNumber = employeeNumber,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Gender = employee.EmployeeGender,
                    BirthDate = employee.BirthDate,
                    HireDate = employee.HireDate
                };

                _employeeService.UpdateEmployee(employeeToUpdate);

                EmployeeModel updatedEmployee = new EmployeeModel
                {
                    EmployeeNumber = employeeToUpdate.EmployeeNumber,
                    FirstName = employeeToUpdate.FirstName,
                    LastName = employeeToUpdate.LastName,
                    EmployeeGender = employeeToUpdate.Gender,
                    BirthDate = employeeToUpdate.BirthDate,
                    HireDate = employeeToUpdate.HireDate
                };

                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }

        [HttpDelete("{employeeNumber}")]
        public IActionResult DeleteEmployeeByEmployeeNumber(
            int employeeNumber
            )
        {
            try
            {
                int deleteCode = _employeeService.DeleteEmployee(employeeNumber);

                if (deleteCode <= 0)
                {
                    return NotFound(APIResponse.ApiNotFound());
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, APIResponse.DefaultErrorMessage(ex.Message, 500));
            }
        }
        #endregion EMPLOYEES

    }
}
