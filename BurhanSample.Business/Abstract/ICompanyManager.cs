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
        IDataResult<IEnumerable<CompanyDto>> GetCompanies();
        IDataResult<CompanyDto> GetCompany(Guid id);
    }
}
