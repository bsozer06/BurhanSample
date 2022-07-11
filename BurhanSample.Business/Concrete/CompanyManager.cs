using AutoMapper;
//using BurhanSample.API.Service.Abstract;
using BurhanSample.Business.Abstract;
using BurhanSample.Core.Services.Abstract;
using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using BurhanSample.Entities.Concrete;
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
        private readonly IMapper _mapper;


        public CompanyManager(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IDataResult<IEnumerable<CompanyDto>> GetCompanies()
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges: false);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return new SuccessDataResult<IEnumerable<CompanyDto>>(companiesDto);

        }

        public IDataResult<CompanyDto> GetCompany(Guid id)
        {
            var company = _repository.Company.GetCompany(id, false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return new ErrorDataResult<CompanyDto>("Company object is null");
            }

            var companyDto = _mapper.Map<CompanyDto>(company);

            return new SuccessDataResult<CompanyDto>(companyDto);
        }

        public IDataResult<CompanyDto> CreateCompany(CompanyForCreationDto company)
        {
            if (company == null)
            {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return new ErrorDataResult<CompanyDto>("CompanyForCreationDto object is null");
            }

            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return new SuccessDataResult<CompanyDto>(companyToReturn);
        }



    }
}
