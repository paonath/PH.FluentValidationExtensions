using System.Collections.Generic;
using FluentValidation;

namespace PH.FluentValidationExtensions.Test.Image
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageByContentTypeValidatorToBeTested : AbstractValidator<ImageSample>
    {/// <summary>
        /// 
        /// </summary>
        public ImageByContentTypeValidatorToBeTested()
        {
            RuleFor(x => x.ContentType).ImageFileByContentType();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExtendedImageByContentTypeValidatorToBeTested : AbstractValidator<ImageSample>
    {
        private static readonly string[] Ctc = new string []{ "image/jpeg", "image/jpeg" };
        /// <summary>
        /// 
        /// </summary>
        public ExtendedImageByContentTypeValidatorToBeTested()
        {
        
        
            RuleFor(c => c.ContentType).ImageFileByContentType(Ctc);
        }
    }
}