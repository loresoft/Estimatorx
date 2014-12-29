using System;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public interface IRoleStore :
        IRoleStore<Role, string>,
        IQueryableRoleStore<Role, string>
    {

    }
}