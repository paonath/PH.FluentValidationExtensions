#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.Validators;

#endregion

namespace PH.FluentValidationExtensions.Validators.AbstractValidator
{
    /// <summary>
    ///     Represents an abstract base class for property validators that operate on specific types and their properties.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public abstract class GenericValidatorOfProperties<T, TProperty> : PropertyValidator<T, TProperty>
    {
        #if NETSTANDARD2_0
        private readonly Dictionary<Type, PropertyInfo[]> _propertiesOfType;

        #else

        private readonly Dictionary<Type, PropertyInfo[]?> _propertiesOfType;

        #endif


        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericValidatorOfProperties{T, TProperty}" /> class.
        /// </summary>
        /// <remarks>
        ///     This constructor sets up the internal dictionary used to cache property information for specific types.
        /// </remarks>
        protected GenericValidatorOfProperties()
        {
            #if NETSTANDARD2_0
            _propertiesOfType = new Dictionary<Type, PropertyInfo[]>();

            #else
            _propertiesOfType = new Dictionary<Type, PropertyInfo[]?>();
            #endif
        }

        /// <summary>
        ///     Retrieves the properties of the specified type using reflection.
        /// </summary>
        /// <param name="sourceType">The type whose properties are to be retrieved.</param>
        /// <returns>
        ///     An array of <see cref="System.Reflection.PropertyInfo" /> representing the properties of the specified type,
        ///     or <c>null</c> if no properties are found.
        /// </returns>
        #if NETSTANDARD2_0
        protected virtual PropertyInfo[] GetProperties(Type sourceType)
        #else
        protected virtual PropertyInfo[]? GetProperties(Type sourceType)
            #endif
        {
            if (_propertiesOfType.TryGetValue(sourceType, out var properties))
            {
                return properties;
            }

            var propertiesOfType = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _propertiesOfType.Add(sourceType, propertiesOfType);
            return propertiesOfType;
        }
    }
}