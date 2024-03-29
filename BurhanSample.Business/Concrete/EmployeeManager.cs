﻿using AutoMapper;
using BurhanSample.Business.Abstract;
using BurhanSample.Business.ValidationRules;
using BurhanSample.Business.ValidationRules.FluentValidation;
using BurhanSample.Core.Services.Abstract;
using BurhanSample.DAL.Abstract;
using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.Dto;
using BurhanSample.Entities.RequestFeatures;
using Core.Utilities.Results;
using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BurhanSample.Core.Utilities.Models.RequestFeatures;

namespace BurhanSample.Business.Concrete
{
    public class EmployeeManager: IEmployeeManager
    {
        private readonly IRepositoryCollection _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataShaper<EmployeeDto> _dataShaper;              /// ????

        public EmployeeManager(IRepositoryCollection repository, ILoggerManager logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IDataShaper<EmployeeDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _dataShaper = dataShaper;
        }

        public async Task<IDataResult<IEnumerable<EmployeeDto>>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
                return new ErrorDataResult<IEnumerable<EmployeeDto>>("Max age can't be less than min age.");

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<IEnumerable<EmployeeDto>>("Company object is null");
            }

            var employees = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            /// add headers info about the pagination parameters
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employees.MetaData));

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new SuccessDataResult<IEnumerable<EmployeeDto>>(employeesDto);
        }

        public async Task<IDataResult<EmployeeDto>> GetEmployee(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Company object is null");
            }

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Employee object is null");
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new SuccessDataResult<EmployeeDto>(employeeDto);

        }

        public async Task<IDataResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
        {
            var validator = new EmployeeForCreationValidator();
            var validationResult = validator.Validate(employee);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (employee == null) 
            { 
                _logger.LogError("EmployeeForCreationDto object sent from client is null."); 
                return new ErrorDataResult<EmployeeDto>("EmployeeForCreationDto object is null");
            }

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null) 
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Company object is null");
            }

            var employeeEntity = _mapper.Map<Employee>(employee); 
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return new SuccessDataResult<EmployeeDto>(employeeToReturn);

        }

        public async Task<IDataResult<EmployeeDto>> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
           var company = _repository.Company.GetCompanyAsync(companyId, false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Company object is null");
            }

            var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false); 
            if (employeeForCompany == null) 
            { 
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Employee object is null");
            }

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();

            return new SuccessDataResult<EmployeeDto>();
        }

        public IDataResult<EmployeeDto> UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee)
        {
            var validator = new EmployeeForUpdateValidator();
            var validationResult = validator.Validate(employee);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            if (employee == null) {
                _logger.LogError("EmployeeForUpdateDto object sent from client is null.");
                return new ErrorDataResult<EmployeeDto>("EmployeeForUpdateDto object is null");
            }

            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false); 
            if (company == null) 
            { 
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Company object is null");
            }

            var employeeEntity = _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);
            if (employeeEntity == null) 
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Employee object is null");
            }

            _mapper.Map(employee, employeeEntity);
            _repository.SaveAsync();

            return new SuccessDataResult<EmployeeDto>();
        }

        
    }
}
