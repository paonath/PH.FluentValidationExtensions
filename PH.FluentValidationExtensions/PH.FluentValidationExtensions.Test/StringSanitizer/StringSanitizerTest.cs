using System;
using System.ComponentModel.Design;
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
        #if NET6_0
        public void WithNoScriptValid(string valueToTest)
        #else
        public void WithNoScriptValid(string? valueToTest)
        #endif
        
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
        #if NET6_0
        public void WithNoScriptInvalid(string valueToTest)
        #else
        public void WithNoScriptInvalid(string? valueToTest)
        #endif
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
        #if NET6_0
        public void TestSCriptsWithGenerics(string valueToTest, bool valid)
        #else
        public void TestSCriptsWithGenerics(string? valueToTest, bool valid)
        #endif
       
        {

            Sample sample = new Sample() { StringValue = valueToTest };

            var validator = new GenericValidator<Sample>();

            var validation = validator.Validate(sample);
            
            Assert.Equal(validation.IsValid, valid);


        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("A simple text with the word script and script example: <script type='text/javascript'></script> within.", false)]

        #if NET6_0
        public void TestSCriptsWithSimpleString(string valueToTest, bool valid)
        #else
        public void TestSCriptsWithSimpleString(string? valueToTest, bool valid)
        #endif
        
        {

            var validator = new GenericValidator<string>();
            #if NET6_0
            Exception exception = null;
            #else
            Exception? exception = null;
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

        
        
        [Theory]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        [InlineData("valid", null, true)]
        [InlineData("valid", "valid", true)]
        [InlineData("not-valid<script", "not-valid", false)]
        #if NET6_0
        public void TestNestedProperties(string valueToTest, string alwaysgood, bool valid)
        #else
        public void TestNestedProperties(string? valueToTest, string? alwaysgood, bool valid)
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
                #if NET6_0
                
                #else
                ArrayOfNullableChars = validTxt.ToCharArray().Select(x => new char?(x)).ToArray(),
                ArrayOffNullableStrings = validTxt.Split(' ').Select(x => (string?)(x)).ToArray() 
                #endif
               
            });

            var notValid = validator.Validate(new WithArray()
            {
                ArrayOfChars            = notValidTxt.ToCharArray(),
                ArrayOfStrings          = notValidTxt.Split(' '),
                #if NET6_0
                
                #else
                ArrayOfNullableChars = notValidTxt.ToCharArray().Select(x => new char?(x)).ToArray(),
                ArrayOffNullableStrings = notValidTxt.Split(' ').Select(x => (string?)(x)).ToArray()
                
                #endif
              
            });
            
            Assert.True(valid.IsValid);
            Assert.False(notValid.IsValid);
            
        }



        
        internal class Sample
        {
            #if NET6_0
            public string StringValue { get; set; }
            #else
            public string? StringValue { get; set; }
            #endif
            
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
                ArrayOfChars            = Array.Empty<Char>();
                #if NET6_0

                ArrayOfNullableChars    = Array.Empty<Char>();
                ArrayOffNullableStrings = Array.Empty<string>();                
                
                #else
                ArrayOfNullableChars = Array.Empty<Char?>();
                ArrayOffNullableStrings = Array.Empty<string?>(); 
                #endif
                
                ArrayOfStrings          = Array.Empty<string>();
               
            }

            public char[] ArrayOfChars { get; set; }

            
            
            public string[] ArrayOfStrings { get; set; }

            #if NET6_0

            public char[]   ArrayOfNullableChars    { get; set; }
            public string[] ArrayOffNullableStrings { get; set; }
            
            #else

            public char?[]   ArrayOfNullableChars    { get; set; }
            public string?[] ArrayOffNullableStrings { get; set; }

            #endif
            
           
            
            
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
            #if NET6_0
            public int    InvValue    { get; set; }
            public string StringValue { get; set; }
            public Sample Nested      { get; }

            private WithNested(Sample n)
            {
                Nested = n;
            }

            public static WithNested Init(string s, Sample sample) => new WithNested(sample) { StringValue = s, InvValue = 0 };
            
            #else


            public int?       InvValue    { get; set; }
            public string?    StringValue { get; set; }
            public Sample?    Nested      { get; }
            
            private  WithNested(Sample? n)
            {
                Nested = n;
            }

            public static WithNested Init(string? s, Sample? sample) => new WithNested(sample) { StringValue =
 s , InvValue = 0};
            
            #endif
          
            
        }
        
        internal class GenericValidator<T> : AbstractValidator<T>
        {
            public GenericValidator()
            {
                RuleFor(x => x).WithNoScripts();
            }
        }
        
        #if NET6_0

        internal class NestedValidator : GenericValidator<WithNested>
        {
            public NestedValidator()
            {

            }
        }
        
        #else

        internal class NestedValidator : GenericValidator<WithNested?>
        {
            public NestedValidator()
            {
                
            }
        }
        
        #endif
        
        
    }
}