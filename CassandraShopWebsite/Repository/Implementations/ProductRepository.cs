using Cassandra;
using Cassandra.Mapping;
using CassandraShopWebsite.Models.ProductModels;
using CassandraShopWebsite.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private ISession _session;

        public ProductRepository(ISession session)
        {
            _session = session;
        }

        public void Add(Product newProduct)
        {
            var insert = new SimpleStatement("insert into product (id, manufacture_year, price, description, manufacture_name, name) values (?, ?, ?, ?, ?, ?)",
                                  newProduct.Id, newProduct.Manufacture_Year, newProduct.Price, newProduct.Description, newProduct.Manufacture_Name, newProduct.Name);
            _session.Execute(insert);
        }

        public Product Find(string id)
        {
            var mapper = new Mapper(_session);
            return mapper.FirstOrDefault<Product>("select * from product where id = ?", Guid.Parse(id));
        }

        public Product QueryByString(string colName, string name)
        {
            var mapper = new Mapper(_session);
            return mapper.FirstOrDefault<Product>("select * from product where " + colName + " = ?", name);
        }

        public Product QueryByInt(string colName, int val)
        {
            var mapper = new Mapper(_session);
            return mapper.FirstOrDefault<Product>("select * from product where " + colName + " = ?", val);
        }

        public List<Product> GetAll()
        {
            var mapper = new Mapper(_session);
            return mapper.Fetch<Product>("select * from product").ToList();
        }

        public Product Remove(string id)
        {
            var product = Find(id);
            if (product == null)
                return null;

            var del = new SimpleStatement("delete from product where id = ?", product.Id);
            _session.Execute(del);

            return product;
        }

        public void Update(Product newProduct)
        {
            var searchProduct = Find(newProduct.Id.ToString());
            if (searchProduct != null)
            {
                var cql = new SimpleStatement("update product set name = ?, manufacture_name = ?, manufacture_year = ?, price = ?, description = ? where id = ?",
                                               newProduct.Name, newProduct.Manufacture_Name, newProduct.Manufacture_Year, newProduct.Price, newProduct.Description, searchProduct.Id);
                _session.Execute(cql);
            }
        }
    }
}
