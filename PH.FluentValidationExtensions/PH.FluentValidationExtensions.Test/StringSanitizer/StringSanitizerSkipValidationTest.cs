using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.Abstractions.StringSanitizer;
using PH.FluentValidationExtensions.TestModels;
using PH.FluentValidationExtensions.Validators.StringSanitizer;
using Xunit;
using static PH.FluentValidationExtensions.Test.StringSanitizer.StringSanitizerSkipValidationTest;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    /// <summary>
    /// 
    /// </summary>
    public class StringSanitizerSkipValidationTest
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valid"></param>
        [Theory]
        [InlineData("test with no script", true)]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.", false)]
        #if NET8_0
        public void SkipValidationByAttribute(string? value, bool valid)
        #else
        public void SkipValidationByAttribute(string value, bool valid)
        
        
        #endif
        
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
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void TestEmptyClassSkipPropertiesValidation()
        {
            var v         = new EmptyClass();
            var validator = new EmptyClassValidator();
            
            var validation = validator.Validate(v);
            
            Assert.True(validation.IsValid);
            
        }

        
        /// <summary>
        /// 
        /// </summary>
        internal class EmptyClass
        {
            
        }
        
        internal class EmptyClassValidator : AbstractValidator<EmptyClass>
        {
            public EmptyClassValidator()
            {
                RuleFor(x => x).WithNoScripts();
            }
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