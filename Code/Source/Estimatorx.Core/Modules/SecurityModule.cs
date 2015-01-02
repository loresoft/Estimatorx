using System;
using Autofac;
using Estimatorx.Core.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Estimatorx.Core.Modules
{
    public class SecurityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<RoleManager>()
                .AsSelf()
                .As<RoleManager<Role, string>>();

            builder.RegisterType<UserManager>()
                .AsSelf()
                .As<UserManager<User, string>>();

            builder.RegisterType<SignInManager>()
                .AsSelf()
                .As<SignInManager<User, string>>();

        }
    }
}
