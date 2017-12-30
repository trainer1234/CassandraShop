using Cassandra;
using Cassandra.Mapping;
using CassandraShopWebsite.Models.ProductModels;
using CassandraShopWebsite.Models.Test;
using CassandraShopWebsite.Repository.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Implementations
{
    public class TodoRepository : ITodoRepository
    {
        private ISession _session;

        public TodoRepository(ISession session)
        {
            _session = session;
        }

        public void Add(TodoItem item)
        {
            var insert = new SimpleStatement("insert into items (id, createdat, name, notes, done) values (?, ?, ?, ?, ?)",
                                              item.Id, item.CreatedAt, item.Name, item.Notes, item.Done);
            _session.Execute(insert);
        }

        public TodoItem Find(string id)
        {
            var mapper = new Mapper(_session);
            return mapper.FirstOrDefault<TodoItem>("select * from items where id = ?", Guid.Parse(id));
        }

        public IEnumerable GetAll()
        {
            var mapper = new Mapper(_session);
            return mapper.Fetch<TodoItem>("select * from items");
        }

        public TodoItem Remove(string id)
        {
            var item = Find(id);
            if (item == null)
                return null;

            var del = new SimpleStatement("delete from items where id = ? and createdat = ?", item.Id, item.CreatedAt);
            _session.Execute(del);

            return item;
        }

        public void Update(TodoItem item)
        {
            var todoitem = Find(item.Id.ToString());
            if (todoitem != null)
            {
                var cql = new SimpleStatement("update items set name = ?, notes = ?, done = ? where id = ? and createdat = ?",
                                               item.Name, item.Notes, item.Done, item.Id, todoitem.CreatedAt);
                _session.Execute(cql);
            }
        }
    }
}
