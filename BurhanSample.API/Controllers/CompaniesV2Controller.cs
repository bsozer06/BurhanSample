using BurhanSample.DAL.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BurhanSample.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/{v:apiversion}/Companies")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly IRepositoryCollection _repository;
        public CompaniesV2Controller(IRepositoryCollection repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// Gets the list of all companies
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Manager")]          /// Role-based authorization
        [HttpGet(Name = "GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false);
            return Ok(companies);
        }
    }
}
