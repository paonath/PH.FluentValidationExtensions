using PH.FluentValidationExtensions.Validators.StringSanitizer;
using Xunit.Sdk;
using FluentValidation;


namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    internal class StringValidator : AbstractValidator<string>
    {
        public StringValidator(char[] allowed)
        {
            RuleFor(x => x).AvoidSpecialChars(allowed);
        }
    }
}