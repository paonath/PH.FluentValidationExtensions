#region

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.Abstractions.StringSanitizer;
using PH.FluentValidationExtensions.Validators.AbstractValidator;

#endregion

namespace PH.FluentValidationExtensions.Validators.StringSanitizer
{
    /// <summary>
    ///     A validator that ensures a property does not contain script tags in its value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <remarks>
    ///     This validator is designed to check for the presence of script tags in string values or collections of strings.
    ///     It extends the <see cref="GenericValidatorOfProperties{T,TProperty}" /> class to provide validation logic specific
    ///     to script tag detection.
    /// </remarks>
    public class NotContainsScriptValidator<T, TProperty> : GenericValidatorOfProperties<T, TProperty>
    {
        /// <summary>Validates a specific property value.</summary>
        /// <param name="context">The validation context. The parent object can be obtained from here.</param>
        /// <param name="value">The current property value to validate</param>
        /// <returns>True if valid, otherwise false.</returns>
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            #if NETSTANDARD2_0
            Type type = value?.GetType();
            #else
            var type = value?.GetType();
            #endif

            
            if (ContainsSkip(context))
            {
                return true;
            }

            if (!IsValidByType(type, value))
            {
                return false;
            }


            var props = value?.GetType().GetProperties((BindingFlags.Public | BindingFlags.Instance));
            if (props?.Length > 0)
            {
                return IsValidByLoopingProperties(props, value);
               
            }

            return true;
        }

        private bool IsValidByLoopingProperties(PropertyInfo[] props, TProperty value)
        {
            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(string))
                {
                    #if NETSTANDARD2_0
                        var v = (string)prop.GetValue(value, null);
                    #else
                    var v = (string?)prop.GetValue(value, null);
                    #endif

                    if (StringSanitizerValidatorExtensions.ContainsScriptTag(v))
                    {
                        return false;
                    }
                }
                else
                {
                    if (prop.PropertyType.IsValueType)
                    {
                        continue;
                    }

                    return !AnyPropOfTypeStringWithScripsFromClass(prop, value);
                }
            }

            return true;
        }


        #if NETSTANDARD2_0
        private bool IsValidByType(Type type, TProperty value)
        #else
        private bool IsValidByType(Type? type, TProperty value)
        #endif
        
        {
            if (type == typeof(string))
            {
                return !StringSanitizerValidatorExtensions.ContainsScriptTag($"{value}");
            }

            #if NETSTANDARD2_0
            #else

            if (typeof(IEnumerable<char?>).IsAssignableFrom(type) && (value is IEnumerable<char?> u))
            {
                var r = string.Join("", u);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r);
            }


            #endif


            if (typeof(IEnumerable<char>).IsAssignableFrom(type) && (value is IEnumerable<char> u3))
            {
                var r = string.Join("", u3);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r);
            }
            #if NETSTANDARD2_0
            #else

            if (typeof(IEnumerable<string?>).IsAssignableFrom(type) && (value is IEnumerable<string?> u1))
            {
                var r = string.Join("", u1);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r);
            }


            #endif


            if (type?.IsNested ?? false)
            {
                var any = AnyPropertiesOfTypeStringWithScrips(type, value);
                return !any;
            }

            return true;
        }

        private static bool ContainsSkip(ValidationContext<T> context)
        {
            var skip = context.InstanceToValidate?.GetType()?.GetProperty(context.PropertyPath)
                             ?.GetCustomAttribute<DisableScriptCheckValidationAttribute>();
            if (null != skip)
            {
                return true;
            }

            return false;
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
            if (null == props || props.Length == 0)
            {
                return false;
            }

            foreach (var propertyInfo in props)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    #if NETSTANDARD2_0
                    var v = (string)propertyInfo.GetValue(value, null);
                    #else
                    var v = (string?)propertyInfo.GetValue(value, null);
                    #endif

                    if (StringSanitizerValidatorExtensions.ContainsScriptTag(v))
                    {
                        return true;
                    }
                }
                else
                {
                    return AnyPropOfTypeStringWithScripsFromClass(propertyInfo, value);
                }
            }

            return false;
        }

        /// <inheritdoc />
        public override string Name => "GenericNotContainsScriptValidator";

        /// <summary>
        ///     Provides the default error message template for the <see cref="NotContainsScriptValidator{T,TProperty}" />.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string representing the default error message template.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' contains the value 'SCRIPT' that is not allowed by the validation rules";
    }
}