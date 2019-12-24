using Microsoft.AspNetCore.Mvc;
using MVCProjEmployees.Models;
using MVCProjEmployees.Utils;
using MySql.Data.MySqlClient;
using OA.Data.Entities;
using OA.Repo.MySql.Extensions;
using OA.Service;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MVCProjEmployees.Controllers
{
    [Route("api/Departments")]
    [ApiController]
    [Produces("application/json")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IDepartmentEmployeeService _departmentEmployeeService;
        private readonly IDepartmentManagerService _departmentManagerService;
        public DepartmentsController(
            IDepartmentService departmentService,
            IDepartmentEmployeeService departmentEmployeeService,
            IDepartmentManagerService departmentManagerService)
        {
            _departmentService = departmentService;
            _departmentEmployeeService = departmentEmployeeService;
            _departmentManagerService = departmentManagerService;
        }

        #region DEPARTMENTS
        // GET: api/departments
        [HttpGet]
        public IActionResult GetAllDepartments(
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "filter")] string filter,
            [FromQuery(Name = "searchString")] string searchString
            )
        {
            page = (page <= 0) ? Constants.PageDefaultOffset : page;
            pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
            sort = sort ?? Constants.PageDefaultSort;
            filter = filter ?? Constants.DepartmentModelDefaultFilter;

            IQueryable<Department> myQuery =
                _departmentService
                .GetDepartmentQuery();

            if (searchString != null && !searchString.Equals(""))
            {
                myQuery =
                    myQuery.Where(e =>
                        e.DepartmentNumber.ToString().Contains(searchString.ToUpper())
                        || e.DepartmentName.ToUpper().Contains(searchString.ToUpper())
                        );
            }

            List<DepartmentModel> departmentList =
                myQuery
                    .OrderByDynamic(filter, sort)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Select(e => new DepartmentModel
                    {
                        DepartmentName = e.DepartmentName,
                        DepartmentNumber = e.DepartmentNumber
                    })
                    .ToList();

            if (departmentList.Count <= 0)
            {
                return NoContent();
            }

            return Ok(departmentList);
        }

        // GET api/departments/{dep-no}
        [HttpGet("{departmentNumber}")]
        public IActionResult GetDepartmentByDepartmentNumber(string departmentNumber)
        {
            Department dept = _departmentService.GetOneDepartment(departmentNumber);

            if (dept == null) return NoContent();

            DepartmentModel departmentModel = new DepartmentModel
            {
                DepartmentNumber = dept.DepartmentNumber,
                DepartmentName = dept.DepartmentName
            };

            return Ok(departmentModel);
        }

        // POST api/departments
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult PostDepartment(
            [FromBody]DepartmentModel department
            )
        {
            try
            {
                Department lastDepartment = _departmentService.GetDepartments().OrderByDescending(prop => prop.DepartmentNumber).First();

                string departmentNumber = UtilityKit.ConstructDepartmentNumber(lastDepartment.DepartmentNumber.Substring(1));

                department.DepartmentNumber = departmentNumber;

                _departmentService.InsertDepartment(new Department { DepartmentNumber = department.DepartmentNumber, DepartmentName = department.DepartmentName });

                return Created($"/api/departments/{department.DepartmentNumber}", department);
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

        // PUT api/departments/{dep-no}
        [HttpPut("{departmentNumber}")]
        public IActionResult PutDepartment(
            string departmentNumber,
            [FromBody]DepartmentModel department
            )
        {
            Department departmentToUpdate = new Department
            {
                DepartmentNumber = departmentNumber,
                DepartmentName = department.DepartmentName
            };

            _departmentService.UpdateDepartment(departmentToUpdate);

            DepartmentModel updatedDepartment = new DepartmentModel
            {
                DepartmentNumber = departmentToUpdate.DepartmentNumber,
                DepartmentName = departmentToUpdate.DepartmentName
            };

            return Ok(updatedDepartment);
        }

        //// DELETE api/departments/{dep-no}
        [HttpDelete("{departmentNumber}")]
        public IActionResult DeleteDepartment(string departmentNumber)
        {
            int deleteCode = _departmentService.DeleteDepartment(departmentNumber);

            if (deleteCode <= 0)
            {
                return NotFound(APIResponse.ApiNotFound());
            }

            return Ok();
        }

        #endregion DEPARTMENTS

        #region DEPARTMENT EMPLOYEES

        [HttpGet("employees")]
        public IActionResult GetAllDepartmentEmployees(
        [FromQuery(Name = "page")] int page,
        [FromQuery(Name = "pageSize")] int pageSize,
        [FromQuery(Name = "sort")] string sort,
        [FromQuery(Name = "filter")] string filter,
        [FromQuery(Name = "currentlyEmployed")] bool currentlyEmployed
        )
        {
            page = (page <= 0) ? Constants.PageDefaultOffset : page;
            pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
            sort = sort ?? Constants.PageDefaultSort;
            filter = filter ?? Constants.DepartmentEmplyoeeDefaultFilter;

            List<DepartmentEmployeeModel> departmentList = new List<DepartmentEmployeeModel>();
            _departmentEmployeeService.GetDepartmentEmployees()
                .Where(e =>
                {
                    if (currentlyEmployed)
                    {
                        return e.ToDate.Equals(Constants.DatabaseDefaultDate);
                    }
                    else
                    {
                        return true;
                    }
                })
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .ForEach(d =>
                {
                    DepartmentEmployeeModel department = new DepartmentEmployeeModel
                    {
                        EmployeeNumber = d.EmployeeNumber,
                        DepartmentNumber = d.DepartmentNumber,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    };

                    departmentList.Add(department);
                });

            if (departmentList.Count <= 0)
            {
                return NoContent();
            }

            return Ok(departmentList);
        }

        // GET api/departments/{dep-no}
        [HttpGet("employees/department-employees/{departmentNumber}")]
        public IActionResult GetAllDepartmentEmployeesByDepartmentNumber(
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "pageSize")] int pageSize,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "filter")] string filter,
            [FromQuery(Name = "currentlyEmployed")] bool currentlyEmployed,
            string departmentNumber)
        {
            page = (page <= 0) ? Constants.PageDefaultOffset : page;
            pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
            sort = sort ?? Constants.PageDefaultSort;
            filter = filter ?? Constants.DepartmentEmplyoeeDefaultFilter;

            List<DepartmentEmployeeModel> departmentList = new List<DepartmentEmployeeModel>();
            _departmentEmployeeService
                .GetDepartmentEmployeeByDepartmentNumber(departmentNumber)
                .Where(e =>
                {
                    if (currentlyEmployed)
                    {
                        return e.ToDate.Equals(Constants.DatabaseDefaultDate);
                    }
                    else
                    {
                        return true;
                    }
                })
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .ForEach(d =>
                {
                    DepartmentEmployeeModel department = new DepartmentEmployeeModel
                    {
                        EmployeeNumber = d.EmployeeNumber,
                        DepartmentNumber = d.DepartmentNumber,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    };
                    departmentList.Add(department);
                });

            if (departmentList.Count <= 0)
            {
                return NoContent();
            }

            return Ok(departmentList);
        }

        [HttpGet("employees/employee-departments/{employeeNumber}")]
        public IActionResult GetAllDepartmentEmployeesByEmployeeNumber(
                [FromQuery(Name = "page")] int page,
                [FromQuery(Name = "pageSize")] int pageSize,
                [FromQuery(Name = "sort")] string sort,
                [FromQuery(Name = "filter")] string filter,
                [FromQuery(Name = "currentlyEmployed")] bool currentlyEmployed,
                int employeeNumber
                )
        {
            page = (page <= 0) ? Constants.PageDefaultOffset : page;
            pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
            sort = sort ?? Constants.PageDefaultSort;
            filter = filter ?? Constants.DepartmentEmplyoeeDefaultFilter;

            List<DepartmentEmployeeModel> departmentList = new List<DepartmentEmployeeModel>();
            _departmentEmployeeService.GetDepartmentEmployeeByEmployeeNumber(employeeNumber)
                .Where(e =>
                {
                    if (currentlyEmployed)
                    {
                        return e.ToDate.Equals(Constants.DatabaseDefaultDate);
                    }
                    else
                    {
                        return true;
                    }
                })
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .ForEach(d =>
                {
                    DepartmentEmployeeModel department = new DepartmentEmployeeModel
                    {
                        EmployeeNumber = d.EmployeeNumber,
                        DepartmentNumber = d.DepartmentNumber,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate,
                        Department = d.Department
                    };
                    departmentList.Add(department);
                });

            if (departmentList.Count <= 0)
            {
                return NoContent();
            }

            return Ok(departmentList);
        }

        // POST api/departments
        [HttpPost("employees")]
        [Consumes("application/json")]
        public IActionResult PostDepartmentEmployee(
            [FromBody]DepartmentEmployeeModel departmentEmployee
            )
        {
            try
            {
                _departmentEmployeeService.InsertDepartmentEmployee(
                    new DepartmentEmployee
                    {
                        EmployeeNumber = departmentEmployee.EmployeeNumber,
                        DepartmentNumber = departmentEmployee.DepartmentNumber,
                        FromDate = departmentEmployee.FromDate,
                        ToDate = departmentEmployee.ToDate
                    });

                return Created($"/api/departments/{departmentEmployee.DepartmentNumber}/employees/{departmentEmployee.EmployeeNumber}", departmentEmployee);
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

        // PUT api/departments/{dep-no}
        [HttpPut("{departmentNumber}/employees/{employeeNumber}")]
        public IActionResult PutDepartmentEmployee(
            string departmentNumber,
            int employeeNumber,
            [FromBody]DepartmentEmployeeModel department
            )
        {
            DepartmentEmployee departmentToUpdate = new DepartmentEmployee
            {
                EmployeeNumber = employeeNumber,
                DepartmentNumber = departmentNumber,
                FromDate = department.FromDate,
                ToDate = department.ToDate
            };

            _departmentEmployeeService.UpdateDepartmentEmployee(departmentToUpdate);

            DepartmentEmployeeModel updatedDepartment = new DepartmentEmployeeModel
            {
                EmployeeNumber = departmentToUpdate.EmployeeNumber,
                DepartmentNumber = departmentToUpdate.DepartmentNumber,
                FromDate = departmentToUpdate.FromDate,
                ToDate = departmentToUpdate.ToDate
            };

            return Ok(updatedDepartment);
        }

        //// DELETE api/departments/{dep-no}
        [HttpDelete("{departmentNumber}/employees/{employeeNumber}")]
        public IActionResult DeleteDepartmentEmployee(string departmentNumber, int employeeNumber)
        {
            int deleteCode = _departmentEmployeeService.DeleteDepartmentEmployee(employeeNumber, departmentNumber);

            if (deleteCode <= 0)
            {
                return NotFound(APIResponse.ApiNotFound());
            }

            return Ok();
        }

        #endregion DEPARTMENT EMPLOYEES

        #region DEPARTMENT MANAGERS

        [HttpGet("managers")]
        public IActionResult GetAllDepartmentManagers(
                [FromQuery(Name = "page")] int page,
                [FromQuery(Name = "pageSize")] int pageSize,
                [FromQuery(Name = "sort")] string sort,
                [FromQuery(Name = "filter")] string filter,
                [FromQuery(Name = "currentManagers")] bool currentManagers
                )
        {
            page = (page <= 0) ? Constants.PageDefaultOffset : page;
            pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
            sort = sort ?? Constants.PageDefaultSort;
            filter = filter ?? Constants.DepartmentManagerDefaultFilter;

            List<DepartmentManagerModel> departmentList = new List<DepartmentManagerModel>();
            _departmentManagerService.GetDepartmentManagers()
                .Where(e =>
                {
                    if (currentManagers)
                    {
                        return e.ToDate.Equals(Constants.DatabaseDefaultDate);
                    }
                    else
                    {
                        return true;
                    }
                })
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .ForEach(d =>
                {
                    DepartmentManagerModel department = new DepartmentManagerModel
                    {
                        EmployeeNumber = d.EmployeeNumber,
                        DepartmentNumber = d.DepartmentNumber,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    };

                    departmentList.Add(department);
                });

            if (departmentList.Count <= 0)
            {
                return NoContent();
            }

            return Ok(departmentList);
        }

        // GET api/departments/{dep-no}
        [HttpGet("managers/department-managers/{departmentNumber}")]
        public IActionResult GetAllDepartmentManagersByDepartmentNumber(
        [FromQuery(Name = "page")] int page,
        [FromQuery(Name = "pageSize")] int pageSize,
        [FromQuery(Name = "sort")] string sort,
        [FromQuery(Name = "filter")] string filter,
        [FromQuery(Name = "currentManagers")] bool currentManagers,
        string departmentNumber
        )
        {
            page = (page <= 0) ? Constants.PageDefaultOffset : page;
            pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
            sort = sort ?? Constants.PageDefaultSort;
            filter = filter ?? Constants.DepartmentManagerDefaultFilter;

            List<DepartmentManagerModel> departmentList = new List<DepartmentManagerModel>();
            _departmentManagerService.GetDepartmentManagerByDepartmentNumber(departmentNumber)
                .Where(e =>
                {
                    if (currentManagers)
                    {
                        return e.ToDate.Equals(Constants.DatabaseDefaultDate);
                    }
                    else
                    {
                        return true;
                    }
                })
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .ForEach(d =>
                {
                    DepartmentManagerModel department = new DepartmentManagerModel
                    {
                        EmployeeNumber = d.EmployeeNumber,
                        DepartmentNumber = d.DepartmentNumber,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    };

                    departmentList.Add(department);
                });

            if (departmentList.Count <= 0)
            {
                return NoContent();
            }

            return Ok(departmentList);
        }

        [HttpGet("managers/manager-departments/{employeeNumber}")]
        public IActionResult GetAllDepartmentManagersByEmployeeNumber(
                [FromQuery(Name = "page")] int page,
                [FromQuery(Name = "pageSize")] int pageSize,
                [FromQuery(Name = "sort")] string sort,
                [FromQuery(Name = "filter")] string filter,
                [FromQuery(Name = "currentManagers")] bool currentManagers,
                int employeeNumber
                )
        {
            page = (page <= 0) ? Constants.PageDefaultOffset : page;
            pageSize = (pageSize <= 0) ? Constants.PageDefaultLimit : pageSize;
            sort = sort ?? Constants.PageDefaultSort;
            filter = filter ?? Constants.DepartmentManagerDefaultFilter;

            List<DepartmentManagerModel> departmentList = new List<DepartmentManagerModel>();
            _departmentManagerService.GetDepartmentManagerByEmployeeNumber(employeeNumber)
                .Where(e =>
                {
                    if (currentManagers)
                    {
                        return e.ToDate.Equals(Constants.DatabaseDefaultDate);
                    }
                    else
                    {
                        return true;
                    }
                })
                .AsQueryable()
                .OrderByDynamic(filter, sort)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .ForEach(d =>
                {
                    DepartmentManagerModel department = new DepartmentManagerModel
                    {
                        EmployeeNumber = d.EmployeeNumber,
                        DepartmentNumber = d.DepartmentNumber,
                        FromDate = d.FromDate,
                        ToDate = d.ToDate
                    };
                    departmentList.Add(department);
                });

            if (departmentList.Count <= 0)
            {
                return NoContent();
            }

            return Ok(departmentList);
        }

        // POST api/departments
        [HttpPost("managers")]
        [Consumes("application/json")]
        public IActionResult PostDepartmentManager(
            [FromBody]DepartmentManagerModel departmentManager
            )
        {
            try
            {
                _departmentManagerService.InsertDepartmentManager(
                    new DepartmentManager
                    {
                        EmployeeNumber = departmentManager.EmployeeNumber,
                        DepartmentNumber = departmentManager.DepartmentNumber,
                        FromDate = departmentManager.FromDate,
                        ToDate = departmentManager.ToDate
                    });

                return Created($"/api/departments/{departmentManager.DepartmentNumber}/managers/{departmentManager.EmployeeNumber}", departmentManager);
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

        // PUT api/departments/{dep-no}
        [HttpPut("{departmentNumber}/managers/{employeeNumber}")]
        public IActionResult PutDepartmentManager(
            string departmentNumber,
            int employeeNumber,
            [FromBody]DepartmentManagerModel department
            )
        {
            DepartmentManager departmentToUpdate = new DepartmentManager
            {
                EmployeeNumber = employeeNumber,
                DepartmentNumber = departmentNumber,
                FromDate = department.FromDate,
                ToDate = department.ToDate
            };

            _departmentManagerService.UpdateDepartmentManager(departmentToUpdate);

            DepartmentManagerModel updatedDepartment = new DepartmentManagerModel
            {
                EmployeeNumber = departmentToUpdate.EmployeeNumber,
                DepartmentNumber = departmentToUpdate.DepartmentNumber,
                FromDate = departmentToUpdate.FromDate,
                ToDate = departmentToUpdate.ToDate
            };

            return Ok(updatedDepartment);
        }

        //// DELETE api/departments/{dep-no}
        [HttpDelete("{departmentNumber}/managers/{employeeNumber}")]
        public IActionResult DeleteDepartmentManager(string departmentNumber, int employeeNumber)
        {
            int deleteCode = _departmentManagerService.DeleteDepartmentManager(employeeNumber, departmentNumber);

            if (deleteCode <= 0)
            {
                return NotFound(APIResponse.ApiNotFound());
            }

            return Ok();
        }

        #endregion DEPARTMENT MANAGERS

    }
}
