using BurhanSample.API.Helper.ModelBinder;
using BurhanSample.Business.Abstract;
using BurhanSample.Business.Filters;
using BurhanSample.Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurhanSample.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : BaseApiController
    {
        private ICompanyManager _manager;

        public CompaniesController(ICompanyManager manager)
        {
            _manager = manager;
        }


        // api/companies
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var result = await _manager.GetCompanies();
            return Ok(result);
        }


        // api/companies/3d490a70-94ce-4d15-9494-5248280c2ce3
        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var result = await _manager.GetCompany(id);
            return Ok(result);
        }


        /// <summary> 
        /// Creates a newly created company 
        /// </summary> /// 
        /// <param name="company"></param> 
        /// <returns>A newly created company</returns>  
        /// <response code="201">Returns the newly created item</response> 
        /// <response code="400">If the item is null</response>  
        /// <response code="422">If the model is invalid</response>
        [HttpPost(Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]          /// ****** Actionresult olan yerde kullanilabiliyor.
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var result = await _manager.CreateCompany(company);
            return CreatedAtRoute("CompanyById", new { id = result.Data.Id }, result);
        }


        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType=typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var result = await _manager.GetCompanyCollection(ids);
            return Ok(result);
        }


        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _manager.CreateCompanyCollection(companyCollection);
            return CreatedAtRoute("CompanyCollection",
                new { ids= string.Join(",", result.Data.Select(c => c.Id)) },
                result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _manager.DeleteCompany(id);

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _manager.UpdateCompany(id, company);

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        { 
            Response.Headers.Add("Allow", "GET, OPTIONS, POST"); 
            return Ok(); 
        }

    }
}
