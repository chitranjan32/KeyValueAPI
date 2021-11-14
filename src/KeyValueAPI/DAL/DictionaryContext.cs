using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyValueAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyValueAPI.DAL
{
    /// <summary>
    /// Data Access Layer for InMemory Database using Entity Framework ORM
    /// </summary>
    public class DictionaryContext : DbContext
    {
        public DictionaryContext(DbContextOptions<DictionaryContext> options) 
            : base(options)
        {
        }

        public DbSet<DictionaryItem> CustomDictionary { get; set; }
    }
}
