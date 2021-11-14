using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace KeyValueAPI.ActionFilters
{
    /// <summary>
    /// Class to Implement Action filter for Validating the Input Model
    /// </summary>
    public class ValidationFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var key = context.ActionArguments["key"] as string;

            
            //Validtion for length of "Key"
            _logger.LogDebug("Validating Key length");
            if (key.Length > 32)
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult()
                {
                    Content = "Key length exceeded 32 characters"
                };
                return;
            }

            
            //Validation for "Key" to be in URL format
            _logger.LogDebug("Validating Key format");
            if (!Regex.IsMatch(key, @"^[a-zA-Z0-9_\-.~]*$"))
            {
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult()
                {
                    Content = 
                    "Keys must be url-safe (only alphanumeric, hyphen, period, underscore, tilde are allowed)"
                };
                return;
            }

            
            //Validation for length of "Value"
            if (context.ActionArguments.Count == 2)
            {
                var value = context.ActionArguments["value"] as string;

                _logger.LogDebug("Validating Value Length");
                if ((value != null) && ((value.Length==0)||(value.Length > 1024)))
                {
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = new ContentResult()
                    {
                        Content = "Length of Value must be between 1-1024 characcters"
                    };
                    return;
                }
            }                     

            base.OnActionExecuting(context);
        }
    }
}
