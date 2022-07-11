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
using System.Text;
using System.Threading.Tasks;

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

        public IDataResult<EmployeeDto> GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }

            var employee = _repository.Employee.GetEmployee(companyId, id, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return null;
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new SuccessDataResult<EmployeeDto>(employeeDto);

        }

        public IDataResult<IEnumerable<EmployeeDto>> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }

            var employees = _repository.Employee.GetEmployees(companyId, trackChanges: false);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new SuccessDataResult<IEnumerable<EmployeeDto>>(employeesDto);
        }




    }
}
