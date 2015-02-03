using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Estimatorx.Core.Tests.Export
{
    public class DocumentCreatorTest
    {
        [Fact]
        public void CreatePdf()
        {
            var project = ProjectFactory.Create();
            var creator = new DocumentCreator();

            string file = string.Format("Test-{0}.pdf", DateTime.Now.Ticks);
            creator.CreatePdf(project, file);
        }
    }
}
