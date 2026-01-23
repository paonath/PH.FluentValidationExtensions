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
           
            var type = value?.GetType();
           
            
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
            bool foundOnError = false;
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

                    foundOnError = AnyPropOfTypeStringWithScripsFromClass(prop, value);
                    if (foundOnError)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Determines whether the specified value is valid for the given type by checking for the presence of script
        /// tags in its string representation or its nested string properties.
        /// </summary>
        /// <remarks>This method checks for script tags in string representations of the value, including
        /// within enumerables and nested types. For nested types, all string properties are examined. Ensure that the
        /// type and value provided are compatible for accurate validation.</remarks>
        /// <param name="type">The type against which to validate the value. This can be a string type, an enumerable of characters or
        /// strings, or a nested type containing string properties.</param>
        /// <param name="value">The value to validate. The value must be compatible with the specified type and will be checked for script
        /// tags according to its type.</param>
        /// <returns>true if the value does not contain script tags according to the validation rules for its type; otherwise,
        /// false.</returns>
        private bool IsValidByType(Type? type, TProperty value)
        
        {
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
            
            if (typeof(IEnumerable<string?>).IsAssignableFrom(type) && (value is IEnumerable<string?> u1))
            {
                var r = string.Join("", u1);
                return !StringSanitizerValidatorExtensions.ContainsScriptTag(r);
            }



            if (type?.IsNested ?? false)
            {
                var any = AnyPropertiesOfTypeStringWithScrips(type, value);
                return !any;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the property specified in the validation context is marked with the
        /// DisableScriptCheckValidationAttribute, indicating that script check validation should be skipped for that
        /// property.
        /// </summary>
        /// <remarks>This method checks the property specified by the PropertyPath in the
        /// ValidationContext for the presence of the DisableScriptCheckValidationAttribute, which indicates that script
        /// check validation should be skipped for that property.</remarks>
        /// <param name="context">The validation context containing the instance to validate and the property path to check for the attribute.</param>
        /// <returns>true if the property has the DisableScriptCheckValidationAttribute applied; otherwise, false.</returns>
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

        /// <summary>
        /// Determines whether the specified property of the given object instance contains any string values that match
        /// predefined script patterns.
        /// </summary>
        /// <remarks>Returns false if the property value is null. This method is typically used to
        /// validate that string properties do not contain unwanted script content.</remarks>
        /// <typeparam name="TGenProperty">The type of the object from which the property value is retrieved.</typeparam>
        /// <param name="propertyInfo">The property metadata used to obtain and evaluate the property's value.</param>
        /// <param name="value">The object instance from which the property value is obtained.</param>
        /// <returns>true if the property contains string values that match the script patterns; otherwise, false.</returns>
        private bool AnyPropOfTypeStringWithScripsFromClass<TGenProperty>(PropertyInfo propertyInfo, TGenProperty value)
        {
            var pValue = propertyInfo.GetValue(value);
            if (null == pValue)
            {
                return false;
            }

            return AnyPropertiesOfTypeStringWithScrips(propertyInfo.PropertyType, pValue);
        }


        /// <summary>
        /// Determines whether any properties of the specified type contain string values that include script tags.
        /// </summary>
        /// <remarks>This method checks all properties of the specified type, including nested properties,
        /// to identify any that are of type string and contain script tags. It skips value types and only inspects
        /// reference types for nested properties.</remarks>
        /// <typeparam name="TGenProperty">Specifies the type of the object from which property values are retrieved.</typeparam>
        /// <param name="typeToCheck">The type to inspect for properties that may contain string values.</param>
        /// <param name="value">The instance of the specified type from which property values are obtained.</param>
        /// <returns>true if any string property contains a script tag; otherwise, false.</returns>
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

                    bool found = AnyPropOfTypeStringWithScripsFromClass(propertyInfo, value);
                    if(found)
                    {
                        return true;
                    }

                    
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