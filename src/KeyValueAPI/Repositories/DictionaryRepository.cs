using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyValueAPI.Models;
using KeyValueAPI.DAL;
using KeyValueAPI.Repositories.Interface;

namespace KeyValueAPI.Repositories
{
    /// <summary>
    /// Implemntation of IRepository contract for DictionaryItem  
    /// </summary>
    public class DictionaryRepository : IRepository<DictionaryItem>
    {
        private readonly DictionaryContext _dbContext;

        public DictionaryRepository(DictionaryContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Method to create Dictionary Item
        /// </summary>
        /// <param name="item">Dictionary Item</param>
        public void CreateItem(DictionaryItem item)
        {
            _dbContext.CustomDictionary.Add(item);
            _dbContext.SaveChanges();           
        }

        /// <summary>
        /// Method to Fetch Dictinory Item for a given Key
        /// </summary>
        /// <param name="key">Dictionary Item Key</param>
        /// <returns>Dictionary Item</returns>
        public DictionaryItem GetItem(string key)
        {
            return _dbContext.CustomDictionary.Find(key);
        }

        /// <summary>
        /// Method to update Dictionary Item
        /// </summary>
        /// <param name="item">Dictionary Item</param>
        public void UpdateItem(DictionaryItem item)
        {
            _dbContext.CustomDictionary.Update(item);
            _dbContext.SaveChanges();            
        }
    }
}
