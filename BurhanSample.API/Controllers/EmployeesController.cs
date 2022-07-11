using BurhanSample.Business.Abstract;
using BurhanSample.Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BurhanSample.API.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private IEmployeeManager _manager;

        public EmployeesController(IEmployeeManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var result = _manager.GetEmployee(companyId, id, false);
            return Ok(result);
        }

        // api/employees
        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var result = _manager.GetEmployees(companyId, false);
            return Ok(result);
        }

        /// calismiyor
        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var result = _manager.CreateEmployeeForCompany(companyId, employee);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = result.Data.Id}, result.Data);
        }
    }
}
