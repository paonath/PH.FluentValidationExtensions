using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PH.FluentValidationExtensions.TestModels;
using Xunit;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    /// <summary>
    /// 
    /// </summary>
    public class AvoidSpecialCharsValidatorTest2
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        /// <param name="allowed"></param>
        /// <param name="isValid"></param>
        [Theory]
        [InlineData("", new char[0], true)]                  // Empty string, no allowed chars
        [InlineData("valid", new char[0], true)]             // Valid string, no special chars
        [InlineData("valid.", new char[] { '.' }, true)]     // Valid string with allowed special char
        [InlineData("notvalid.", new char[] { '_' }, false)] // Invalid string with disallowed special char
        //[InlineData(null, new char[0], true)] // Null value
        public void AvoidSpecialChars_StringValidation(string valueToTest, char[] allowed, bool isValid)
        {
            var validator = new StringValidator(allowed);
            var result    = validator.Validate(valueToTest);
            Assert.Equal(isValid, result.IsValid);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void AvoidSpecialChars_NestedPropertiesValidation()
        {
            var allowedChars    = new char[] { '.' };
            var nestedValidator = new SampleModelNestedValidator(allowedChars);
            var validNested = new AvoidSpecialCharsValidatorTest.SampleModelNested
            {
                StringValue = "valid.",
                Nested = new AvoidSpecialCharsValidatorTest.SampleModel
                {
                    StringValue = "valid.",
                    CharsValue  = "valid.".ToCharArray()
                }
            };
            var invalidNested = new AvoidSpecialCharsValidatorTest.SampleModelNested
            {
                StringValue = "invalid!",
                Nested = new AvoidSpecialCharsValidatorTest.SampleModel
                {
                    StringValue = "invalid!",
                    CharsValue  = "invalid!".ToCharArray()
                }
            };
            var validResult   = nestedValidator.Validate(validNested);
            var invalidResult = nestedValidator.Validate(invalidNested);
            Assert.True(validResult.IsValid);
            Assert.False(invalidResult.IsValid);
        }
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void AvoidSpecialChars_SkipValidation()
        {
            var allowedChars = new char[] { '.' };
            var validator    = new GenericValidator<WithSkipForSpecialChar>(allowedChars);
            var toValidate = new WithSkipForSpecialChar
            {
                V                   = "invalid!",
                ArrayOfChars        = "invalid!".ToCharArray(),
                ArrayOfCharsSkipped = "valid.".ToCharArray()
            };
            var result = validator.Validate(toValidate);
            Assert.True(result.IsValid); // Skipped properties should not fail validation
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void AvoidSpecialChars_NullValues()
        {
            var allowedChars = Array.Empty<char>();
            var validator    = new GenericValidator<WithSkipForSpecialChar>(allowedChars);
            var toValidate   = new WithSkipForSpecialChar();
            
            var result = validator.Validate(toValidate);
            Assert.True(result.IsValid); // Null values should not fail validation
        }
    }
}