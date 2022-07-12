using BurhanSample.Entities.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Business.ValidationRules.FluentValidation
{
    public class EmployeeForUpdateValidator: AbstractValidator<EmployeeForUpdateDto>
    {
        public EmployeeForUpdateValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(e => e.Age)
                .NotEmpty()
                .InclusiveBetween(18, int.MaxValue);

            RuleFor(e => e.Position)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
