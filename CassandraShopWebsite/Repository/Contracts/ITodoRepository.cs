using CassandraShopWebsite.Models.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Contracts
{
    public interface ITodoRepository
    {
        void Add(TodoItem item);
        TodoItem Find(string id);
        TodoItem Remove(string id);
        void Update(TodoItem item);
        IEnumerable GetAll();
    }
}
