using Cassandra;
using Cassandra.Mapping;
using CassandraShopWebsite.Models.ProductAccountModels;
using CassandraShopWebsite.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Implementations
{
    public class UserByProductRepository : IUserByProductRepository
    {
        private ISession _session;

        public UserByProductRepository(ISession session)
        {
            _session = session;
        }

        public bool Add(UserByProduct newUserByProduct)
        {
            var userByProduct = Find(newUserByProduct.Product_Id);
            if (userByProduct == null)
                return false;

            var insert = new SimpleStatement("insert into user_by_product (id, product_id, product_name, user_names) values (?, ?, ?, ?)",
                      newUserByProduct.Id, newUserByProduct.Product_Id, newUserByProduct.Product_Name, newUserByProduct.User_Names);
            _session.Execute(insert);
            return true;
        }

        public UserByProduct Find(string productId)
        {
            var mapper = new Mapper(_session);
            return mapper.FirstOrDefault<UserByProduct>("select * from user_by_product where product_id = ?", Guid.Parse(productId));
        }

        public List<UserByProduct> GetAll()
        {
            var mapper = new Mapper(_session);
            return mapper.Fetch<UserByProduct>("select * from user_by_product").ToList();
        }

        public UserByProduct Remove(string productId)
        {
            var userByProduct = Find(productId);
            if (userByProduct == null)
                return null;

            var del = new SimpleStatement("delete from user_by_product where product_id = ?", userByProduct.Product_Id);
            _session.Execute(del);

            return userByProduct;
        }

        public void Update(UserByProduct newUserByProduct)
        {
            var searchUserByProduct = Find(newUserByProduct.Product_Id.ToString());
            if (searchUserByProduct != null)
            {
                var cql = new SimpleStatement("update user_by_product set user_name = ? where product_id = ?",
                                               newUserByProduct.User_Names, searchUserByProduct.Product_Id);
                _session.Execute(cql);
            }
        }
    }
}
