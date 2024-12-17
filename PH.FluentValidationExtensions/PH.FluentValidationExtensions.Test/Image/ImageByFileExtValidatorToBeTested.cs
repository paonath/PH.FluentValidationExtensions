using System.Collections.Generic;
using FluentValidation;

namespace PH.FluentValidationExtensions.Test.Image;

public class ImageByFileExtValidatorToBeTested : AbstractValidator<ImageSample>
{
    public ImageByFileExtValidatorToBeTested()
    {
        RuleFor(x => x.FileName).ImageFileByFileExtension();
    }
}

public class InvalidImageByFileExtValidatorToBeTested : AbstractValidator<ImageSample>
{
    public InvalidImageByFileExtValidatorToBeTested()
    {
        RuleFor(x => x.FileName).ImageFileByFileExtension(new List<string>());
    }
}

public class ExtendedImageByFileExtValidatorToBeTested : AbstractValidator<ImageSample>
{
    public ExtendedImageByFileExtValidatorToBeTested()
    {
        RuleFor(c => c.FileName).ImageFileByFileExtension(new List<string>() { "jpg", "png" });
    }
}

