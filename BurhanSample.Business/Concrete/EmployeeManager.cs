﻿using AutoMapper;
using BurhanSample.Business.Abstract;
using BurhanSample.Core.Services.Abstract;
using BurhanSample.DAL.Abstract;
using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.Dto;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;

namespace BurhanSample.Business.Concrete
{
    public class EmployeeManager: IEmployeeManager
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeManager(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public IDataResult<IEnumerable<EmployeeDto>> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<IEnumerable<EmployeeDto>>("Company object is null");
            }

            var employees = _repository.Employee.GetEmployees(companyId, trackChanges: false);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new SuccessDataResult<IEnumerable<EmployeeDto>>(employeesDto);
        }

        public IDataResult<EmployeeDto> GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Company object is null");
            }

            var employee = _repository.Employee.GetEmployee(companyId, id, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Employee object is null");
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new SuccessDataResult<EmployeeDto>(employeeDto);

        }

        public IDataResult<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
        {
            if (employee == null) 
            { 
                _logger.LogError("EmployeeForCreationDto object sent from client is null."); 
                return new ErrorDataResult<EmployeeDto>("EmployeeForCreationDto object is null");
            }

            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null) 
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return new ErrorDataResult<EmployeeDto>("Company object is null");
            }

            var employeeEntity = _mapper.Map<Employee>(employee); 
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return new SuccessDataResult<EmployeeDto>(employeeToReturn);

        }

    }
}
