using Autofac;
using Estimatorx.Core.Security;
using Estimatorx.Data.Mongo.Security;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Data.Mongo.Modules
{
    public class SecurityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<RoleStore>()
                .As<IRoleStore<Role, string>>()
                .As<IRoleStore>()
                .As<IRoleRepository>()
                .SingleInstance();


            builder.RegisterType<UserStore>()
                .As<IUserStore<User, string>>()
                .As<IUserStore>()
                .As<IUserRepository>()
                .SingleInstance();


            builder.RegisterType<OrganizationRepository>()
                .As<IOrganizationRepository>()
                .SingleInstance();
        }
    }
}