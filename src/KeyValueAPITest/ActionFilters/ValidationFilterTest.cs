using System;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyValueAPI.Controllers;
using KeyValueAPI.Models;
using KeyValueAPI.Repositories;
using KeyValueAPI.Repositories.Interface;
using KeyValueAPI.ActionFilters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using KeyValueAPI.UnitTests.Constants;
using KeyValueAPI.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace KeyValueAPI.UnitTests.ActionFilters
{
    /// <summary>
    /// Unit tests for ValidationFilter 
    /// </summary>
    public class ValidationFilterTest
    {
        /// <summary>
        /// Method to set up : ActionExecutingContext 
        /// </summary>
        /// <returns></returns>
        private static ActionExecutingContext SetupValidatorActionExecutingContext()
        {
            var mockControllerLogger = new Mock<ILogger<DictionaryController>>();
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            var controller = new DictionaryController(mockRepo.Object, mockControllerLogger.Object);
            var actionContext = new ActionContext()
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };
            var actionArguments = new Dictionary<string, object>
            {
                ["key"] = "123456"
            };
            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                actionArguments,
                controller
                );
        }
        

        /// <summary>
        /// Test method to verify Validation logic for "Key" length more than 32 characters 
        /// </summary>
        [Fact]        
        public void OnResultExecuting_Key_Morethan_32Characters()
        {
            var mockValidatorLogger = new Mock<ILogger<ValidationFilter>>();
            ValidationFilter validationFilter = new ValidationFilter(mockValidatorLogger.Object);
            var actionExecutingContext = SetupValidatorActionExecutingContext();

            var keyMoreThan1024Chars = RandomStringGenerator.GenerateRandomString(33);
            actionExecutingContext.ActionArguments["key"] = keyMoreThan1024Chars;
            
            validationFilter.OnActionExecuting(actionExecutingContext);

            Assert.Equal(400, actionExecutingContext.HttpContext.Response.StatusCode);
            var contentResult = Assert.IsType<ContentResult>(actionExecutingContext.Result);
            Assert.Equal(APITestsConstants.KEY_LENGTH_EXCEEDED_ERROR, contentResult.Content);
        }


        /// <summary>
        /// Test method to verify Validation logic for "Key" not in URL format 
        /// </summary>
        [Fact]
        public void OnResultExecuting_Key_NotIn_URL_Format()
        {
            var mockValidatorLogger = new Mock<ILogger<ValidationFilter>>();
            ValidationFilter validationFilter = new ValidationFilter(mockValidatorLogger.Object);
            var actionExecutingContext = SetupValidatorActionExecutingContext();

            actionExecutingContext.ActionArguments["key"] = "test@#$";

            validationFilter.OnActionExecuting(actionExecutingContext);

            Assert.Equal(400, actionExecutingContext.HttpContext.Response.StatusCode);
            var contentResult = Assert.IsType<ContentResult>(actionExecutingContext.Result);
            Assert.Equal(APITestsConstants.KEY_NOT_IN_URL_FORMAT_ERROR, contentResult.Content);
        }


        /// <summary>
        /// Test method to verify Validation logic for "Value" length more than 1024 characters 
        /// </summary>
        [Fact]
        public void OnResultExecuting_Value_Morethan_1024Characters()
        {
            var mockValidatorLogger = new Mock<ILogger<ValidationFilter>>();
            ValidationFilter validationFilter = new ValidationFilter(mockValidatorLogger.Object);
            var actionExecutingContext = SetupValidatorActionExecutingContext();

            var valueMoreThan1024Chars = RandomStringGenerator.GenerateRandomString(1025);
            actionExecutingContext.ActionArguments["value"] = valueMoreThan1024Chars;

            validationFilter.OnActionExecuting(actionExecutingContext);

            Assert.Equal(400, actionExecutingContext.HttpContext.Response.StatusCode);
            var contentResult = Assert.IsType<ContentResult>(actionExecutingContext.Result);
            Assert.Equal(APITestsConstants.VALUE_LENGTH_ERROR, contentResult.Content);
        }


        /// <summary>
        /// Test method to verify Validation logic for "Value" as Empty string
        /// </summary>
        [Fact]
        public void OnResultExecuting_Value_EMPTY_STRING()
        {
            var mockValidatorLogger = new Mock<ILogger<ValidationFilter>>();
            ValidationFilter validationFilter = new ValidationFilter(mockValidatorLogger.Object);
            var actionExecutingContext = SetupValidatorActionExecutingContext();

            actionExecutingContext.ActionArguments["value"] = string.Empty;

            validationFilter.OnActionExecuting(actionExecutingContext);

            Assert.Equal(400, actionExecutingContext.HttpContext.Response.StatusCode);
            var contentResult = Assert.IsType<ContentResult>(actionExecutingContext.Result);
            Assert.Equal(APITestsConstants.VALUE_LENGTH_ERROR, contentResult.Content);
        }

    }
}
