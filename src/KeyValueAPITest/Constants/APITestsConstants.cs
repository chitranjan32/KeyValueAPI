using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyValueAPI.UnitTests.Constants
{
    /// <summary>
    /// Constants 
    /// </summary>
    public class APITestsConstants
    {
        public const string ACCEPTABLE_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const string KEY_NOT_IN_URL_FORMAT_ERROR =
            "Keys must be url-safe (only alphanumeric, hyphen, period, underscore, tilde are allowed)";
        public const string KEY_LENGTH_EXCEEDED_ERROR = "Key length exceeded 32 characters";
        public const string VALUE_LENGTH_ERROR = "Length of Value must be between 1-1024 characcters";
    }
}
