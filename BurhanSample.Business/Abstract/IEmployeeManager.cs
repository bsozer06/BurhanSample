using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.Dto;
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
        IDataResult<IEnumerable<EmployeeDto>> GetEmployees(Guid companyId, bool trackChanges);
        IDataResult<EmployeeDto> GetEmployee(Guid companyId, Guid id, bool trackChanges);
    }
}
