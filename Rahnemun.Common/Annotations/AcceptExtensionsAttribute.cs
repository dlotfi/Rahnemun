// Based on FileExtensionsAttribute defined in Mvc3 futures.
// Also refer to http://jqueryvalidation.org/extension-method/ and http://jqueryvalidation.org/accept-method/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edreamer.Framework.Localization;
using Edreamer.Framework.Mvc.Validation;
using Edreamer.Framework.Validation;

namespace Rahnemun.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AcceptExtensionsAttribute : ValidationAttribute
    {
        public AcceptExtensionsAttribute()
            : base("The {0} field only accepts files with the following extensions: {1}")
        {
            Extensions = "png,jpg,jpeg,gif";
        }

        public string Extensions { get; set; }

        private string ExtensionsFormatted
        {
            get { return ExtensionsParsed.Aggregate((left, right) => left + ", " + right); }
        }

        private string ExtensionsNormalized
        {
            get { return Extensions.Replace(" ", "").Replace(".", "").ToLowerInvariant(); }
        }

        private IEnumerable<string> ExtensionsParsed
        {
            get { return ExtensionsNormalized.Split(',').Select(e => "." + e); }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, ExtensionsFormatted);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var valueAsFileBase = value as HttpPostedFileBase;
            if (valueAsFileBase != null)
            {
                return ValidateExtension(valueAsFileBase.FileName);
            }

            var valueAsString = value as string;
            if (valueAsString != null)
            {
                return ValidateExtension(valueAsString);
            }

            return false;
        }

        private bool ValidateExtension(string fileName)
        {
            try
            {
                return ExtensionsParsed.Contains(Path.GetExtension(fileName).ToLowerInvariant());
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public class Adapter : MvcDataAnnotationsValidatorAdapter<AcceptExtensionsAttribute>
        {
            public Adapter(ObjectMetadata metadata, AcceptExtensionsAttribute attribute, Localizer localizer)
                : base(metadata, attribute, localizer)
            {
            }

            public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
            {
                var rule = new ModelClientValidationRule
                {
                    ValidationType = "extension",
                    ErrorMessage = GetAttributeErrorMessage(metadata.GetDisplayName())
                };
                rule.ValidationParameters["extension"] = Attribute.ExtensionsNormalized;
                yield return rule;
            }
        }
    }
}
