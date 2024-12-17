using System;

namespace PH.FluentValidationExtensions.Abstractions.StringSanitizer
{
    /// <summary>
    /// Specifies that the associated field or property allows script content. 
    /// This attribute is used to indicate that script content is permitted 
    /// for the target member in the FluentValidationSanitizer framework.
    /// <para>If omitted Base Validators search for javascript/html Tag SCRIPT on code</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DisableScriptCheckValidationAttribute : FluentValidationExtensionsAbstractionAttribute
    {
    }
}