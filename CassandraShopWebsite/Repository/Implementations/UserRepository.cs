using Cassandra;
using Cassandra.Mapping;
using CassandraShopWebsite.Models.AccountModels;
using CassandraShopWebsite.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private ISession _session;

        public UserRepository(ISession session)
        {
            _session = session;
        }

        public void Add(User newUser)
        {
            var insert = new SimpleStatement("insert into user (id, username, fullname, birthday, password) values (?, ?, ?, ?, ?)",
                                  newUser.Id, newUser.UserName, newUser.FullName, newUser.Birthday, newUser.Password);
            _session.Execute(insert);
        }

        public User Find(string id)
        {
            var mapper = new Mapper(_session);
            var userQuery = _session.Execute("select * from user where id = " + Guid.Parse(id));
            var searchUser = new User();
            foreach (var row in userQuery)
            {
                var user = new User
                {
                    Id = row.GetValue<Guid>("id"),
                    FullName = row.GetValue<string>("fullname"),
                    UserName = row.GetValue<string>("username"),
                    Password = row.GetValue<string>("password"),
                };
                if (row.GetValue<LocalDate>("birthday") != null)
                {
                    user.Birthday = row.GetValue<LocalDate>("birthday");
                    user.BirthdayTemp = new DateTime(user.Birthday.Year, user.Birthday.Month, user.Birthday.Day);
                }
                searchUser = user;
            }
            return searchUser;
            //return mapper.FirstOrDefault<User>("select * from user where id = ?", Guid.Parse(id));
        }

        public List<User> GetAll()
        {
            var mapper = new Mapper(_session);
            var userTable = _session.Execute("select * from user");
            var users = new List<User>();
            foreach (var row in userTable)
            {
                var user = new User
                {
                    Id = row.GetValue<Guid>("id"),
                    FullName = row.GetValue<string>("fullname"),
                    UserName = row.GetValue<string>("username"),
                    Password = row.GetValue<string>("password"),
                };
                if(row.GetValue<LocalDate>("birthday") != null)
                {
                    user.Birthday = row.GetValue<LocalDate>("birthday");
                    user.BirthdayTemp = new DateTime(user.Birthday.Year, user.Birthday.Month, user.Birthday.Day);
                }
                users.Add(user);
            }
            return users;
            //return mapper.Fetch<User>("select * from user").ToList();
        }

        public User Remove(string id)
        {
            var user = Find(id);
            if (user == null)
                return null;

            var del = new SimpleStatement("delete from user where id = ?", user.Id);
            _session.Execute(del);

            return user;
        }

        public void Update(User newUser)
        {
            var searchUser = Find(newUser.Id.ToString());
            if (searchUser != null)
            {
                var cql = new SimpleStatement("update user set fullname = ?, birthday = ?, password = ? where id = ?",
                                               newUser.FullName, newUser.Birthday, newUser.Password, searchUser.Id);
                _session.Execute(cql);
            }
        }
    }
}
