using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Entities.Dto
{
    // We don’t have the CompanyId, because we accept that parameter through the route !
    // such as  [Route("api/companies/{companyId}/employees")] 
    public class EmployeeForCreationDto
    {
        public string Name { get; set; }    
        public int Age { get; set; }
        public string Position { get; set; }
    }
}
