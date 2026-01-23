using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.Validators.StringSanitizer;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    internal class GenericValidator<T> : AbstractValidator<T>
    {
        public GenericValidator(char[] allowed)
        {
            RuleFor(x => x).AvoidSpecialChars(allowed);
        }
    }
}