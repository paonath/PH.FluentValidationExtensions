using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

namespace PH.FluentValidationExtensions.Validators
{
	/// <summary>
	/// Simple File validator: check only file extensions of a <see cref="string">string property</see>.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="FluentValidation.Validators.PropertyValidator&lt;T, System.String&gt;" />
	/// <seealso cref="PH.FluentValidationExtensions.Validators.IFileValidator" />
	public class FileValidatorByFileExt<T> : PropertyValidator<T, string>, IFileValidator
	{
		private readonly FileInfoFileByFileExtensionValidator<T> _validator;

		public FileValidatorByFileExt(IEnumerable<string> allowedFileExtensions)
		{
			_validator = new FileInfoFileByFileExtensionValidator<T>(allowedFileExtensions);
		}




		/// <summary>Validates a specific property value.</summary>
		/// <param name="context">The validation context. The parent object can be obtained from here.</param>
		/// <param name="value">The current property value to validate</param>
		/// <returns>True if valid, otherwise false.</returns>
		public override bool IsValid(ValidationContext<T> context, string value) =>
			_validator.IsValid(context, new FileInfo(value));


		public bool IsValid(ValidationContext<T> context, string value, IEnumerable<string> fileExtensionsAllowed) =>
			_validator.IsValid(context, new FileInfo(value), fileExtensionsAllowed);
		
		/// <inheritdoc />
		public override string Name => "FileValidatorByFileExt";

	
	}
}