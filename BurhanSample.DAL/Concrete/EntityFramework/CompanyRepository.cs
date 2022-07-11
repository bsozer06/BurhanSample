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
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context) : base(context)
        {
        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return GetAll(trackChanges).OrderBy(x => x.Name).ToList();
        }

        public Company GetCompany(Guid companyId, bool trackChanges) =>
            GetByCondition(c => c.Id == companyId, trackChanges).FirstOrDefault();

        public void CreateCompany(Company company) => Create(company);

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) => 
            GetByCondition(x => ids.Contains(x.Id), trackChanges).ToList();

    }
}
