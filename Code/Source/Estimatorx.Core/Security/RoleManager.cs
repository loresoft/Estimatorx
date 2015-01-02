using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public class RoleManager : RoleManager<Role, string>
    {
        public RoleManager(IRoleStore<Role, string> store) 
            : base(store)
        {
        }
    }
}