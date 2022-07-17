using BurhanSample.DAL.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BurhanSample.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/{v:apiversion}/Companies")]
    [ApiController]
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly IRepositoryCollection _repository;
        public CompaniesV2Controller(IRepositoryCollection repository) 
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies() 
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false);
            return Ok(companies);
        }
    }
}
