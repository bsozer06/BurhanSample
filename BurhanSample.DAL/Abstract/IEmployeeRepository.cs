using BurhanSample.Entities.Concrete;
using Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.DAL.Abstract
{
    public interface IEmployeeRepository: IRepositoryBase<Employee>
    {
        IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
        Employee GetEmployee(Guid companyId, Guid id, bool trackChanges);
    }
}
