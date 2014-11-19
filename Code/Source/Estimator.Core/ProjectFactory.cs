using System;

namespace Estimator.Core
{
    public static class ProjectFactory
    {
        public static Project Create()
        {
            var project = new Project
            {
                IsActive = true,
                HoursPerWeek = 30,
                ContingencyRate = 10,
                Id = Guid.NewGuid(),
                Name = "New Project",
                SysCreateDate = DateTime.Now,
                SysCreateUser = UserName.Current(),
                SysUpdateDate = DateTime.Now,
                SysUpdateUser = UserName.Current()
            };

            var presentationFactor = new Factor
            {
                Id = Guid.NewGuid(),
                Name = "Presentation Factor",
                VerySimple = 2,
                Simple = 4,
                Medium = 8,
                Complex = 16,
                VeryComplex = 32,
                SysCreateDate = DateTime.Now,
                SysCreateUser = UserName.Current(),
                SysUpdateDate = DateTime.Now,
                SysUpdateUser = UserName.Current()
            };
            project.Factors.Add(presentationFactor);

            var backendFactor = new Factor
            {
                Id = Guid.NewGuid(),
                Name = "Back-End Logic Factor",
                VerySimple = 2,
                Simple = 4,
                Medium = 8,
                Complex = 16,
                VeryComplex = 32,
                SysCreateDate = DateTime.Now,
                SysCreateUser = UserName.Current(),
                SysUpdateDate = DateTime.Now,
                SysUpdateUser = UserName.Current()
            };
            project.Factors.Add(backendFactor);

            var section = new Section
            {
                Id = Guid.NewGuid(),
                Name = "Application Section",
                SysCreateDate = DateTime.Now,
                SysCreateUser = UserName.Current(),
                SysUpdateDate = DateTime.Now,
                SysUpdateUser = UserName.Current()
            };
            project.Sections.Add(section);

            var presentationEstimate = new Estimate
            {
                Id = Guid.NewGuid(),
                Name = "Presentation Task",
                FactorId = presentationFactor.Id,
                SysCreateDate = DateTime.Now,
                SysCreateUser = UserName.Current(),
                SysUpdateDate = DateTime.Now,
                SysUpdateUser = UserName.Current()
            };
            section.Estimates.Add(presentationEstimate);

            var backendEstimate = new Estimate
            {
                Id = Guid.NewGuid(),
                Name = "Back-End Logic Task",
                FactorId = backendFactor.Id,
                SysCreateDate = DateTime.Now,
                SysCreateUser = UserName.Current(),
                SysUpdateDate = DateTime.Now,
                SysUpdateUser = UserName.Current()
            };
            section.Estimates.Add(backendEstimate);

            return project;
        }
    }
}