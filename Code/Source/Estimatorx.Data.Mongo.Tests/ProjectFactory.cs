using System;
using Estimatorx.Core;
using Estimatorx.Core.Security;
using MongoDB.Bson;

namespace Estimatorx.Data.Mongo.Tests
{
    public static class ProjectFactory
    {
        public static Project Create()
        {
            var project = new Project
            {
                HoursPerWeek = 30,
                ContingencyRate = 10,
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "New Project",
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

            var section = new Section
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Application Section",
            };
            project.Sections.Add(section);

            var presentationEstimate = new Task
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Presentation Task",
                FactorId = presentationFactor.Id,
            };
            section.Tasks.Add(presentationEstimate);

            var backendEstimate = new Task
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Back-End Logic Task",
                FactorId = backendFactor.Id,
            };
            section.Tasks.Add(backendEstimate);

            return project;
        }
    }
}