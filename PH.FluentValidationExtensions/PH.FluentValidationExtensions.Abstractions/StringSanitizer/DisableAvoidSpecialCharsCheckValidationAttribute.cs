using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH.FluentValidationExtensions.Abstractions.StringSanitizer
{
    /// <summary>
    /// Specifies that the associated field or property allows special characters. 
    /// This attribute is used to indicate that special character checks are disabled 
    /// for the target member in the FluentValidationSanitizer framework.
    /// <para>If omitted, base validators will enforce restrictions on special characters.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DisableAvoidSpecialCharsCheckValidationAttribute : FluentValidationExtensionsAbstractionAttribute
    {
    }
}