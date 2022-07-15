using BurhanSample.Core.Utilities.Models.RequestFeatures;
using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using BurhanSample.DataAccess.Concrete.EntityFramework.Extensions;
using BurhanSample.Entities.Concrete;
using BurhanSample.Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BurhanSample.DAL.Concrete.EntityFramework
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            return await GetEmployeeAsync(companyId, id, trackChanges);
        }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            /// => Skip, Take bilerek burada kullanildi. 
            /// sebep: Cok kayit olan database'in performans kaybinin onune gecmek icin.
            var employees = await GetByCondition(e => e.CompanyId == companyId, trackChanges)
                         .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                         .Search(employeeParameters.SearchTerm)
                         .Sort(employeeParameters.OrderBy)
                         .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                         .Take(employeeParameters.PageSize)
                         .ToListAsync();

            var count = await GetByCondition(e => e.CompanyId == companyId, trackChanges).CountAsync();

            return PagedList<Employee>.ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize, count);
        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);

    }
}
