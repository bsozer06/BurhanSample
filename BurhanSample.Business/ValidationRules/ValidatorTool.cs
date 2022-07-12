using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Business.ValidationRules
{
    public static class ValidatorTool<T>
    {
        public static ValidationResult FluentValidate(IValidator validator, T entity)
        {
            var context = new ValidationContext<T>(entity);
            var result = validator.Validate(context);
            return result;
        }


    }
}
