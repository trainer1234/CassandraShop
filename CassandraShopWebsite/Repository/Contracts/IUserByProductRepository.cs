using CassandraShopWebsite.Models.ProductAccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Contracts
{
    public interface IUserByProductRepository
    {
        UserByProduct Find(string productId);
        bool Add(UserByProduct newUserByProduct);
        UserByProduct Remove(string id);
        void Update(UserByProduct newUserByProduct);
        List<UserByProduct> GetAll();
    }
}
