using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Xunit;

namespace PH.FluentValidationExtensions.Test
{
	public class FileValidatorTest
	{
		[Fact]
		public void FileInfoTest()
		{
			var validator = new FileInfoFileByFileExtensionValidatorToBeTested();

			var good = TarSample.GoodSample;
			var bad  = TarSample.BadSample;

			var isGood = validator.Validate(good);
			var isBad  = validator.Validate(bad);

			Assert.True(isGood.IsValid);

			Assert.False(isBad.IsValid);
		}

		[Fact]
		public void CheckStartupException()
		{
			Exception e = null;
			try
			{
				var r      = new FileInfoFileByFileExtensionValidatorToBeTestedException();
				var isGood = r.Validate(TarSample.GoodSample);
			}
			catch (Exception exception)
			{
				e = exception;
			}

			Assert.NotNull(e);
		}

		public class TarSample : ImageSample
		{
			public static TarSample GoodSample => new TarSample() { FileName = "arch.tar.gz" };
			public static TarSample BadSample  => new TarSample() { FileName = "badArchive.bad" };
		}

		class FileInfoFileByFileExtensionValidatorToBeTested : AbstractValidator<ImageSample>
		{
			public FileInfoFileByFileExtensionValidatorToBeTested()
			{
				RuleFor(x => x.FileName).NotEmpty().WithMessage("ciao");
				RuleFor(x => x.FileInfo).FileInfoByFileExtension("jpg", "tar.gz").WithMessage("check file extensions");
				RuleFor(x => x.FileName).FileNameByFileExtension(".jpg",".tar.gz").WithMessage("check file extensions");
			}

		}

		class FileInfoFileByFileExtensionValidatorToBeTestedException : AbstractValidator<ImageSample>
		{
			public FileInfoFileByFileExtensionValidatorToBeTestedException()
			{
				
				RuleFor(x => x.FileInfo).FileInfoByFileExtension().WithMessage("check file extensions");

			}

		}
	}
}