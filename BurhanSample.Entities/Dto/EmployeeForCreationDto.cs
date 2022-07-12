using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Entities.Dto
{
    // We don’t have the CompanyId, because we accept that parameter through the route !
    // such as  [Route("api/companies/{companyId}/employees")] 
    public class EmployeeForCreationDto
    {
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }


        [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
        public int Age { get; set; }


        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")] 
        public string Position { get; set; }
    }
}
