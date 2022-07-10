using BurhanSample.API.Service.Abstract;
using BurhanSample.Business.Abstract;
using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using BurhanSample.Entities.Dto;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Business.Concrete
{
    public class CompanyManager : ICompanyManager
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;


        public CompanyManager(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IDataResult<IEnumerable<CompanyDto>> GetCompanies()
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges: false);

            var companiesDto = companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                FullAddress = string.Join(' ', c.Address, c.Country)
            });

            return new SuccessDataResult<IEnumerable<CompanyDto>>(companiesDto);

        }
    }
}
