#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

#endregion

namespace PH.FluentValidationExtensions.Test.Image
{
    /// <summary>
    /// </summary>
    public class ImageByFileExtValidatorToBeTested : AbstractValidator<ImageSample>
    {
        /// <summary>
        /// </summary>
        public ImageByFileExtValidatorToBeTested()
        {
            RuleFor(x => x.FileName).ImageFileByFileExtension();
        }
    }

    /// <summary>
    /// </summary>
    public class InvalidImageByFileExtValidatorToBeTested : AbstractValidator<ImageSample>
    {
        /// <summary>
        /// </summary>
        public InvalidImageByFileExtValidatorToBeTested()
        {
            RuleFor(x => x.FileName).ImageFileByFileExtension(new List<string>());
        }
    }

    /// <summary>
    /// </summary>
    public class ExtendedImageByFileExtValidatorToBeTested : AbstractValidator<ImageSample>
    {
        /// <summary>
        /// </summary>
        public ExtendedImageByFileExtValidatorToBeTested()
        {
            RuleFor(c => c.FileName).ImageFileByFileExtension(new List<string> { "jpg", "png" });
        }
    }
}