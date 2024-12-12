using FluentValidation;
using Microsoft.VisualBasic;
using PH.FluentValidationExtensions.Validators.StringSanitizer;
using Xunit;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    public class StringSanitizerTest
    {

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("A simple text with the word script within.")]
        public void WithNoScriptValid(string? valueToTest)
        {

            Sample sample    = new Sample() { StringValue = valueToTest };
            var    validator = new SampleValidator();
            
            var validation = validator.Validate(sample);
            
            Assert.True(validation.IsValid);

        }

        [Theory]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.")]
        [InlineData("</script> within.")]
        public void WithNoScriptInvalid(string? valueToTest)
        {

            Sample sample    = new Sample() { StringValue = valueToTest };
            var    validator = new SampleValidator();

            var validation = validator.Validate(sample);

            Assert.False(validation.IsValid);
            Assert.NotEmpty(validation.Errors);
        }







        internal class Sample
        {
            public string? StringValue { get; set; }
        }

        internal class SampleValidator : AbstractValidator<Sample>
        {
            public SampleValidator()
            {
                RuleFor(x => x.StringValue).WithNoScripts();
            }
        }
    }
}