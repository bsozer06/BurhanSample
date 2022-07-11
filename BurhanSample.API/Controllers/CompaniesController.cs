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


        [HttpGet]
        public IActionResult GetCompanies()
        {
            try
            {
                throw new Exception("Exception");
                var result = _manager.GetCompanies();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCompanies)} action {ex}");
                return StatusCode(500, "Internal server error");
            }

         
        }
    }
}
