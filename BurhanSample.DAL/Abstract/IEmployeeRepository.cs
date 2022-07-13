using BurhanSample.Core.Utilities.Models.RequestFeatures;
using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.RequestFeatures;
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
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
