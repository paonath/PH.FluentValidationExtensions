using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.VisualBasic;
using PH.FluentValidationExtensions.Abstractions.StringSanitizer;
using PH.FluentValidationExtensions.TestModels;
using PH.FluentValidationExtensions.Validators.StringSanitizer;
using Xunit;
using static PH.FluentValidationExtensions.Test.StringSanitizer.StringSanitizerTest;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    /// <summary>
    /// 
    /// </summary>
    public class StringSanitizerTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("A simple text with the word script within.")]
        #if NET8_0
        public void WithNoScriptValid(string? valueToTest)
        #else
        public void WithNoScriptValid(string valueToTest)
        
        #endif
        
        {

            Sample sample    = new Sample() { StringValue = valueToTest };
            var    validator = new SampleValidator();
            
            var validation = validator.Validate(sample);
            
            Assert.True(validation.IsValid);

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        [Theory]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.")]
        [InlineData("</script> within.")]
        [InlineData("/script> within.")]
        #if NET8_0
        public void WithNoScriptInvalid(string? valueToTest)
        #else
        public void WithNoScriptInvalid(string valueToTest)
        #endif
        {

            Sample sample    = new Sample() { StringValue = valueToTest };
            var    validator = new SampleValidator();

            var validation = validator.Validate(sample);

            Assert.False(validation.IsValid);
            Assert.NotEmpty(validation.Errors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        /// <param name="valid"></param>
        [Theory]
        [InlineData(null, true)]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.", false)]
        #if NET8_0
        public void TestSCriptsWithGenerics(string? valueToTest, bool valid)
        #else
        public void TestSCriptsWithGenerics(string valueToTest, bool valid)
        #endif
       
        {

            Sample sample = new Sample() { StringValue = valueToTest };

            ClassToValidate c = new ClassToValidate
            {
                StringValue = valueToTest
            };

            var validator0 = new GenericValidator<ClassToValidate>();
            var v0         = validator0.Validate(c);
            var v1 = validator0.Validate(new ClassToValidate());
            Assert.Equal(valid, v0.IsValid);
            Assert.True(v1.IsValid);
            

            var validator = new GenericValidator<Sample>();

            var validation = validator.Validate(sample);
            
            
            
            
            Assert.Equal(validation.IsValid, valid);


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        /// <param name="valid"></param>
        [Theory]
        [InlineData(null, true)]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.", false)]

        #if NET8_0
        public void TestSCriptsWithSimpleString(string? valueToTest, bool valid)
        #else
        public void TestSCriptsWithSimpleString(string valueToTest, bool valid)
        #endif
        
        {

            var validator = new GenericValidator<string>();
            #if NET8_0
            Exception? exception = null;
            #else
            Exception exception = null;
            #endif
            

            try
            {
                #pragma warning disable CS8604 // Possible null reference argument.
                var validation = validator.Validate(valueToTest);
                #pragma warning restore CS8604 // Possible null reference argument.
                Assert.Equal(validation.IsValid, valid);
            }
            catch (Exception e)
            {
                exception = e;
                
            }

            if (string.IsNullOrWhiteSpace(valueToTest))
            {
                Assert.NotNull(exception);
            }
            else
            {
                Assert.Null(exception);
            }

            


        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueToTest"></param>
        /// <param name="alwaysgood"></param>
        /// <param name="valid"></param>
        [Theory]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        [InlineData("valid", null, true)]
        [InlineData("valid", "valid", true)]
        [InlineData("not-valid<script", "not-valid", false)]
        #if NET8_0
        public void TestNestedProperties(string? valueToTest, string? alwaysgood, bool valid)
        #else
        public void TestNestedProperties(string valueToTest, string alwaysgood, bool valid)
        #endif
        
        {
            NestedValidator validator = new NestedValidator();

            
            var value0        = WithNested.Init(valueToTest, new Sample() { StringValue = valueToTest });
            var value1        = WithNested.Init(alwaysgood, new Sample() { StringValue  = valueToTest });
            var valueWithNull = WithNested.Init(valueToTest, null);

            
            var validation0 = validator.Validate(value0);
            var validation1 = validator.Validate(value1);
            var validation2 = validator.Validate(valueWithNull);
            
            Assert.Equal(validation0.IsValid, valid);
            Assert.Equal(validation1.IsValid, valid);
            Assert.Equal(validation2.IsValid, valid);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void TestArray()
        {
            var validTxt    = "valid";
            var notValidTxt = "not-valid: <script";

            WithArrayValidator validator = new WithArrayValidator();

            var valid = validator.Validate(new WithArray()
            {
                ArrayOfChars            = validTxt.ToCharArray(),
                ArrayOfStrings          = validTxt.Split(' '),
                #if NET8_0
                
                ArrayOfNullableChars = validTxt.ToCharArray().Select(x => new char?(x)).ToArray(),
                ArrayOffNullableStrings = validTxt.Split(' ').Select(x => (string?)(x)).ToArray() 
                
                #else
               
                #endif
               
            });

            var notValid = validator.Validate(new WithArray()
            {
                ArrayOfChars            = notValidTxt.ToCharArray(),
                ArrayOfStrings          = notValidTxt.Split(' '),
                #if NET8_0
                
                ArrayOfNullableChars = notValidTxt.ToCharArray().Select(x => new char?(x)).ToArray(),
                ArrayOffNullableStrings = notValidTxt.Split(' ').Select(x => (string?)(x)).ToArray()
                
                #else
               
                
                #endif
              
            });
            
            Assert.True(valid.IsValid);
            Assert.False(notValid.IsValid);
            
        }


        #if !NET48_OR_GREATER

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="isValid"></param>
        [Theory]
        [InlineData("valid",  true)]
        [InlineData("not-valid<script",  false)]
        public void TestARecord(string v, bool isValid)
        {
            var validator = new GenericValidator<SampleRecord>();

            var validation = validator.Validate(new SampleRecord(v, 0));
            
            Assert.Equal(isValid, validation.IsValid);

        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("valid", true)]
        [InlineData("not-valid<script", false)]
        public async Task TestRecordAsync(string v, bool isValid)
        {
            var validator = new GenericValidator<SampleRecord>();

            var validation = await validator.ValidateAsync(new SampleRecord(v, 0));

            Assert.Equal(isValid, validation.IsValid);

        }

        
        #endif
       
       

        internal class SampleValidator : AbstractValidator<Sample>
        {
            public SampleValidator()
            {
                RuleFor(x => x.StringValue).WithNoScripts();
            }
        }
        internal class WithArrayValidator : AbstractValidator<WithArray>
        {
            public WithArrayValidator()
            {
                RuleFor(x => x.ArrayOfChars).WithNoScripts();
                RuleFor(x => x.ArrayOfStrings).WithNoScripts();
                RuleFor(x => x.ArrayOfNullableChars).WithNoScripts();
                #if NET8_0
                RuleFor(x => x.ArrayOffNullableStrings).WithNoScripts();
                #endif
                
            }
        }
        
       
        internal class GenericValidator<T> : AbstractValidator<T>
        {
            public GenericValidator()
            {
                RuleFor(x => x).WithNoScripts();
            }
        }
        
        #if NET8_0

        internal class NestedValidator : GenericValidator<WithNested?>
        {
            public NestedValidator()
            {

            }
        }
        
        #else

        internal class NestedValidator : GenericValidator<WithNested>
        {
            public NestedValidator()
            {
                
            }
        }
        
        #endif
        
        
    }
}