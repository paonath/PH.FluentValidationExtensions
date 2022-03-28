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
	public class ImageByFileExtensionValidator<T> : PropertyValidator<T, string>, IImageValidator
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
		public ImageByFileExtensionValidator(IEnumerable<string> allowedFileExt)
		{
			if (null == allowedFileExt || allowedFileExt.LongCount() == 0)
			{
				throw new ArgumentNullException(nameof(allowedFileExt), "Missing list of allowed file extensions");
			}

			_allowedFileExt = allowedFileExt.Select(x => x.Replace(".", "")).Distinct().ToArray();
		}


		/// <summary>Validates a specific property value.</summary>
		/// <param name="context">The validation context. The parent object can be obtained from here.</param>
		/// <param name="value">The current property value to validate</param>
		/// <returns>True if valid, otherwise false.</returns>
		public override bool IsValid(ValidationContext<T> context, string value) =>
			IsValid(context, value, _allowedFileExt);


		public bool IsValid(ValidationContext<T> context, string value, IEnumerable<string> fileExtensionsAllowed)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}

			var extPoint = 1 + value.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase);
			var ext      = value.Substring(extPoint);

			var check = fileExtensionsAllowed.Any(x => x == ext);
			return check;
		}

		/// <inheritdoc />
		public override string Name => "ImageByFileExtensionValidator";

		protected override string GetDefaultMessageTemplate(string errorCode) => $"Invalid File Extension";
	}
}