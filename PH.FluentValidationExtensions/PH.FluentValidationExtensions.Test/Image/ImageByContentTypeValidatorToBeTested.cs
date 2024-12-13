using System.Collections.Generic;
using FluentValidation;

namespace PH.FluentValidationExtensions.Test.Image;

public class ImageByContentTypeValidatorToBeTested : AbstractValidator<ImageSample>
{
    public ImageByContentTypeValidatorToBeTested()
    {
        RuleFor(x => x.ContentType).ImageFileByContentType();
    }
}


public class ExtendedImageByContentTypeValidatorToBeTested : AbstractValidator<ImageSample>
{
    public ExtendedImageByContentTypeValidatorToBeTested()
    {
        RuleFor(c => c.ContentType).ImageFileByContentType(["image/jpeg", "image/jpeg"]);
    }
}