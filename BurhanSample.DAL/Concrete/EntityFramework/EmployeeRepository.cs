using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using BurhanSample.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.DAL.Concrete.EntityFramework
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext context) : base(context)
        {
        }

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            return GetEmployee(companyId, id, trackChanges);
        }

        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
                GetByCondition(e => e.CompanyId == companyId, trackChanges).OrderBy(e => e.Name);

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId; 
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }
    }
}
