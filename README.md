# PH.FluentValidationExtensions
My Extensions to Fluent Validation

## String Sanitizer

### `WithNoScripts`

It is a simple extension of the validators to check the presence of a `<stript> ` tag within a string
```csharp
public class SampleValidator : AbstractValidator<Sample>
{
    public SampleValidator()
    {
        RuleFor(x => x.StringValue).WithNoScripts();
    }
}
```

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

