using BurhanSample.Entities.Dto;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Business.Abstract
{
    public interface ICompanyManager
    {
        Task<IDataResult<IEnumerable<CompanyDto>>> GetCompanies();
        Task<IDataResult<CompanyDto>> GetCompany(Guid id);
        Task<IDataResult<CompanyDto>> CreateCompany(CompanyForCreationDto company);
        Task<IDataResult<IEnumerable<CompanyDto>>> GetCompanyCollection(IEnumerable<Guid> ids);
        Task<IDataResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
        Task<IDataResult<CompanyDto>> DeleteCompany(Guid id);
        Task<IDataResult<CompanyDto>> UpdateCompany(Guid id, CompanyForUpdateDto company);
    }
}
