using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Service.Security
{

    public class PasswordlessLoginProvider<TUser> : DataProtectorTokenProvider<TUser>
        where TUser : class
    {

        public PasswordlessLoginProvider(IDataProtectionProvider dataProtectionProvider, IOptions<PasswordlessLoginProviderOptions> options, ILogger<PasswordlessLoginProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
