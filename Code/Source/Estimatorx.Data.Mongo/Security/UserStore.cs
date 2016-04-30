using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estimatorx.Core.Security;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using Claim = System.Security.Claims.Claim;

namespace Estimatorx.Data.Mongo.Security
{
    public class UserStore : UserRepository, IUserStore
    {
        public UserStore()
            : this("EstimatorxMongo")
        {
        }

        public UserStore(string connectionName)
            : base(connectionName)
        {
        }

        public UserStore(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }


        public void Dispose()
        {
        }


        public Task CreateAsync(User user)
        {
            return InsertAsync(user);
        }

        public new Task UpdateAsync(User user)
        {
            return base.UpdateAsync(user);
        }

        public new Task DeleteAsync(User user)
        {
            return base.DeleteAsync(user);
        }


        public Task<User> FindByIdAsync(string userId)
        {
            return FindAsync(userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return FindOneAsync(u => u.UserName == userName.ToLowerInvariant());
        }



        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            // TODO what is this for?
            return Task.FromResult(false);
        }


        public Task AddToRoleAsync(User user, string roleName)
        {
            user.Roles.Add(roleName);
            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            user.Roles.Remove(roleName);
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            IList<string> roles = user.Roles.ToList();
            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            var contains = user.Roles.Contains(roleName);
            return Task.FromResult(contains);
        }


        public Task AddLoginAsync(User user, UserLoginInfo userLogin)
        {
            var login = new Login();
            login.LoginProvider = userLogin.LoginProvider;
            login.ProviderKey = userLogin.ProviderKey;

            user.Logins.Add(login);
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo userLogin)
        {
            var login = user.Logins.FirstOrDefault(l =>
                l.LoginProvider == userLogin.LoginProvider &&
                l.ProviderKey == userLogin.ProviderKey);

            if (login != null)
                user.Logins.Remove(login);

            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            IList<UserLoginInfo> result = user.Logins
                .Select(u => new UserLoginInfo(u.LoginProvider, u.ProviderKey))
                .ToList();

            return Task.FromResult(result);
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            return FindOneAsync(u =>
                u.Logins.Any(l =>
                    l.LoginProvider == login.LoginProvider &&
                    l.ProviderKey == login.ProviderKey
                )
            );
        }


        public Task SetSecurityStampAsync(User user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.FromResult(user.SecurityStamp);
        }


        public Task SetEmailAsync(User user, string email)
        {
            user.UserName = email;
            user.Email = email;

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return FindOneAsync(u => u.Email == email);
        }


        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            IList<Claim> result = user.Claims
                .Select(u => new Claim(u.Type, u.Value))
                .ToList();

            return Task.FromResult(result);
        }

        public Task AddClaimAsync(User user, Claim claim)
        {
            var c = new Core.Security.Claim();
            c.Type = claim.Type;
            c.Value = claim.Value;

            user.Claims.Add(c);
            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            var c = user.Claims.FirstOrDefault(l =>
                l.Type == claim.Type &&
                l.Value == claim.Value
            );

            if (c != null)
                user.Claims.Remove(c);

            return Task.FromResult(0);
        }


        public IQueryable<User> Users => All();


        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(User user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }


        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }


        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(user.LockoutEndDateUtc ?? new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = new DateTime(lockoutEnd.Ticks, DateTimeKind.Utc);
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

    }
}
