using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.Abstractions.StringSanitizer;
using PH.FluentValidationExtensions.Validators.AbstractValidator;

namespace PH.FluentValidationExtensions.Validators.StringSanitizer
{
    /// <summary>
    /// Represents a validator that ensures a property value does not contain special characters,
    /// except for those explicitly allowed.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <remarks>
    /// This validator is designed to sanitize string properties by restricting the use of special characters.
    /// It allows customization of permitted characters through the constructor.
    /// </remarks>
    public class AvoidSpecialCharsValidator<T, TProperty> : GenericValidatorOfProperties<T, TProperty>
    {
        private readonly char[] _alloewdChars;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericValidatorOfProperties{T, TProperty}" /> class.
        /// </summary>
        /// <remarks>
        ///     This constructor sets up the internal dictionary used to cache property information for specific types.
        /// </remarks>
        public AvoidSpecialCharsValidator(char[] alloewdChars)
        {
            _alloewdChars = alloewdChars;
        }

        private static bool ContainsSkip(ValidationContext<T> context)
        {
            var skip = context.InstanceToValidate?.GetType()?.GetProperty(context.PropertyPath)
                             ?.GetCustomAttribute<DisableAvoidSpecialCharsCheckValidationAttribute>();
            if (null != skip)
            {
                return true;
            }

            return false;
        }

        #if NETSTANDARD2_0
        private bool IsValidByType(Type type, TProperty value)
        #else
        private bool IsValidByType(Type? type, TProperty value)
            #endif

        {
            if (type == typeof(string))
            {
                return !StringSanitizerValidatorExtensions.ContainsSpecialChar($"{value}", _alloewdChars);
            }

            #if NETSTANDARD2_0
            #else

            if (typeof(IEnumerable<char?>).IsAssignableFrom(type) && (value is IEnumerable<char?> u))
            {
                var r = string.Join("", u);
                return !StringSanitizerValidatorExtensions.ContainsSpecialChar(r, _alloewdChars);
            }


            #endif


            if (typeof(IEnumerable<char>).IsAssignableFrom(type) && (value is IEnumerable<char> u3))
            {
                var r = string.Join("", u3);
                return !StringSanitizerValidatorExtensions.ContainsSpecialChar(r, _alloewdChars);
            }
            #if NETSTANDARD2_0
            #else

            if (typeof(IEnumerable<string?>).IsAssignableFrom(type) && (value is IEnumerable<string?> u1))
            {
                var r = string.Join("", u1);
                return !StringSanitizerValidatorExtensions.ContainsSpecialChar(r, _alloewdChars);
            }


            #endif


            if (type?.IsNested ?? false)
            {
                var any = AnyPropertiesOfTypeStringContainsSpecialChar(type, value);
                return !any;
            }

            return true;
        }

        private bool AnyPropertiesOfTypeStringContainsSpecialChar<TGenProperty>(Type typeToCheck, TGenProperty value)
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

                    if (StringSanitizerValidatorExtensions.ContainsSpecialChar(v, _alloewdChars))
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

                    return AnyPropOfTypeStringWithCharsFromClass(propertyInfo, value);
                }
            }

            return false;
        }

        private bool AnyPropOfTypeStringWithCharsFromClass<TGenProperty>(PropertyInfo propertyInfo, TGenProperty value)
        {
            var pValue = propertyInfo.GetValue(value);
            if (null == pValue)
            {
                return false;
            }

            return AnyPropertiesOfTypeStringContainsSpecialChar(propertyInfo.PropertyType, pValue);
        }


        private bool IsValidByLoopingProperties(PropertyInfo[] props, TProperty value)
        {
            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<DisableAvoidSpecialCharsCheckValidationAttribute>() != null)
                {
                    return true;
                }
                
                if (prop.PropertyType == typeof(string))
                {
                    #if NETSTANDARD2_0
                        var v = (string)prop.GetValue(value, null);
                    #else
                    var v = (string?)prop.GetValue(value, null);
                    #endif

                    if (StringSanitizerValidatorExtensions.ContainsSpecialChar(v, _alloewdChars))
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

                    return !AnyPropOfTypeStringWithCharsFromClass(prop, value);
                }
            }

            return true;
        }
        
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


        /// <inheritdoc />
        public override string Name => "AvoidSpecialCharsValidator";

        /// <summary>
        ///     Provides the default error message template for the <see cref="NotContainsScriptValidator{T,TProperty}" />.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string representing the default error message template.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' contains special chars not explicity allowed";
    }
}