using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace KeyValueAPI.Models
{
    /// <summary>
    /// Class to capture Error details for Unhandled exceptions 
    /// This is used in Global Error Handling
    /// </summary>
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
