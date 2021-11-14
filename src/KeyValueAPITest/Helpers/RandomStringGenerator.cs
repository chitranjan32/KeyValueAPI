using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyValueAPI.UnitTests.Constants;
using System.Threading.Tasks;

namespace KeyValueAPI.UnitTests.Helpers
{
    /// <summary>
    /// Helper class to generate Random string of a Given length
    /// </summary>
    public static class RandomStringGenerator
    {
       public static string GenerateRandomString(int length)
        {
            Random random = new();            
            return new string(Enumerable.Repeat(APITestsConstants.ACCEPTABLE_CHARS, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
