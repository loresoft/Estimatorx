using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace Estimatorx.Core.Security
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(IUserStore<User, string> store, IDataProtectionProvider provider) 
            : base(store)
        {
            // Configure validation logic for user names
            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            EmailService = new IdentityEmailService();

            UserTokenProvider = new DataProtectorTokenProvider<User>(provider.Create("Estimatorx"));
        }
    }
}
