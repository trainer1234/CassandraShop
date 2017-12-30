using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Models.ProductModels
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Manufacture_Year { get; set; }
        public string Manufacture_Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
