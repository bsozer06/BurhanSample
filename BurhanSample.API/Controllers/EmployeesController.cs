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

        
        [HttpGet("{id}", Name = "GetEmployee")]
        public IActionResult GetEmployee(Guid companyId, Guid id)
        {
            var result = _manager.GetEmployee(companyId, id, false);
            return Ok(result);
        }

        [HttpGet]
        // api/employees
        public IActionResult GetEmployees(Guid companyId)
        {
            var result = _manager.GetEmployees(companyId, false);
            return Ok(result);
        }

        // calismayabilir.
        [HttpPost]
        public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var result = _manager.CreateEmployeeForCompany(companyId, employee);
            return CreatedAtRoute("GetEmployee", new { companyId, id = result.Data.Id }, result.Data);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid companyId, Guid id)
        {
            var result = _manager.DeleteEmployeeForCompany(companyId, id);
            return NoContent();
        }



    }
}
