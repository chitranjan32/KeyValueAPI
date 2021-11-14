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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace KeyValueAPITest
{
    /// <summary>
    /// Unit tests to Verify DictionaryController logic 
    /// </summary>
    public class DictionaryControllerTest
    {

        /// <summary>
        /// Test to verify OK response from GET method when a valid key is passed 
        /// </summary>
        [Fact]
        public void Test_GET_OK()
        {
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            mockRepo.Setup(repo => repo.GetItem(It.IsAny<string>())).Returns(GetTestDictionaryItem());
            var logger = new Mock<ILogger<DictionaryController>>();
            var controller = new DictionaryController(mockRepo.Object, logger.Object);

            var result = controller.Get("key");

            var actionResult = Assert.IsType<ActionResult<string>>(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }


        /// <summary>
        /// Test to verify valid "Value" returned from GET method for a given valid key
        /// </summary>
        [Fact]
        public void Test_GET_ValidKeyValue()
        {
            var key = "key1";
            var value = "value1";
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            mockRepo.Setup(repo => repo.GetItem(It.IsAny<string>())).Returns(GetTestDictionaryItem());
            var logger = new Mock<ILogger<DictionaryController>>();
            var controller = new DictionaryController(mockRepo.Object, logger.Object);

            var result = controller.Get(key);

            var actionResult = Assert.IsType<ActionResult<string>>(result);
            var actionValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(value, actionValue.Value);
        }


        /// <summary>
        /// Test to verify NOT FOUND response from GET method when an Invalid key(Not Present in Db) is passed 
        /// </summary>
        [Fact]        
        public void Test_GET_NotFound()
        {
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            mockRepo.Setup(repo => repo.GetItem(It.IsAny<string>())).Returns(()=>null);
            var logger = new Mock<ILogger<DictionaryController>>();
            var controller = new DictionaryController(mockRepo.Object, logger.Object);

            var result = controller.Get("key");

            var actionResult = Assert.IsType<ActionResult<string>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }


        /// <summary>
        /// Test to verify Successful Creation of Key/Value pair using POST  
        /// </summary>
        [Fact]
        public void Test_POST_AddKeySucess()
        {
            var key = "key1";
            var value = "value1";
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            mockRepo.Setup(repo => repo.CreateItem(It.IsAny<DictionaryItem>())).Verifiable();
            var logger = new Mock<ILogger<DictionaryController>>();
            var controller = new DictionaryController(mockRepo.Object, logger.Object);

            var result = controller.Post(key,value);
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(DictionaryController.Get), actionResult.ActionName);            
        }


        /// <summary>
        /// Test to verify "BAD REQUEST" response from POST method when trying to Insert a duplicate Key
        /// </summary>
        [Fact]
        public void Test_POST_AddDuplicateKey()
        {
            var key = "key1";
            var value = "value1";
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            mockRepo.Setup(repo => repo.GetItem(It.IsAny<string>())).Returns(GetTestDictionaryItem());
            var logger = new Mock<ILogger<DictionaryController>>();
            var controller = new DictionaryController(mockRepo.Object, logger.Object);

            var result= controller.Post(key, value);
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Item with same Key value exists",actionResult.Value.ToString());
        }


        /// <summary>
        /// Test to verify Successful Updation of Key/Value pair using PUT  
        /// </summary>
        [Fact]
        public void Test_PUT_AddKeySucess()
        {
            var key = "key1";
            var value = "value2";
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            mockRepo.Setup(repo => repo.GetItem(It.IsAny<string>())).Returns(GetTestDictionaryItem());
            var logger = new Mock<ILogger<DictionaryController>>();
            var controller = new DictionaryController(mockRepo.Object, logger.Object);

            var result = controller.Put(key, value);
            var actionResult = Assert.IsType<OkResult>(result);           
        }


        /// <summary>
        /// Test to verify "NOTFOUND REQUEST" response from PUT method when trying to update a non existent  Key
        /// </summary>
        [Fact]
        public void Test_PUT_KeyNotPresent()
        {
            var key = "key1";
            var value = "value2";
            var mockRepo = new Mock<IRepository<DictionaryItem>>();
            mockRepo.Setup(repo => repo.GetItem(It.IsAny<string>())).Returns(() => null);
            var logger = new Mock<ILogger<DictionaryController>>();
            var controller = new DictionaryController(mockRepo.Object, logger.Object);

            var result = controller.Put(key, value);
            var actionResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Key is not present.", actionResult.Value.ToString());

        }

        /// <summary>
        /// Method to set up test data 
        /// </summary>
        /// <returns></returns>
        private static DictionaryItem GetTestDictionaryItem()
        {
            return new DictionaryItem { ItemKey = "key1", ItemValue = "value1" };
        }

    }
}
