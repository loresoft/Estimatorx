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
            return InsertAsync(role);
        }

        public new Task UpdateAsync(Role role)
        {
            return base.UpdateAsync(role);
        }

        public new Task DeleteAsync(Role role)
        {
            return base.DeleteAsync(role);
        }

        public Task<Role> FindByIdAsync(string roleId)
        {
            return FindAsync(roleId);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return FindOneAsync(u => u.Name == roleName);
        }

        public IQueryable<Role> Roles => All();
    }
}
