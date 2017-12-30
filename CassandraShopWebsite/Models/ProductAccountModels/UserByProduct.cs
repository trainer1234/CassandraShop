using CassandraShopWebsite.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Models.ProductAccountModels
{
    public class UserByProduct
    {
        public UserByProduct()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Product_Id { get; set; }
        public List<string> User_Names { get; set; }

    }
}
