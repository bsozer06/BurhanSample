using BurhanSample.API.Service.Abstract;
using BurhanSample.DAL.Abstract;
using BurhanSample.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BurhanSample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public CompaniesController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            try
            {
                var componies = _repository.Company.GetAllCompanies(trackChanges: false);

                var componiesDto = componies.Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    FullAddress = string.Join(' ', c.Address, c.Country)
                });

                return Ok(componiesDto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetCompanies)} action {ex}"); 
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
