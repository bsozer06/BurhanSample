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

        public async Task<IDataResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return new SuccessDataResult<IEnumerable<CompanyDto>>(companiesDto);

        }

        public async Task<IDataResult<CompanyDto>> GetCompany(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return new ErrorDataResult<CompanyDto>("Company object is null");
            }

            var companyDto = _mapper.Map<CompanyDto>(company);

            return new SuccessDataResult<CompanyDto>(companyDto);
        }

        public async Task<IDataResult<CompanyDto>> CreateCompany(CompanyForCreationDto company)
        {
            if (company == null)
            {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return new ErrorDataResult<CompanyDto>("CompanyForCreationDto object is null");
            }

            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return new SuccessDataResult<CompanyDto>(companyToReturn);
        }

        public async Task<IDataResult<IEnumerable<CompanyDto>>> GetCompanyCollection(IEnumerable<Guid> ids)
        {
            if (ids == null) { 
                _logger.LogError("Parameter ids is null"); 
                return new ErrorDataResult<IEnumerable<CompanyDto>>("Parameter ids is null"); 
            }
            var companyEntities = await _repository.Company.GetByIdsAsync(ids, false); 

            if (ids.Count() != companyEntities.Count()) 
            { 
                _logger.LogError("Some ids are not valid in a collection");
                return new ErrorDataResult<IEnumerable<CompanyDto>>("Some ids are not valid in a collection");
            }

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return new SuccessDataResult<IEnumerable<CompanyDto>>(companiesToReturn);
        }

        public async Task<IDataResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection) {
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
            await _repository.SaveAsync();
            
            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities); 
            
            return new SuccessDataResult<IEnumerable<CompanyDto>>(companyCollectionToReturn);
        }

        public async Task<IDataResult<CompanyDto>> DeleteCompany(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges: false); 
            if (company == null) {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return new ErrorDataResult<CompanyDto>("CompanyForCreationDto object is null");
            }

            _repository.Company.DeleteCompany(company); 
            await _repository.SaveAsync(); 

            return new SuccessDataResult<CompanyDto>();
        }

        public async Task<IDataResult<CompanyDto>> UpdateCompany(Guid id, CompanyForUpdateDto company)
        {
            if (company == null)
            {
                _logger.LogError("CompanyForUpdateDto object sent from client is null.");
                return new ErrorDataResult<CompanyDto>("CompanyForUpdateDto object is null");
            }

            var companyEntity = await _repository.Company.GetCompanyAsync(id, trackChanges: true);
            if (companyEntity == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return new ErrorDataResult<CompanyDto>("Company entity object is null");
            }

            _mapper.Map(company, companyEntity);
            await _repository.SaveAsync();

            return new SuccessDataResult<CompanyDto>();
        }
    }
}
