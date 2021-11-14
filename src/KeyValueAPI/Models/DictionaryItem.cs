using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace KeyValueAPI.Models
{
    /// <summary>
    /// Class to represent a KEY/VALUE pair    
    /// </summary>
    public class DictionaryItem
    {
       [Key]                  
        public string ItemKey { get; set; }

        public string ItemValue { get; set; }
    }
}
