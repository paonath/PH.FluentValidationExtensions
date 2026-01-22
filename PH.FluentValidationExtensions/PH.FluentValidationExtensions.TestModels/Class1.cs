#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PH.FluentValidationExtensions.Abstractions.StringSanitizer;

#endregion

namespace PH.FluentValidationExtensions.TestModels
{
    /// <summary>
    /// 
    /// </summary>
    public class SampleSkippingAttributeClassToValidate : AbsClassToValidate
    {
        /// <summary>
        /// 
        /// </summary>
        [DisableAvoidSpecialCharsCheckValidation]
        [DisableScriptCheckValidation]
        public override string? StringValue { get; set; }
       

    }
    /// <summary>
    /// 
    /// </summary>
    public class AbsClassToValidate
    {
        /// <summary>
        /// 
        /// </summary>

        public virtual string? StringValue { get; set; }     
     

    }



    /// <summary>
    /// 
    /// </summary>
    public class ClassToValidate : AbsClassToValidate
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SkipValue { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class WithSkipForSpecialChar
    {
        /// <summary>
        /// 
        /// </summary>
        [DisableAvoidSpecialCharsCheckValidation]
        public string V { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisableAvoidSpecialCharsCheckValidation]
        public char[] ArrayOfChars { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisableAvoidSpecialCharsCheckValidation]
        public char[] ArrayOfCharsSkipped { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WithSkipForSpecialChar()
        {
            V                   = String.Empty;
            ArrayOfChars        = Array.Empty<char>();
            ArrayOfCharsSkipped = Array.Empty<char>();
        }

    }

    /// <summary>
    ///     Sample Data
    /// </summary>
    public class WithArray
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public WithArray()
        {
            
            ArrayOfChars = Array.Empty<char>();
           
            ArrayOfNullableChars = Array.Empty<char?>();
            ArrayOffNullableStrings = Array.Empty<string?>();

            ArrayOfStrings = Array.Empty<string>();
        }

        /// <summary>
        ///     Sample Data
        /// </summary>
        public char[] ArrayOfChars { get; set; }


        /// <summary>
        ///     Sample Data
        /// </summary>
        public string[] ArrayOfStrings { get; set; }

       
        /// <summary>
        ///     Gets or sets an array of nullable characters.
        /// </summary>
        /// <remarks>
        ///     This property can hold an array of nullable <see cref="char" /> values.
        ///     It is initialized to an empty array by default.
        /// </remarks>
        public char?[] ArrayOfNullableChars { get; set; }


        

        /// <summary>
        ///     Sample Data
        /// </summary>
        public string?[] ArrayOffNullableStrings { get; set; }
        
     
    }

    
    
   
    /// <summary>
    ///     Sample Data
    /// </summary>
    public record SampleRecord(string Value, int? Number);
    
   
    /// <summary>
    ///     Sample Data
    /// </summary>
    public class Sample
    {
       
        
        
        /// <summary>
        ///     Sample Data
        /// </summary>
        public string? StringValue { get; set; }
        
       
        
        
      


        /// <summary>
        /// 
        /// </summary>
        public string? AlwaysNullString { get; set; }

     
    }


    /// <summary>
    ///     Sample Data
    /// </summary>
    public class WithNested
    {
        
        /// <summary>
        ///     Sample Data
        /// </summary>
        public int? InvValue { get; set; }

       

          /// <summary>
        ///     Sample Data
        /// </summary>
        public string? StringValue { get; set; }

        /// <summary>
        ///     Sample Data
        /// </summary>
        public Sample? Nested { get; }

        private WithNested(Sample? n)
        {
            Nested = n;
        }

        /// <summary>
        ///     Sample Data
        /// </summary>
        public static WithNested Init(string? s, Sample? sample) => new(sample)
        {
            StringValue =
                s,
            InvValue = 0
        };
        
      

    }
    
    
}