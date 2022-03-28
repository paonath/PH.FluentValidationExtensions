# PH.FluentValidationExtensions
My Extensions to Fluent Validation

## Image Validation

### Build Rule

```csharp
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
		// this rule allow only jpeg content type instead of builtin list
		RuleFor(c => c.ContentType).ImageFileByContentType(new string[] { "image/jpeg" });
	}
}
```
