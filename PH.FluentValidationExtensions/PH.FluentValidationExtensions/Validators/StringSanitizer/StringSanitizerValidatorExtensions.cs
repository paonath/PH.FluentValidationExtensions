#region

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        ///     Adds a validation rule to ensure that the string property does not contain any script-related content.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validation rule is defined.</param>
        /// <returns>An instance of <see cref="IRuleBuilderOptions{T, TProperty}" /> to allow further configuration of the rule.</returns>
        /// <remarks>
        ///     This method uses the <see cref="NotContainsScriptValidator{T}" /> to perform the validation.
        /// </remarks>
        public static IRuleBuilderOptions<T, string?> WithNoScripts<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
            ruleBuilder.SetValidator(new NotContainsScriptValidator<T>());

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

            var endTag = value.Contains("/script>", StringComparison.CurrentCultureIgnoreCase);
            if (endTag)
            {
                return true;
            }

            return false;
        }
    }
}