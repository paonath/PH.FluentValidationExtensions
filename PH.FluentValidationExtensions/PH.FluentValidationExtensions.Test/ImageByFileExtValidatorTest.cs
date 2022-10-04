using System;
using PH.FluentValidationExtensions.Validators;
using Xunit;

namespace PH.FluentValidationExtensions.Test;

public class ImageByFileExtValidatorTest
{
	[Fact]
	public void Test()
	{
		var validator = new ImageByFileExtValidatorToBeTested();

		var good = ImageSample.GoodInstance();
		var bad  = ImageSample.BadInstance();

		var isGood = validator.Validate(good);
		var isBad  = validator.Validate(bad);

		Assert.True(isGood.IsValid);

		Assert.False(isBad.IsValid);
	}

	[Fact]
	public void TestWithListOfExtensions()
	{
		var validator = new ExtendedImageByFileExtValidatorToBeTested(); //this allow only png and jpg
		var good      = ImageSample.GoodInstance();
		var bad       = new ImageSample() { FileName = "notAllowed.gif" };

		var isGood = validator.Validate(good);
		var isBad  = validator.Validate(bad);



		Assert.True(isGood.IsValid);

		Assert.False(isBad.IsValid);

	}


	[Fact]
	public async void TestAsync()
	{
		var validator = new ImageByFileExtValidatorToBeTested();

		var good = ImageSample.GoodInstance();

		var isGood = await validator.ValidateAsync(good);

		Assert.True(isGood.IsValid);
	}

	[Fact]
	public void TestException()
	{
		Exception e = null;
		try
		{
			var v = new InvalidImageByFileExtValidatorToBeTested();
		}
		catch (Exception exception)
		{
			e = exception;
		}

		Assert.NotNull(e);
	}
}