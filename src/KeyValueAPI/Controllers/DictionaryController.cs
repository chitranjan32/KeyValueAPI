using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyValueAPI.Models;
using KeyValueAPI.ActionFilters;
using KeyValueAPI.Repositories.Interface;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using System.Net;

namespace KeyValueAPI.Controllers
{
    
    /// <summary>
    /// Controller class for Dictionary Item 
    /// </summary>
    [ServiceFilter(typeof(ValidationFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IRepository<DictionaryItem> _repository;
        private readonly ILogger _logger;

        public DictionaryController(IRepository<DictionaryItem> repository,ILogger<DictionaryController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //GET: api/Dictionary/myKey
        [HttpGet("{key}")]
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult<string> Get(string key)
        {
            var itemFromDb = _repository.GetItem(key.ToLower().Trim());
            if (itemFromDb == null)
            {
                //Return "NotFound" if Key is not present in database
                _logger.LogWarning($"Key- {key} is not present");
                return NotFound("Key is not present.");
            }
            return Ok(itemFromDb.ItemValue);
        }

        //POST: api/Dictionary/myKey
        [HttpPost("{key}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Post(string key,[FromBody]string value)
        {
            var itemToInsert = new DictionaryItem 
                        { 
                            ItemKey = key.ToLower().Trim(), 
                            ItemValue = value 
                        };

            //check for existence of Key in database
            var itemFromDb = _repository.GetItem(itemToInsert.ItemKey);
            if (itemFromDb != null)
            {
                _logger.LogWarning($"Key- {key} is already present");
                //Return "BadRequest" if Key is already existing in database
                return BadRequest("Item with same Key value exists");                
            }
            _repository.CreateItem(itemToInsert);
            return CreatedAtAction(nameof(Get), new { key = key }, itemToInsert);            
        }

        //PUT: api/Dictionary/myKey
        [HttpPut("{key}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]        
        public IActionResult Put(string key, [FromBody]string value)
        {
            //check for existence of Key in database
            var itemFromDb = _repository.GetItem(key.ToLower().Trim());
            if (itemFromDb == null)
            {
                //Return "NotFound" if Key is not present in database
                _logger.LogWarning($"Key- {key} is not present");
                return NotFound("Key is not present.");
            }

            itemFromDb.ItemValue = Convert.ToString(value);
            _repository.UpdateItem(itemFromDb);
            return Ok();
        }
    }
}
