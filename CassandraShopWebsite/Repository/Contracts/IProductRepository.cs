using CassandraShopWebsite.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Contracts
{
    public interface IProductRepository
    {
        void Add(Product newProduct);
        Product Find(string id);
        Product Remove(string id);
        void Update(Product newProduct);
        List<Product> GetAll();
    }
}
