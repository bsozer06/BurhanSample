using BurhanSample.Business.Abstract;
using BurhanSample.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BurhanSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private ICompanyManager _manager;
        private readonly ILoggerManager _logger;

        public CompaniesController(ICompanyManager manager, ILoggerManager logger)
        {
            _manager = manager;
            _logger = logger;
        }

        // api/companies
        [HttpGet]
        public IActionResult GetCompanies()
        {
            var result = _manager.GetCompanies();
            return Ok(result);
        }

        // api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3
        [HttpGet("{id}")]
        public IActionResult GetCompany(Guid id)
        {
            var result = _manager.GetCompany(id);
            return Ok(result);
        }
    }
}
