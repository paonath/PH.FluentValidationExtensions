using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;

namespace PH.FluentValidationExtensions.Validators
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="PH.FluentValidationExtensions.Validators.IImageValidator" />
	public class ImageByContentTypeValidator<T> : PropertyValidator<T, string>, IImageValidator
	{
		private readonly string[] _allowedContentTypes;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageByContentTypeValidator{T}"/> class.
		/// </summary>
		public ImageByContentTypeValidator() : this(ImageContentTypesDictionary.All)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageByContentTypeValidator{T}"/> class.
		/// </summary>
		/// <param name="contentTypesAllowed">The content types allowed.</param>
		/// <exception cref="System.ArgumentNullException">contentTypesAllowed - Missing list of allowed Content-Types</exception>
		public ImageByContentTypeValidator(IEnumerable<string> contentTypesAllowed)
		{
			if (null == contentTypesAllowed || contentTypesAllowed.LongCount() == 0)
			{
				throw new ArgumentNullException(nameof(contentTypesAllowed), "Missing list of allowed Content-Types");
			}

			_allowedContentTypes = contentTypesAllowed.Distinct().ToArray();
		}

		/// <summary>
		/// Returns the default error message template for this validator, when not overridden.
		/// </summary>
		/// <param name="errorCode">The currently configured error code for the validator.</param>
		/// <returns></returns>
		protected override string GetDefaultMessageTemplate(string errorCode) => $"Invalid Content-Type";


		/// <summary>Validates a specific property value.</summary>
		/// <param name="context">The validation context. The parent object can be obtained from here.</param>
		/// <param name="value">The current property value to validate</param>
		/// <returns>True if valid, otherwise false.</returns>
		public override bool IsValid(ValidationContext<T> context, string value)
		{
			return IsValid(context, value, _allowedContentTypes);
		}

		/// <summary>
		/// Returns true if <see cref="value"/> is one of the elements of the given list.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="value">The value.</param>
		/// <param name="contentTypesAllowed">The list of content types allowed.</param>
		/// <returns>
		///   <c>true</c> if the specified context is valid; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValid(ValidationContext<T> context, string value, IEnumerable<string> contentTypesAllowed)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}

			var check = contentTypesAllowed.Any(x => x == value);
			return check;
		}


		/// <inheritdoc />
		public override string Name => "ImageByContentTypeValidator";
	}
}