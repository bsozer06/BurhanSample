using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.DAL.Concrete.EntityFramework
{
    public class RepositoryCollection : IRepositoryCollection
    {
        private RepositoryContext _context;
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;

        public RepositoryCollection(RepositoryContext context)
        {
            _context = context;
        }

        // first shown -> short way
        public ICompanyRepository Company => _companyRepository ??= new CompanyRepository(_context);

        // first shown -> long way
        public IEmployeeRepository Employee
        {
            get
            {
                if (_employeeRepository == null)
                    _employeeRepository = new EmployeeRepository(_context);

                return _employeeRepository;
            }
        }


        public Task SaveAsync() => _context.SaveChangesAsync();

    }
}
