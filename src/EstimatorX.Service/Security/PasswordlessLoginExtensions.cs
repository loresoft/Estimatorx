using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EstimatorX.Service.Security
{
    public static class PasswordlessLoginExtensions
    {
        public static IdentityBuilder AddPasswordlessLoginProvider(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var provider = typeof(PasswordlessLoginProvider<>).MakeGenericType(userType);

            return builder.AddTokenProvider(PasswordlessLoginConstants.ProviderName, provider);
        }

        public static async Task<string> GenerateUserPasswordlessTokenAsync<TUser>(this UserManager<TUser> userManager, TUser user)
            where TUser : class
        {
            return await userManager.GenerateUserTokenAsync(user,
                PasswordlessLoginConstants.ProviderName,
                PasswordlessLoginConstants.Purpose);
        }

        public static async Task<bool> VerifyUserPasswordlessTokenAsync<TUser>(this UserManager<TUser> userManager, TUser user, string token)
            where TUser : class
        {
            return await userManager.VerifyUserTokenAsync(user,
                PasswordlessLoginConstants.ProviderName,
                PasswordlessLoginConstants.Purpose,
                token);
        }

    }
}
