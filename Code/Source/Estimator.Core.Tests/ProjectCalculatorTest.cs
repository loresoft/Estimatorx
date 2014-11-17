using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Estimator.Core.Tests
{
    public class ProjectCalculatorTest
    {
        [Fact]
        public void MyFact()
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Unit Test",
                Description = "Unit Test",
                ContingencyRate = .10,
                HoursPerWeek = 30,
                IsActive = true,
                Sections = new List<Section>
                {
                    new Section
                    {
                        Id = Guid.NewGuid(),
                        Name = "Edit Estimate Page",
                        Estimates = new List<Estimate>
                        {
                            new Estimate
                            {
                                Id = Guid.NewGuid(),
                                Name = "Create Web View",
                                Simple = 1
                            },
                        }
                    }
                }
            };
        }
    }
}
