
using Estimatorx.Data.Mongo;
using Estimatorx.Data.Mongo.Mapping;

using Microsoft.Extensions.Hosting;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Estimatorx.Migration;

internal class ImportHostedService : IHostedService
{
    private readonly Data.Mongo.ProjectRepository _mongoProjectRepository;
    private readonly EstimatorX.Core.Repositories.IProjectRepository _projectRepository;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly EstimatorX.Core.Repositories.IOrganizationRepository _organizationRepository;

    public ImportHostedService(ProjectRepository mongoProjectRepository, EstimatorX.Core.Repositories.IProjectRepository projectRepository, IHostApplicationLifetime hostApplicationLifetime, EstimatorX.Core.Repositories.IOrganizationRepository organizationRepository)
    {
        _mongoProjectRepository = mongoProjectRepository;
        _projectRepository = projectRepository;
        _hostApplicationLifetime = hostApplicationLifetime;
        _organizationRepository = organizationRepository;
    }


    public async System.Threading.Tasks.Task StartAsync(CancellationToken cancellationToken)
    {

        var converter = new ProjectConverter();

        var sourceOrganizationId = "5e66a9551d331fce35ac256d";
        var targetOrganizationId = "61db9032d73397747b7719ae";

        //var sourceOrganizationId = "54b1f8c4a636558ac1233689";
        //var targetOrganizationId = "61dcaa9c307d214523c82bd8";


        var organization = await _organizationRepository.FindAsync(targetOrganizationId);

        var oldProjects = _mongoProjectRepository.All()
            .Where(p => p.OrganizationId == sourceOrganizationId)
            .ToList();


        foreach (var oldProject in oldProjects)
        {
            var newProject = converter.Convert(oldProject);

            newProject.OrganizationId = targetOrganizationId;
            newProject.OrganizationName = organization.Name;

            await _projectRepository.CreateAsync(newProject);
        }

        _hostApplicationLifetime.StopApplication();
    }

    public System.Threading.Tasks.Task StopAsync(CancellationToken cancellationToken)
    {
        return System.Threading.Tasks.Task.CompletedTask;
    }
}
