using System;
using FluentValidation;
using FluentValidation.Validators;

namespace PH.FluentValidationExtensions.Validators.StringSanitizer
{
    /// <summary>
    /// A custom property validator that ensures a string property does not contain script-related content.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    public class NotContainsScriptValidator<T> : PropertyValidator<T, string?>
    {
        /// <summary>Validates a specific property value.</summary>
        /// <param name="context">The validation context. The parent object can be obtained from here.</param>
        /// <param name="value">The current property value to validate</param>
        /// <returns>True if valid, otherwise false.</returns>
        public override bool IsValid(ValidationContext<T> context, string? value) =>
            !StringSanitizerValidatorExtensions.ContainsScriptTag(value);


        /// <summary>
        /// Gets the name of the validator.
        /// </summary>
        /// <remarks>
        /// This property returns the name of the <see cref="NotContainsScriptValidator{T}"/> validator, 
        /// which is used to identify the validation logic ensuring that a string does not contain script-related content.
        /// </remarks>
        public override string Name => "NotContainsScriptValidator";




        /// <summary>
        /// Provides the default error message template for the <see cref="NotContainsScriptValidator{T}"/>.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string representing the default error message template.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' contains the value 'SCRIPT' that is not allowed by the validation rules";
    }
}