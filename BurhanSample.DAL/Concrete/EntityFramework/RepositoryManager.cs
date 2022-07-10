using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.DAL.Concrete.EntityFramework
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _context;
        private IEmployeeRepository _employeeRepository;
        private ICompanyRepository _companyRepository;

        public RepositoryManager(RepositoryContext context)
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

        public void Save() => _context.SaveChanges();

    }
}
