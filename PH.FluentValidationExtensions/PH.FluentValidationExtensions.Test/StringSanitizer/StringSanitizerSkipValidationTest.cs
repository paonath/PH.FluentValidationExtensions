using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.Abstractions.StringSanitizer;
using PH.FluentValidationExtensions.Validators.StringSanitizer;
using Xunit;
using static PH.FluentValidationExtensions.Test.StringSanitizer.StringSanitizerSkipValidationTest;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    
    public class StringSanitizerSkipValidationTest
    {


        [Theory]
        [InlineData("test with no script", true)]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.", false)]
        public void SkipValidationByAttribute(string? value, bool valid)
        {

            var skipCheck = new SampleSkippingAttributeClassToValidate() { StringValue = value };

            var noSkipCheck = new ClassToValidate() { StringValue = value };

            var validator = new TestSkipValidator<AbsClassToValidate>();

            var skippedValidation = validator.Validate(skipCheck);
            
            Assert.True(skippedValidation.IsValid);
            
            var noSkipValidation = validator.Validate(noSkipCheck);
            if (valid)
            {
                Assert.True(noSkipValidation.IsValid);
            }
            else
            {
                Assert.False(noSkipValidation.IsValid);
            }



        }


        internal class AbsClassToValidate
        {
            public virtual string? StringValue { get; set; }
        }
        
        
        internal class SampleSkippingAttributeClassToValidate : AbsClassToValidate
        {
            [DisableScriptCheckValidation]
            public override string? StringValue { get; set; }
        }
        
        internal class ClassToValidate : AbsClassToValidate
        {
            
            
        }

        internal class TestSkipValidator<TClass> : AbstractValidator<TClass> where TClass : AbsClassToValidate , new()
        {
            public TestSkipValidator()
            {
                RuleFor(x => x.StringValue).WithNoScripts();
            }
        }
        
    }
    
    
    
}