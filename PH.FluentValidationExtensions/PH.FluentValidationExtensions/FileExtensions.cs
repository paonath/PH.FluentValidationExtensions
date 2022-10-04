using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using PH.FluentValidationExtensions.Validators;

namespace PH.FluentValidationExtensions
{
	public static class FileExtensions
	{
		/// <summary>
		/// Defines a 'FileInfo By File Extension' validator on the current rule builder.
		/// Validation will fail if the <see cref="FileSystemInfo.Extension">FileInfo.Extension</see> not match the list of allowed file extensions.
		/// </summary>
		/// <typeparam name="T">Type of object being validated</typeparam>
		/// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
		/// <param name="allowedFileExtensions">The list of allowed file extensions.</param>
		/// <returns></returns>
		public static IRuleBuilderOptions<T, FileInfo> FileInfoByFileExtension<T>(
			this IRuleBuilder<T, FileInfo> ruleBuilder,
			IEnumerable<string> allowedFileExtensions) => ruleBuilder.SetValidator(new
				                                                                       FileInfoFileByFileExtensionValidator<
					                                                                       T>(allowedFileExtensions));
		
		


		/// <summary>
		/// Defines a 'FileInfo By File Extension' validator on the current rule builder.
		/// Validation will fail if the <see cref="FileSystemInfo.Extension">FileInfo.Extension</see> not match the list of allowed file extensions.
		/// </summary>
		/// <typeparam name="T">Type of object being validated</typeparam>
		/// <param name="ruleBuilder">The rule builder.</param>
		/// <param name="allowedFileExtensions">The allowed file extensions.</param>
		/// <returns></returns>
		public static IRuleBuilderOptions<T, FileInfo> FileInfoByFileExtension<T>(this IRuleBuilder<T, FileInfo> ruleBuilder,params string[] allowedFileExtensions)
			=> ruleBuilder.SetValidator(new FileInfoFileByFileExtensionValidator<T>(allowedFileExtensions));

		/// <summary>
		/// Defines a 'File By File Extension' validator on the current rule builder.
		/// Validation will fail if the given string (FileName) not match the list of allowed file extensions.
		/// </summary>
		/// <typeparam name="T">Type of object being validated</typeparam>
		/// <param name="ruleBuilder">The rule builder.</param>
		/// <param name="alloweFileExtensions">The allowe file extensions.</param>
		/// <returns></returns>
		public static IRuleBuilderOptions<T, string> FileNameByFileExtension<T>(this IRuleBuilder<T, string> ruleBuilder,
		                                                                        IEnumerable<string> alloweFileExtensions)
			=> ruleBuilder.SetValidator(new FileValidatorByFileExt<T>(alloweFileExtensions));

		/// <summary>
		/// Defines a 'File By File Extension' validator on the current rule builder.
		/// Validation will fail if the given string (FileName) not match the list of allowed file extensions.
		/// </summary>
		/// <typeparam name="T">Type of object being validated</typeparam>
		/// <param name="ruleBuilder">The rule builder.</param>
		/// <param name="allowedFileExtensions">The allowed file extensions.</param>
		/// <returns></returns>
		public static IRuleBuilderOptions<T, string> FileNameByFileExtension<T>(this IRuleBuilder<T, string> ruleBuilder
		                                                                        , params string[] allowedFileExtensions)
			=> ruleBuilder.SetValidator(new FileValidatorByFileExt<T>(allowedFileExtensions));
	}
}