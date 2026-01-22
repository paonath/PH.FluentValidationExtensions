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
    internal class GenericValidator<T> : AbstractValidator<T>
    {
        public GenericValidator(char[] allowed)
        {
            RuleFor(x => x).AvoidSpecialChars(allowed);
        }
    }
    internal class SampleModelNestedValidator : GenericValidator<AvoidSpecialCharsValidatorTest.SampleModelNested>
    {
        public SampleModelNestedValidator(char[] allowed) : base(allowed) { }
    }

}