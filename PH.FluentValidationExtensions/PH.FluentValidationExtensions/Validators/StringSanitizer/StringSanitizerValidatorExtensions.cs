#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;

#endregion

namespace PH.FluentValidationExtensions.Validators.StringSanitizer
{
    /// <summary>
    ///     Provides extension methods for adding string sanitization validation rules to FluentValidation validators.
    /// </summary>
    /// <remarks>
    ///     This class includes methods to validate string properties, ensuring they do not contain script-related content
    ///     that could potentially lead to security vulnerabilities such as cross-site scripting (XSS).
    /// </remarks>
    public static class StringSanitizerValidatorExtensions
    {

        #region AvoidSpecialChars

        /// <summary>
        /// Adds a validation rule to ensure that the property value does not contain special characters,
        /// except for those explicitly allowed.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="rulebuilder">The rule builder on which the validation rule is defined.</param>
        /// <param name="allowed">An array of characters that are explicitly allowed in the property value.</param>
        /// <returns>
        /// An instance of <see cref="IRuleBuilderOptions{T, TProperty}"/> to continue building the validation rule.
        /// </returns>
        /// <remarks>
        /// This method uses the <see cref="AvoidSpecialCharsValidator{T, TProperty}"/> to enforce the rule.
        /// It is particularly useful for sanitizing string properties by restricting the use of special characters,
        /// which can help prevent security vulnerabilities.
        /// </remarks>
        public static IRuleBuilderOptions<T, TProperty>
            AvoidSpecialChars<T, TProperty>(this IRuleBuilder<T, TProperty> rulebuilder, params char[] allowed) =>
            rulebuilder.SetValidator(new AvoidSpecialCharsValidator<T, TProperty>(allowed));


        /// <summary>
        /// Adds a validation rule to ensure that the string property does not contain any characters
        /// outside the specified allowed characters.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="rulebuilder">The rule builder for the property being validated.</param>
        /// <param name="charsAllowed">A string containing the characters that are allowed.</param>
        /// <returns>An instance of <see cref="IRuleBuilderOptions{T, TProperty}"/> to continue building the validation rule.</returns>
        /// <remarks>
        /// This method converts the provided string of allowed characters into a character array
        /// and applies a custom validator to ensure that the property value only contains these characters.
        /// </remarks>
        public static IRuleBuilderOptions<T, TProperty>
            AvoidSpecialChars<T, TProperty>(this IRuleBuilder<T, TProperty> rulebuilder, string charsAllowed)
        {
            char[] c = charsAllowed.ToCharArray();
            return rulebuilder.SetValidator(new AvoidSpecialCharsValidator<T, TProperty>(c));
        }



        internal static bool ContainsSpecialChar(
            #if NETSTANDARD2_0
            string v 
            #else
            string? v
            #endif
            , char[] allowed
        )
        {
            if (string.IsNullOrWhiteSpace(v))
            {
                return false;
            }


            if (allowed.Length == 0)
            {
                var regexItem = new Regex("^[a-zA-Z0-9 ]*$", RegexOptions.Compiled , TimeSpan.FromMilliseconds(500));
                return !regexItem.IsMatch(v);
            }

            var specials = v.Where(x => !char.IsLetterOrDigit(x)).ToArray();

            return specials.Except(allowed).Any();

        }

        #endregion

        #region WithNoScripts

        /// <summary>
        ///     Adds a validation rule to ensure that the property being validated does not contain script-related content.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="rulebuilder">The rule builder used to define the validation rule.</param>
        /// <returns>An instance of <see cref="IRuleBuilderOptions{T, TProperty}" /> to allow further rule chaining.</returns>
        /// <remarks>
        ///     This method applies a <see cref="NotContainsScriptValidator{T, TProperty}" /> to the property being validated.
        ///     It is designed to prevent potential security vulnerabilities, such as cross-site scripting (XSS), by ensuring
        ///     that the property value does not include script tags or similar content.
        /// </remarks>
        public static IRuleBuilderOptions<T, TProperty>
            WithNoScripts<T, TProperty>(this IRuleBuilder<T, TProperty> rulebuilder) =>
            rulebuilder.SetValidator(new NotContainsScriptValidator<T, TProperty>());

        /// <summary>
        ///     Determines whether the specified string contains a script tag.
        /// </summary>
        /// <param name="value">The string to check for the presence of a script tag.</param>
        /// <returns>
        ///     <c>true</c> if the string contains a script tag; otherwise, <c>false</c>.
        /// </returns>

        #if NETSTANDARD2_0
        internal static bool ContainsScriptTag(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var start = value.ToLowerInvariant().Contains("<script");
            if (start)
            {
                return true;
            }

            var end = value.ToLowerInvariant().Contains("script>");
            if (end)
            {
                return true;
            }


            return false;
        }
        #else
        internal static bool ContainsScriptTag(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var start = value.Contains("<script", StringComparison.InvariantCultureIgnoreCase);
            if (start)
            {
                return true;
            }

            var end = value.Contains("script>", StringComparison.CurrentCultureIgnoreCase);
            if (end)
            {
                return true;
            }


            return false;
        }

        #endif
        
        

        #endregion
    }
}