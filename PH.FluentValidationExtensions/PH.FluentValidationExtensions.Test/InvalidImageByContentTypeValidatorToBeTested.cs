using System.Collections.Generic;
using FluentValidation;

namespace PH.FluentValidationExtensions.Test;

public class InvalidImageByContentTypeValidatorToBeTested : AbstractValidator<ImageSample>
{
	public InvalidImageByContentTypeValidatorToBeTested()
	{
		RuleFor(x => x.ContentType).ImageFileByContentType(new List<string>());
	}
}