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
    public class InvalidImageByContentTypeValidatorToBeTested : AbstractValidator<ImageSample>
    {
        /// <summary>
        /// </summary>
        public InvalidImageByContentTypeValidatorToBeTested()
        {
            RuleFor(x => x.ContentType).ImageFileByContentType(new List<string>());
        }
    }
}