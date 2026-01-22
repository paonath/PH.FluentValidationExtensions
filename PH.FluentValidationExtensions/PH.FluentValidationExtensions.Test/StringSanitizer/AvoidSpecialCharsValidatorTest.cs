using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.TestModels;
using PH.FluentValidationExtensions.Validators.StringSanitizer;
using Xunit;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    /// <summary>
    /// 
    /// </summary>
    public class AvoidSpecialCharsValidatorTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        /// <param name="allowed"></param>
        /// <param name="isValid"></param>
        [Theory]
        [InlineData("",new char[0], true)]
        [InlineData("valid", new char[0], true)]
        [InlineData("valid.", new char[]{'.'}, true)]
        [InlineData("valid.", new char[]{'_'}, false)]
        public void AvoidSpacialChars(string valueToTest, char[] allowed, bool isValid)
        {
            var validator = new StringValidator(allowed);
            var result    = validator.Validate(valueToTest);
            Assert.Equal(isValid, result.IsValid);


            var anotherValidator = new GenericValidator<WithNested>(allowed);
            var n                = WithNested.Init("alwaysGood", new Sample() { StringValue = valueToTest });


           
            var nestedResult = anotherValidator.Validate(n);
            Assert.Equal(isValid, nestedResult.IsValid);


            var nestedValidator       = new SampleModelNestedValidator(allowed);
            var stringNestedValidator = new SampleModelByStringNestedValidator(string.Join("", allowed));
            
            var s = new SampleModelNested()
            {
                StringValue = valueToTest, Nested = new SampleModel()
                {
                    StringValue = valueToTest,
                    CharsValue  = valueToTest.ToCharArray()
                }
            };
            var nestedValidation = nestedValidator.Validate(s);

            #if NET8_0

            s.Nested.NullableChars        = s.Nested.CharsValue;
            s.Nested.NullableStringsValue = s.Nested.CharsValue.Select(x => $"{x}").ToArray();
            
            var sValidation = nestedValidator.Validate(s);
            Assert.Equal(isValid, sValidation.IsValid);
            
            #endif

            var sv = stringNestedValidator.Validate(s);
            Assert.Equal(isValid, sv.IsValid);
            
            
            Assert.Equal(isValid, nestedValidation.IsValid);

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        /// <param name="allowed"></param>
        /// <param name="isValid"></param>
        [Theory]
        [InlineData("", new char[0], true)]
        [InlineData("valid", new char[0], true)]
        [InlineData("valid.", new char[] { '.' }, true)]
        [InlineData("valid.", new char[] { '_' }, true)]
        public void SKipDisallowValidation(string valueToTest, char[] allowed, bool isValid)
        {

            WithSkipForSpecialChar toValidate = new WithSkipForSpecialChar()
            {
                ArrayOfChars        = valueToTest.ToCharArray(),
                ArrayOfCharsSkipped = valueToTest.ToCharArray(),
                V                   = valueToTest
            };
            

            var validator  = new GenericValidator<WithSkipForSpecialChar>(allowed);
            var validation = validator.Validate(toValidate);

            var secondValidator = new GenericValidator<SampleSkippingAttributeClassToValidate>(allowed);

            var validation2 = secondValidator.Validate(new SampleSkippingAttributeClassToValidate()
                                                           { StringValue = valueToTest });

            var nullDataValidation = validator.Validate(new WithSkipForSpecialChar());
            Assert.True(nullDataValidation.IsValid);


            SKipAnotherValidator sKipAnotherValidator = new SKipAnotherValidator(allowed);
            var validation3 =
                sKipAnotherValidator.Validate(toValidate);
            
            

            Assert.Equal(isValid, validation.IsValid);
            Assert.Equal(isValid, validation2.IsValid);
            Assert.Equal(isValid, validation3.IsValid);
        }






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
        
        
        internal class SampleModelByStringNestedValidator : AbstractValidator<SampleModelNested>
        {
            public SampleModelByStringNestedValidator(string allowed)
            {
                RuleFor(x => x.StringValue).AvoidSpecialChars(allowed);
                RuleFor(x => x.Nested).AvoidSpecialChars(allowed);
            }
        }
        
        internal class SampleModelNestedValidator : GenericValidator<SampleModelNested>
        {
            public SampleModelNestedValidator(char[] allowed) : base(allowed)
            {
            }
        }
        
        internal class SKipAnotherValidator : AbstractValidator<WithSkipForSpecialChar>
        {
            public SKipAnotherValidator(char[] allowed)
            {
                RuleFor(x => x.ArrayOfCharsSkipped).AvoidSpecialChars(allowed);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        internal class SampleModel
        {
            /// <summary>
            /// 
            /// </summary>
            public string StringValue { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public char[] CharsValue  { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string[] StringsValue { get; set; }

            #if NET8_0

            public char[]?    NullableChars        { get; set; }
            public string?[]? NullableStringsValue { get; set; }

            
            #endif
            
            /// <summary>
            /// 
            /// </summary>
            public bool   BoolValue   { get; set; }

            public SampleModel()
            {
                StringValue = "";
                CharsValue  = Array.Empty<char>();
                StringsValue = Array.Empty<string>();

                #if NET8_0

                NullableChars        = null;
                NullableStringsValue = null;
                #endif
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        internal class SampleModelNested
        {
            /// <summary>
            /// 
            /// </summary>
            public string StringValue { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public SampleModel Nested { get; set; }


            public SampleModelNested()
            {
                Nested      = new SampleModel();
                StringValue = "";
            }
        }

    }
    
}