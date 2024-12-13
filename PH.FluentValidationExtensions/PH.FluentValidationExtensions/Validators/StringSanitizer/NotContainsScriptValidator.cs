using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.Validators;
using PH.FluentValidationExtensions.Validators.AbstractValidator;

namespace PH.FluentValidationExtensions.Validators.StringSanitizer
{
    /// <summary>
    /// A validator that ensures a property does not contain script tags in its value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <remarks>
    /// This validator is designed to check for the presence of script tags in string values or collections of strings.
    /// It extends the <see cref="GenericValidatorOfProperties{T, TProperty}"/> class to provide validation logic specific to script tag detection.
    /// </remarks>
    public class NotContainsScriptValidator<T, TProperty> : GenericValidatorOfProperties<T, TProperty> 
    {

        
        /// <summary>Validates a specific property value.</summary>
        /// <param name="context">The validation context. The parent object can be obtained from here.</param>
        /// <param name="value">The current property value to validate</param>
        /// <returns>True if valid, otherwise false.</returns>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            
            
            Type? type = value?.GetType();
            if (null == type)
            {
                return true;
            }

            if (type == typeof(string))
            {
                return !StringSanitizerValidatorExtensions.ContainsScriptTag($"{value}"); 
            }

            if (typeof(IEnumerable<char?>).IsAssignableFrom(type) && (value is IEnumerable<char?> u))
            {
                var r = string.Join("", u);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r);
            }

            if (typeof(IEnumerable<char>).IsAssignableFrom(type) && (value is IEnumerable<char> u3))
            {
                var r = string.Join("", u3);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r);
            }
            
            
            if (typeof(IEnumerable<string>).IsAssignableFrom(type) && (value is IEnumerable<string> u2))
            {
                var r = string.Join("", u2);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r); 
            }
            
            if(typeof(IEnumerable<string?>).IsAssignableFrom(type) && (value is IEnumerable<string?> u1))
            {
                var r = string.Join("", u1);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r);
            }

            

            


            if (type.IsNested)
            {
                var any = AnyPropertiesOfTypeStringWithScrips(type, value);
                return !any;
            }

            return true;
        }


        private bool AnyPropOfTypeStringWithScripsFromClass<TGenProperty>(PropertyInfo propertyInfo, TGenProperty value)
        {
            var pValue = propertyInfo.GetValue(value);
            if (null == pValue)
            {
                return false;
            }

            return AnyPropertiesOfTypeStringWithScrips(propertyInfo.PropertyType, pValue);
            
            
        }

        private bool AnyPropertiesOfTypeStringWithScrips<TGenProperty>(Type typeToCheck, TGenProperty value)
        {
            
            

            var props = GetProperties(typeToCheck);
            if (null == props)
            {
                return false;
            }

            foreach (var propertyInfo in props)
            {
                
                if (propertyInfo.PropertyType == typeof(string))
                {
                    var v = (string?)propertyInfo.GetValue(value, null);
                    if (StringSanitizerValidatorExtensions.ContainsScriptTag(v))
                    {
                        return true;
                    }
                }
                else
                {
                    if (propertyInfo.PropertyType.IsValueType)
                    {
                        continue;
                    }

                    return AnyPropOfTypeStringWithScripsFromClass(propertyInfo,  value);
                   
                }

                
                
            }

            return false;
        }

        /// <inheritdoc />
        public override string Name => "GenericNotContainsScriptValidator";

        /// <summary>
        /// Provides the default error message template for the <see cref="NotContainsScriptValidator{T}"/>.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string representing the default error message template.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' contains the value 'SCRIPT' that is not allowed by the validation rules";
    }


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
        public override string Name => "NotContainsScriptValidatorForString";




        /// <summary>
        /// Provides the default error message template for the <see cref="NotContainsScriptValidator{T}"/>.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string representing the default error message template.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' contains the value 'SCRIPT' that is not allowed by the validation rules";
    }
}