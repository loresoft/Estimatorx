using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimatorx.Data.Mongo.Tests
{
    public class TestBase
    {
        protected TestBase()
        {
            Bootstrap.Start();
        }
    }
}
