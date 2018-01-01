using CassandraShopWebsite.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Models.ProductAccountModels
{
    public class ProductByUser
    {
        public ProductByUser()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string User_Id { get; set; }
        public string User_Name { get; set; }
        public List<string> Product_Names { get; set; }

    }
}
