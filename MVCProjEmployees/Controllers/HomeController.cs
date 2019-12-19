using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OA.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("empltab")]
        public IActionResult LoadEmployees() {
            return PartialView("../Partial/_EmployeeTable");
        }

        [HttpGet]
        [Route("departmenttab")]
        public IActionResult LoadDepartments() {
            return PartialView("../Partial/_DepartmentTable");
        }

        [HttpGet]
        [Route("salarytab")]
        public IActionResult LoadSalaries() {
            return PartialView("../Partial/_SalaryTable");
        }

        [HttpGet]
        [Route("employeePage")]
        public IActionResult LoadEmployeePage()
        {
            return PartialView("../Partial/_EmployeePage");
        }

        [HttpGet]
        [Route("departmentPage")]
        public IActionResult LoadDepartmentPage()
        {
            return PartialView("../Partial/_DepartmentPage");
        }
    }
}