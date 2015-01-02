using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(IUserStore<User, string> store) 
            : base(store)
        {
        }
    }
}
