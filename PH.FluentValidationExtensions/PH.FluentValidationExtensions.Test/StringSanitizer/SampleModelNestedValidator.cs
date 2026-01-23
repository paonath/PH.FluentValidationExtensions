using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH.FluentValidationExtensions.Test.StringSanitizer
{
    internal class SampleModelNestedValidator : GenericValidator<AvoidSpecialCharsValidatorTest.SampleModelNested>
    {
        public SampleModelNestedValidator(char[] allowed) : base(allowed) { }
    }
}