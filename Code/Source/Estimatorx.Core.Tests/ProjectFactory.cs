using System;
using Estimatorx.Core.Security;

namespace Estimatorx.Core.Tests
{
    public static class ProjectFactory
    {
        public static Project Create()
        {
            var project = new Project
            {
                HoursPerWeek = 30,
                ContingencyRate = 10,
                Id = Guid.NewGuid().ToString(),
                Name = "New Project",
                Created = DateTime.Now,
                Creator = UserName.Current(),
                Updated = DateTime.Now,
                Updater = UserName.Current()
            };

            var presentationFactor = new Factor
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Presentation Factor",
                VerySimple = 2,
                Simple = 4,
                Medium = 8,
                Complex = 16,
                VeryComplex = 32,
            };
            project.Factors.Add(presentationFactor);

            var backendFactor = new Factor
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Back-End Logic Factor",
                VerySimple = 2,
                Simple = 4,
                Medium = 8,
                Complex = 16,
                VeryComplex = 32,
            };
            project.Factors.Add(backendFactor);

            var section = new Section
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Application Section",
            };
            project.Sections.Add(section);

            var presentationEstimate = new Task
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Presentation Task",
                FactorId = presentationFactor.Id,
            };
            section.Tasks.Add(presentationEstimate);

            var backendEstimate = new Task
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Back-End Logic Task",
                FactorId = backendFactor.Id,
            };
            section.Tasks.Add(backendEstimate);

            return project;
        }
    }
}