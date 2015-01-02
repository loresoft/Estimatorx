using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Estimatorx.Core.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Estimatorx.Web.Modules
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .Register(c => HttpContext.Current.GetOwinContext().Authentication)
                .As<IAuthenticationManager>();
        }
    }
}