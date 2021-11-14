using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyValueAPI.Models;

namespace KeyValueAPI.Repositories.Interface
{
    /// <summary>
    /// Contract for Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> 
    {
        T GetItem(string key);
        void CreateItem(T item);
        void UpdateItem(T item);       
    }
}
