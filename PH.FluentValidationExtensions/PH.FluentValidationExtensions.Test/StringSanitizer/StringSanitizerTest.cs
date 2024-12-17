using System;
using System.Linq;
using FluentValidation;
using Microsoft.VisualBasic;
using PH.FluentValidationExtensions.Abstractions.StringSanitizer;
using PH.FluentValidationExtensions.Validators.StringSanitizer;
using Xunit;
using static PH.FluentValidationExtensions.Test.StringSanitizer.StringSanitizerTest;

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
        [InlineData("/script> within.")]
        public void WithNoScriptInvalid(string? valueToTest)
        {

            Sample sample    = new Sample() { StringValue = valueToTest };
            var    validator = new SampleValidator();

            var validation = validator.Validate(sample);

            Assert.False(validation.IsValid);
            Assert.NotEmpty(validation.Errors);
        }


        [Theory]
        [InlineData(null, true)]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.", false)]
        public void TestSCriptsWithGenerics(string? valueToTest, bool valid)
        {

            Sample sample = new Sample() { StringValue = valueToTest };

            var validator = new GenericValidator<Sample>();

            var validation = validator.Validate(sample);
            
            Assert.Equal(validation.IsValid, valid);


        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.",
                       false)]
        public void TestSCriptsWithSimpleString(string? valueToTest, bool valid)
        {

            var validator = new GenericValidator<string>();
            Exception? exception = null;

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

        
        
        [Theory]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        [InlineData("valid", null, true)]
        [InlineData("valid", "valid", true)]
        [InlineData("not-valid<script", "not-valid", false)]
        public void TestNestedProperties(string? valueToTest, string? alwaysgood, bool valid)
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
                ArrayOfNullableChars    = validTxt.ToCharArray().Select(x => new char?(x)).ToArray(),
                ArrayOffNullableStrings = validTxt.Split(' ').Select(x => (string?)(x)).ToArray()
            });

            var notValid = validator.Validate(new WithArray()
            {
                ArrayOfChars            = notValidTxt.ToCharArray(),
                ArrayOfStrings          = notValidTxt.Split(' '),
                ArrayOfNullableChars    = notValidTxt.ToCharArray().Select(x => new char?(x)).ToArray(),
                ArrayOffNullableStrings = notValidTxt.Split(' ').Select(x => (string?)(x)).ToArray()
            });
            
            Assert.True(valid.IsValid);
            Assert.False(notValid.IsValid);
            
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
        
        internal class WithArray
        {
            /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
            public WithArray()
            {
                ArrayOfChars            = [];
                ArrayOfNullableChars    =[];
                ArrayOfStrings          = [];
                ArrayOffNullableStrings = [];
            }

            public char[] ArrayOfChars { get; set; }

            public char?[] ArrayOfNullableChars { get; set; }
            
            public string[] ArrayOfStrings { get; set; }
            
            public string?[] ArrayOffNullableStrings { get; set; }
            
            
        }
        
        internal class WithArrayValidator : AbstractValidator<WithArray>
        {
            public WithArrayValidator()
            {
                RuleFor(x => x.ArrayOfChars).WithNoScripts();
                RuleFor(x => x.ArrayOfStrings).WithNoScripts();
                RuleFor(x => x.ArrayOfNullableChars).WithNoScripts();
                RuleFor(x => x.ArrayOffNullableStrings).WithNoScripts();
            }
        }
        
        internal class WithNested
        {
            
            public int?       InvValue    { get; set; }
            public string?    StringValue { get; set; }
            public Sample?    Nested      { get; }
            
            private  WithNested(Sample? n)
            {
                Nested = n;
            }

            public static WithNested Init(string? s, Sample? sample) => new WithNested(sample) { StringValue = s , InvValue = 0};
            
        }
        
        internal class GenericValidator<T> : AbstractValidator<T>
        {
            public GenericValidator()
            {
                RuleFor(x => x).WithNoScripts();
            }
        }
        
        internal class NestedValidator : GenericValidator<WithNested?>
        {
            public NestedValidator()
            {
                
            }
        }
    }
}