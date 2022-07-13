using BurhanSample.Entities.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Business.ValidationRules.FluentValidation
{
    public class EmployeeForCreationValidator: AbstractValidator<EmployeeForCreationDto>
    {
        public EmployeeForCreationValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(e => e.Age)
                .NotEmpty()
                .InclusiveBetween(18, 100);

            RuleFor(e => e.Position)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
