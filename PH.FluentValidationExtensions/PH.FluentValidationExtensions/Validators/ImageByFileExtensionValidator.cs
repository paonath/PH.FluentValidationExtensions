using System;
using FluentValidation;
using FluentValidation.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PH.FluentValidationExtensions.Validators
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="FluentValidation.Validators.PropertyValidator&lt;T, System.String&gt;" />
	/// <seealso cref="PH.FluentValidationExtensions.Validators.IImageValidator" />
	public class ImageByFileExtensionValidator<T> : FileValidatorByFileExt<T>, IImageValidator
	{
		private readonly string[] _allowedFileExt;

		public ImageByFileExtensionValidator():this(ImageFileExtensionsDictionary.All)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageByFileExtensionValidator{T}"/> class.
		/// </summary>
		/// <param name="allowedFileExt">The allowed file ext.</param>
		/// <exception cref="System.ArgumentNullException">allowedFileExt - Missing list of allowed file extensions</exception>
		public ImageByFileExtensionValidator(IEnumerable<string> allowedFileExt):base(allowedFileExt)
		{
			
		}




		/// <inheritdoc />
		public override string Name => "ImageByFileExtensionValidator";

		protected override string GetDefaultMessageTemplate(string errorCode) => $"Invalid File Extension";
	}
}