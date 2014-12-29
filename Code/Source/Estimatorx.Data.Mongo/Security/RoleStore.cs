using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimatorx.Core.Security;
using MongoDB.Driver;

namespace Estimatorx.Data.Mongo.Security
{
    public class RoleStore : RoleRepository, IRoleStore
    {
        public RoleStore()
        {
        }

        public RoleStore(string connectionName)
            : base(connectionName)
        {
        }

        public RoleStore(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }

        public void Dispose()
        {
        }

        public Task CreateAsync(Role role)
        {
            return Task.Run(() => Insert(role));
        }

        public Task UpdateAsync(Role role)
        {
            return Task.Run(() => Update(role));
        }

        public Task DeleteAsync(Role role)
        {
            return Task.Run(() => Delete(role));
        }

        public Task<Role> FindByIdAsync(string roleId)
        {
            return Task.Run(() => Find(roleId));
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return Task.Run(() => FindOne(u => u.Name == roleName));
        }

        public IQueryable<Role> Roles
        {
            get { return All(); }
        }
    }
}
