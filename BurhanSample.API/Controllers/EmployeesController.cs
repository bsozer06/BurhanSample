using BurhanSample.Business.Abstract;
using BurhanSample.Core.Utilities.Models.RequestFeatures;
using BurhanSample.Entities.Dto;
using BurhanSample.Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BurhanSample.API.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private IEmployeeManager _manager;
        private readonly IDataShaper<EmployeeDto> _dataShaper;              /// ????


        public EmployeesController(IEmployeeManager manager, IDataShaper<EmployeeDto> dataShaper)
        {
            _manager = manager;
            _dataShaper = dataShaper;
        }


        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
        {
            var result = await _manager.GetEmployee(companyId, id);
            return Ok(result);
        }

        [HttpGet]
        // api/employees
        public async Task<IActionResult> GetEmployees(Guid companyId, [FromQuery]EmployeeParameters employeeParameters)
        {
            var result = await _manager.GetEmployees(companyId, employeeParameters);
            //return Ok(result);
            return Ok(_dataShaper.ShapeData(result.Data, employeeParameters.Fields));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var result = await _manager.CreateEmployeeForCompany(companyId, employee);
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
