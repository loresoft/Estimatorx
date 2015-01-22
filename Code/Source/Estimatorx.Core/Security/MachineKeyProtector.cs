using System.Web.Security;
using Microsoft.Owin.Security.DataProtection;

namespace Estimatorx.Core.Security
{
    public class MachineKeyProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }
}