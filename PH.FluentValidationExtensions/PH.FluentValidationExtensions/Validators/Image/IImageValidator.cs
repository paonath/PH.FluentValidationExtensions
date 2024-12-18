#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Validators;

#endregion

namespace PH.FluentValidationExtensions.Validators.Image
{
    /// <summary>
    ///     Represents a validator for image-related properties.
    /// </summary>
    /// <remarks>
    ///     This interface is intended to be implemented by validators that perform validation
    ///     on image properties, such as validating content types or file extensions.
    /// </remarks>
    public interface IImageValidator : IPropertyValidator
    {
    }
}