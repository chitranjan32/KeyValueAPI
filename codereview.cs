using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace CodeSample
{
    public class Item_Not_FoundException : Exception
    {
    }

    //COMMENT-1
	//Interface name should follow the convention. It should be "IEntity"	
	public interface Entity
    { 
        string Key { get; set; }
    }

    public interface IRepository<T> 
        where T: Entity
    {
        Task<T> GetItem(string key);
    }

    public class MemoryRepository<T> : IDisposable, IRepository<T>
        where T : Entity
    {
        public List<T> Items { get; }

        public MemoryRepository(List<T> items)         {
            
			this.Items = new List<T>();
            
			//COMMENT -2
			//There are several other BETTER ways to assign the List "Items" with a shallow copy of values from "items" list. 
			//Avoid using foreach which is more expensive than even a "for" loop
			
			//WAY 1 
			//this.Items = new List<T>(items);
			
			//WAY 2 
			// this.Items = items.ToList();			
			foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        // COMMENT-3
		//No await operator inside this method . This will run in a synchronous manner 
		public async Task<T> GetItem(string entity)
        {
            //COMMENT-4 
			// We can't assign a null value to implicitly typed variable . This will give a compilation error
			var result = null;
            
			//COMMENT-5
			//For loop is better in performance than "Foreach". 
			// OR use LINQ: eg  var result =  Items.Where(x => x.Key == entity).FirstOrDefault();
			foreach (var item in Items) {
                if (item.Key == entity)
                {
                    //COMMENT-6
                    //Once the match has happened we should use "break" statement to avoid iterating over the whole List. 
					result = item;
                }
            }

            if (result == null) 
            {
                throw new Item_Not_FoundException();
            }
			
			//COMMENT -7 
			//No return specified when result != null.This will give a compilation error 
        }

        
		public void Dispose()          {
            
			//COMMENT -8: 
			//Cleanup of Managed resources should be done here. E.g "Items" should be assigned to "Null" here which will assign the underlying field of this property to null. 		
			
			//COMMENT-9: 
			//A better approach would be to provide a "Virtual void Dispose(bool disposing) " method and do cleanup there. 
			//This virtual method will give chance to base classes to override it and provide their cleanup tasks as well. 
			GC.SuppressFinalize(this);
        }
    }
}
