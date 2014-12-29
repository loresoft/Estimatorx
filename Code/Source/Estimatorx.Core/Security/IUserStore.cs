using System;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public interface IUserStore :
        IUserStore<User, string>,
        IUserPasswordStore<User, string>,
        IUserRoleStore<User, string>,
        IUserLoginStore<User, string>,
        IUserSecurityStampStore<User, string>,
        IUserEmailStore<User, string>,
        IUserClaimStore<User, string>,
        IQueryableUserStore<User, string>,
        IUserPhoneNumberStore<User, string>,
        IUserTwoFactorStore<User, string>,
        IUserLockoutStore<User, string>
    {
        
    }
}