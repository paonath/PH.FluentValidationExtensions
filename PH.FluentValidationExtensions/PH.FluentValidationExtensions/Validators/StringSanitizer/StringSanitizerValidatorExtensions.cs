using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace PH.FluentValidationExtensions.Validators.StringSanitizer
{
    public static partial class StringSanitizerValidatorExtensions
    {


        /// <summary>
        /// Adds a validation rule to ensure that the string property does not contain any script-related content.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validation rule is defined.</param>
        /// <returns>An instance of <see cref="IRuleBuilderOptions{T, TProperty}"/> to allow further configuration of the rule.</returns>
        /// <remarks>
        /// This method uses the <see cref="NotContainsScriptValidator{T}"/> to perform the validation.
        /// </remarks>
        public static IRuleBuilderOptions<T, string?> WithNoScripts<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new NotContainsScriptValidator<T>());
        }

        /// <summary>
        /// Determines whether the specified string contains a script tag.
        /// </summary>
        /// <param name="value">The string to check for the presence of a script tag.</param>
        /// <returns>
        /// <c>true</c> if the string contains a script tag; otherwise, <c>false</c>.
        /// </returns>
        internal static bool ContainsScriptTag(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            bool start = value.Contains("<script", StringComparison.InvariantCultureIgnoreCase);
            if (start)
            {
                return true;
            }

            bool end = value.Contains("script>", StringComparison.CurrentCultureIgnoreCase);
            if (end)
            {
                return true;
            }
            
            bool endTag = value.Contains("/script>", StringComparison.CurrentCultureIgnoreCase);
            if (endTag)
            {
                return true;
            }

            return false;
        }
    }
}