using Microsoft.Owin.Security.DataProtection;

namespace Estimatorx.Core.Security
{
    public class MachineKeyProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyProtector(purposes);
        }
    }
}