using System;
using Microsoft.AspNetCore.Identity;

namespace EstimatorX.Service.Security
{
    public class PasswordlessLoginProviderOptions : DataProtectionTokenProviderOptions
    {

        public PasswordlessLoginProviderOptions()
        {
            // update the defaults
            Name = PasswordlessLoginConstants.ProviderName;
            TokenLifespan = TimeSpan.FromMinutes(15);
        }
    }
}
