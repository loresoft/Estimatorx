using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estimatorx.Web.Models
{
    public class VersionModel
    {
        public static string InformationalVersion
        {
            get { return ThisAssembly.AssemblyInformationalVersion; }
        }
    }
}