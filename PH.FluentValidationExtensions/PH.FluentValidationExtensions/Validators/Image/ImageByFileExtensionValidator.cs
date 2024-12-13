#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

#endregion

namespace PH.FluentValidationExtensions.Validators.Image
{
    /// <summary>
    ///     A validator that checks if a given file extension is valid for an image file.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <remarks>
    ///     This validator ensures that the file extension of a provided string matches one of the allowed image file
    ///     extensions.
    ///     It is useful for validating image file uploads or paths.
    /// </remarks>
    /// <seealso cref="PH.FluentValidationExtensions.Validators.Image.IImageValidator" />
    public class ImageByFileExtensionValidator<T> : PropertyValidator<T, string>, IImageValidator
    {
        private readonly string[] _allowedFileExt;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageByFileExtensionValidator{T}" /> class
        ///     with the default list of allowed image file extensions.
        /// </summary>
        /// <remarks>
        ///     This constructor uses a predefined set of common image file extensions
        ///     from <see cref="ImageFileExtensionsDictionary.All" />.
        /// </remarks>
        public ImageByFileExtensionValidator() : this(ImageFileExtensionsDictionary.All)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageByFileExtensionValidator{T}" /> class.
        /// </summary>
        /// <param name="allowedFileExt">The allowed file ext.</param>
        /// <exception cref="System.ArgumentNullException">allowedFileExt - Missing list of allowed file extensions</exception>
        public ImageByFileExtensionValidator(IEnumerable<string> allowedFileExt)
        {
            if (null == allowedFileExt || allowedFileExt.LongCount() == 0)
            {
                throw new ArgumentNullException(nameof(allowedFileExt), "Missing list of allowed file extensions");
            }

            _allowedFileExt = allowedFileExt.Select(x => x.Replace(".", "")).Distinct().ToArray();
        }


        /// <summary>Validates a specific property value.</summary>
        /// <param name="context">The validation context. The parent object can be obtained from here.</param>
        /// <param name="value">The current property value to validate</param>
        /// <returns>True if valid, otherwise false.</returns>
        public override bool IsValid(ValidationContext<T> context, string value) =>
            IsValid(context, value, _allowedFileExt);


        /// <summary>
        ///     Validates whether the provided file extension is allowed for an image file.
        /// </summary>
        /// <param name="context">The validation context containing the object being validated.</param>
        /// <param name="value">The string value representing the file path or file name to validate.</param>
        /// <param name="fileExtensionsAllowed">A collection of allowed file extensions for image files.</param>
        /// <returns>
        ///     <c>true</c> if the file extension of the provided value matches one of the allowed extensions; otherwise,
        ///     <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     This method checks the file extension of the given value against the specified allowed file extensions.
        ///     It is case-insensitive and ignores the leading dot in the extensions.
        /// </remarks>
        public bool IsValid(ValidationContext<T> context, string value, IEnumerable<string> fileExtensionsAllowed)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var extPoint = 1 + value.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase);
            var ext      = value.Substring(extPoint);

            var check = fileExtensionsAllowed.Any(x => x == ext);
            return check;
        }

        /// <inheritdoc />
        public override string Name => "ImageByFileExtensionValidator";

        /// <summary>
        ///     Provides the default error message template for the validator.
        /// </summary>
        /// <param name="errorCode">The error code associated with the validation failure.</param>
        /// <returns>A string representing the default error message template.</returns>
        protected override string GetDefaultMessageTemplate(string errorCode) => "Invalid File Extension";
    }
}