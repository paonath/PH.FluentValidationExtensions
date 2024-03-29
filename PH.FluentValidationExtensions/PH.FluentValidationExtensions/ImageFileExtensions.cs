﻿using System;
using System.Collections.Generic;
using FluentValidation;
using PH.FluentValidationExtensions.Validators;

namespace PH.FluentValidationExtensions
{
    public static class ImageFileExtensions
    {

	    /// <summary>
	    /// Defines a 'Image By Content-Type' validator on the current rule builder.
	    /// Validation will fail if the property not match the list of allowed content-types.
	    /// </summary>
	    /// <typeparam name="T">Type of object being validated</typeparam>
	    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
	    /// <returns></returns>
	    public static IRuleBuilder<T, string> ImageFileByContentType<T>(this IRuleBuilder<T, string> ruleBuilder)
	    {
		    return ruleBuilder.SetValidator(new ImageByContentTypeValidator<T>());
	    }

	    /// <summary>
	    /// Defines a 'Image By Content-Type' validator on the current rule builder.
	    /// Validation will fail if the property not match the <see cref="allowedContentTypes">list of allowed content-types</see>.
	    /// </summary>
	    /// <typeparam name="T">Type of object being validated</typeparam>
	    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
	    /// <param name="allowedContentTypes">list of allowed image content-type</param>
	    /// <exception cref="System.ArgumentNullException">allowedContentTypes - Missing list of allowed Content-Types</exception>
	    /// <returns></returns>
	    public static IRuleBuilder<T, string> ImageFileByContentType<T>(this IRuleBuilder<T, string> ruleBuilder, IEnumerable<string> allowedContentTypes)
	    {
		    return ruleBuilder.SetValidator(new ImageByContentTypeValidator<T>(allowedContentTypes));
	    }

	    /// <summary>
	    /// Defines a 'Image By File Extension' validator on the current rule builder.
	    /// Validation will fail if the property not match the list of allowed file extensions.
	    /// </summary>
	    /// <typeparam name="T">Type of object being validated</typeparam>
	    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
	    /// <returns></returns>
		public static IRuleBuilder<T, string> ImageFileByFileExtension<T>(this IRuleBuilder<T, string> ruleBuilder)
	    {
		    return ruleBuilder.SetValidator(new ImageByFileExtensionValidator<T>());
	    }

	    /// <summary>
	    /// Defines a 'Image By File Extension' validator on the current rule builder.
	    /// Validation will fail if the property not match the <see cref="allowedFileExt">list of allowed content-types</see>.
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="ruleBuilder">The rule builder.</param>
	    /// <param name="allowedFileExt">The allowed file extensions.</param>
	    /// <exception cref="System.ArgumentNullException">allowedFileExt - Missing list of allowed file extensions</exception>
	    /// <returns></returns>
	    public static IRuleBuilder<T, string> ImageFileByFileExtension<T>(this IRuleBuilder<T, string> ruleBuilder,
	                                                                      IEnumerable<string> allowedFileExt)
	    {
		    return ruleBuilder.SetValidator(new ImageByFileExtensionValidator<T>(allowedFileExt));
	    }
    }
}
