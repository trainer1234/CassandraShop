using CassandraShopWebsite.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Contracts
{
    public interface IUserRepository
    {
        void Add(User newUser);
        User Find(string id);
        User Remove(string id);
        void Update(User newUser);
        List<User> GetAll();
    }
}
