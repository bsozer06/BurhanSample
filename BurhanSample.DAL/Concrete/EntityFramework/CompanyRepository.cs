using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using BurhanSample.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurhanSample.DAL.Concrete.EntityFramework
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
        {
            return await GetAll(trackChanges).OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
            await GetByCondition(c => c.Id == companyId, trackChanges).FirstOrDefaultAsync();

        public void CreateCompany(Company company) => Create(company);

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => 
            await GetByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public void DeleteCompany(Company company) => Delete(company);
        
    }
}
