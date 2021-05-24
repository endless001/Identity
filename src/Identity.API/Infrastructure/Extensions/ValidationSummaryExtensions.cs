using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class ValidationSummaryExtensions
    {
        public static IHtmlContent CleanValidationSummary(this IHtmlHelper htmlHelper, bool excludePropertyErrors, string message = null)
        {
            if (htmlHelper == null) throw new ArgumentNullException(nameof(htmlHelper));

            IHtmlContent validationSummary = null;
            if (htmlHelper.ViewData.ModelState.Count == 0) return validationSummary;
            var htmlAttributes = new { @class = "alert alert-danger" };
            validationSummary = htmlHelper.ValidationSummary(excludePropertyErrors, message, htmlAttributes);

            return validationSummary;
        }
    }
}
