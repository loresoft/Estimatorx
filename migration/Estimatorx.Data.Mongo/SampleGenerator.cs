using System;

using Estimatorx.Data.Mongo.Providers;

using MongoDB.Bson;

using Task = Estimatorx.Data.Mongo.Task;

namespace Estimatorx.Data.Mongo
{
    public class SampleGenerator : ISampleGenerator
    {



        public Project GenerateProject(string organizationId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));

            var project = new Project
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Sample Project",
                Description = "Generated Sample Project",
                OrganizationId = organizationId,
                HoursPerWeek = 30,
                ContingencyRate = 10,
                Created = DateTime.Now,
                Creator = String.Empty,
                Updated = DateTime.Now,
                Updater = String.Empty
            };

            project.Assumptions.Add("Project Assumption");

            var presentationFactor = new Factor
            {
                Id = ObjectId.GenerateNewId().ToString(),
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
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Back-End Logic Factor",
                VerySimple = 2,
                Simple = 4,
                Medium = 8,
                Complex = 16,
                VeryComplex = 32,
            };
            project.Factors.Add(backendFactor);

            var dataFactor = new Factor
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Data Model Factor",
                VerySimple = 1,
                Simple = 2,
                Medium = 4,
                Complex = 8,
                VeryComplex = 16,
            };
            project.Factors.Add(dataFactor);

            var section = new Section
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Application Sample Section",
            };
            project.Sections.Add(section);

            var presentationEstimate = new Task
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Presentation Task",
                FactorId = presentationFactor.Id,
                Medium = 1,
                Simple = 1
            };
            section.Tasks.Add(presentationEstimate);

            var backendEstimate = new Task
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Back-End Logic Task",
                FactorId = backendFactor.Id,
                Complex = 1,
                Simple = 1
            };
            section.Tasks.Add(backendEstimate);

            ProjectCalculator.UpdateTotals(project);

            return project;
        }

        public Template GenerateTemplate(string organizationId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));


            var template = new Template
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Sample Template",
                Description = "Generated Sample Template",
                OrganizationId = organizationId,
                Created = DateTime.Now,
                Creator = String.Empty,
                Updated = DateTime.Now,
                Updater = String.Empty
            };

            var presentationFactor = new Factor
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Presentation Factor",
                VerySimple = 2,
                Simple = 4,
                Medium = 8,
                Complex = 16,
                VeryComplex = 32,
            };
            template.Factors.Add(presentationFactor);

            var backendFactor = new Factor
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Back-End Logic Factor",
                VerySimple = 2,
                Simple = 4,
                Medium = 8,
                Complex = 16,
                VeryComplex = 32,
            };
            template.Factors.Add(backendFactor);

            var dataFactor = new Factor
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Data Model Factor",
                VerySimple = 1,
                Simple = 2,
                Medium = 4,
                Complex = 8,
                VeryComplex = 16,
            };
            template.Factors.Add(dataFactor);

            return template;
        }
    }
}
