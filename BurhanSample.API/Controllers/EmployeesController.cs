using BurhanSample.Business.Abstract;
using BurhanSample.Core.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BurhanSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IEmployeeManager _manager;
        private readonly ILoggerManager _logger;

        public EmployeesController(IEmployeeManager manager, ILoggerManager logger)
        {
            _manager = manager;
            _logger = logger;
        }


        // api/employees
        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var result = _manager.GetEmployees(companyId, false);
            return Ok(result);
        }
    }
}
