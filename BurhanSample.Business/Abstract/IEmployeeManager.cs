using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.Dto;
using BurhanSample.Entities.RequestFeatures;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Business.Abstract
{
    public interface IEmployeeManager
    {
        Task<IDataResult<IEnumerable<EmployeeDto>>> GetEmployees(Guid companyId, EmployeeParameters employeeParameters);
        Task<IDataResult<EmployeeDto>> GetEmployee(Guid companyId, Guid id);
        Task<IDataResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee);
        Task<IDataResult<EmployeeDto>> DeleteEmployeeForCompany(Guid companyId, Guid id);
        IDataResult<EmployeeDto> UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee);

    }
}
