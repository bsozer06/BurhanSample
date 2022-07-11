using AutoMapper;
using BurhanSample.Business.Abstract;
using BurhanSample.Core.Services.Abstract;
using BurhanSample.DAL.Abstract;
using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.Dto;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IDataResult<IEnumerable<CompanyDto>> GetCompanyCollection(IEnumerable<Guid> ids)
        {
            if (ids == null) { 
                _logger.LogError("Parameter ids is null"); 
                return new ErrorDataResult<IEnumerable<CompanyDto>>("Parameter ids is null"); 
            }
            var companyEntities = _repository.Company.GetByIds(ids, false); 

            if (ids.Count() != companyEntities.Count()) 
            { 
                _logger.LogError("Some ids are not valid in a collection");
                return new ErrorDataResult<IEnumerable<CompanyDto>>("Some ids are not valid in a collection");
            }

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return new SuccessDataResult<IEnumerable<CompanyDto>>(companiesToReturn);
        }

        public IDataResult<IEnumerable<CompanyDto>> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection) {
            if (companyCollection == null)
            { 
                _logger.LogError("Company collection sent from client is null.");
                return new ErrorDataResult<IEnumerable<CompanyDto>>("Company collection is null"); 
            } 

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection); 
            foreach (var company in companyEntities) 
            {
                _repository.Company.CreateCompany(company); 
            } 
            _repository.Save();
            
            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities); 
            //var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id)); 
            
            return new SuccessDataResult<IEnumerable<CompanyDto>>(companyCollectionToReturn);
        }

    }
}
