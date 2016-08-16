using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using NLog.Fluent;
using Task = Estimatorx.Core.Task;

namespace Estimatorx.Data.Mongo
{
    public class SampleGenerator : ISampleGenerator
    {

        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public Project GenerateProject(string organizationId)
        {
            if (organizationId == null)
                throw new ArgumentNullException(nameof(organizationId));

            _logger.Info()
                .Message("Generate sample project for organization '{0}'", organizationId)
                .Property("Organization", organizationId)
                .Write();

            var project = new Project
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Sample Project",
                Description = "Generated Sample Project",
                OrganizationId = organizationId,
                HoursPerWeek = 30,
                ContingencyRate = 10,
                Created = DateTime.Now,
                Creator = UserName.Current(),
                Updated = DateTime.Now,
                Updater = UserName.Current()
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

            _logger.Info()
                .Message("Generate sample template for organization '{0}'", organizationId)
                .Property("Organization", organizationId)
                .Write();

            var template = new Template
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Sample Template",
                Description = "Generated Sample Template",
                OrganizationId = organizationId,
                Created = DateTime.Now,
                Creator = UserName.Current(),
                Updated = DateTime.Now,
                Updater = UserName.Current()
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
