using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;

namespace PH.FluentValidationExtensions.Validators
{
	public class FileInfoFileByFileExtensionValidator<T> : PropertyValidator<T, FileInfo>
	{
		private readonly string[] _fileExtensions;
		private          string[] _runTimeValues;

		/// <summary>
		/// Initializes a new instance of the <see cref="FileValidatorByFileExt{T}"/> class.
		/// </summary>
		/// <param name="allowedFileExtensions">The allowed file extensions.</param>
		/// <exception cref="System.ArgumentNullException">allowedFileExtensions - Missing list of allowed file extensions</exception>
		public FileInfoFileByFileExtensionValidator(IEnumerable<string> allowedFileExtensions)
		{


			var l = new List<string>();
			foreach (var s in allowedFileExtensions.Distinct())
			{
				if (!string.IsNullOrWhiteSpace(s))
				{
					if (!s.StartsWith(".", StringComparison.InvariantCultureIgnoreCase))
					{
						l.Add($".{s}");
					}
					else
					{
						l.Add(s);
					}
				}
			}

			_fileExtensions = l.OrderBy(x => x).ToArray();

			if (null == allowedFileExtensions || _fileExtensions.Length == 0)
			{
				throw new ArgumentNullException(nameof(allowedFileExtensions), "Missing list of allowed file extensions");
			}

			_runTimeValues = _fileExtensions;
		}


		/// <summary>Validates a specific property value.</summary>
		/// <param name="context">The validation context. The parent object can be obtained from here.</param>
		/// <param name="value">The current property value to validate</param>
		/// <returns>True if valid, otherwise false.</returns>
		public override bool IsValid(ValidationContext<T> context, FileInfo value) =>
			IsValid(context, value, _fileExtensions);

		public bool IsValid(ValidationContext<T> context, FileInfo value, IEnumerable<string> allowedFileExtensions)
		{
			_runTimeValues = allowedFileExtensions.Distinct().OrderBy(x => x).ToArray();

			if (string.IsNullOrWhiteSpace(value.Name))
			{
				return false;
			}

			

			var check = _runTimeValues.Any(x => value.FullName.EndsWith(x , StringComparison.InvariantCultureIgnoreCase) );
			return check;
		}

		/// <inheritdoc />
		public override string Name => "FileInfoFileByFileExtensionValidator";

		/// <summary>
		/// Returns the default error message template for this validator, when not overridden.
		/// </summary>
		/// <param name="errorCode">The currently configured error code for the validator.</param>
		/// <returns></returns>
		protected override string GetDefaultMessageTemplate(string errorCode) =>
			GetDefaultMessageTemplatePrivate(errorCode);

		private string GetDefaultMessageTemplatePrivate(string errorCode)
		{
			if (string.IsNullOrWhiteSpace(errorCode))
			{
				return $"Allowed file extensions: {string.Join(", ", _runTimeValues)}";
			}

			return $"{errorCode} - Allowed file extensions: {string.Join(", ", _runTimeValues)}";
		}

		
	}
}