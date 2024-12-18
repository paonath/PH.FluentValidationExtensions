#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace PH.FluentValidationExtensions.TestModels
{
    /// <summary>
    ///     Sample Data
    /// </summary>
    public class WithArray
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public WithArray()
        {
            ArrayOfChars = Array.Empty<char>();
            #if NET6_0
            ArrayOfNullableChars    = Array.Empty<char>();
            ArrayOffNullableStrings = Array.Empty<string>();

            #else
            ArrayOfNullableChars = Array.Empty<char?>();
            ArrayOffNullableStrings = Array.Empty<string?>();
            #endif

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

        #if NET6_0
        /// <summary>
        ///     Sample Data
        /// </summary>
        public char[] ArrayOfNullableChars { get; set; }

        /// <summary>
        ///     Sample Data
        /// </summary>
        public string[] ArrayOffNullableStrings { get; set; }

        #else
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

        #endif
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
        #if NET6_0
        /// <summary>
        ///     Sample Data
        /// </summary>
        public string StringValue { get; set; }
        #else
        /// <summary>
        ///     Sample Data
        /// </summary>
        public string? StringValue { get; set; }
        #endif
    }


    /// <summary>
    ///     Sample Data
    /// </summary>
    public class WithNested
    {
        #if NET6_0
        /// <summary>
        ///     Sample Data
        /// </summary>
        public int InvValue { get; set; }

        /// <summary>
        ///     Sample Data
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        ///     Sample Data
        /// </summary>
        public Sample Nested { get; }

        private WithNested(Sample n)
        {
            Nested = n;
        }

        /// <summary>
        ///     Sample Data
        /// </summary>
        public static WithNested Init(string s, Sample sample) => new(sample)
        {
            StringValue = s, InvValue
                = 0
        };

        #else
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

        #endif
    }
}