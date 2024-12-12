using System;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.Test;
using Xunit;

namespace PH.FluentValidationExtensions.Test;

	public class ImageByContentTypeValidatorTest
	{
		[Fact]
		public void Test()
		{
			var validator = new ImageByContentTypeValidatorToBeTested();

			var good = ImageSample.GoodInstance();
			var bad  = ImageSample.BadInstance();

			var isGood = validator.Validate(good);
			var isBad  = validator.Validate(bad);

			

			Assert.True(isGood.IsValid);

			Assert.False(isBad.IsValid);
		}

		[Fact]
		public void TestWithListOfContentTypes()
		{
			var validator = new ExtendedImageByContentTypeValidatorToBeTested(); //this allow only png and jpg
			var good      = ImageSample.GoodInstance();
			var bad       = new ImageSample() { ContentType = "image/bmp" };

			var isGood = validator.Validate(good);
			var isBad  = validator.Validate(bad);



			Assert.True(isGood.IsValid);

			Assert.False(isBad.IsValid);

		}


		[Fact]
		public async Task TestAsync()
		{
			var validator = new ImageByContentTypeValidatorToBeTested();

			var good = ImageSample.GoodInstance();

			var isGood = await validator.ValidateAsync(good);

			Assert.True(isGood.IsValid);
		}

		[Fact]
		public void TestException()
		{
			Exception? e = null;
			try
			{
				var v   = new InvalidImageByContentTypeValidatorToBeTested();
                v.CascadeMode = CascadeMode.Continue;
            }
			catch (Exception exception)
			{
				e = exception;
			}

			Assert.NotNull(e);
		}
	}
