using System.Text.Json;

using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Changes;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Stores;

[RegisterScoped]
public class TemplateStore : ChangeFeedStoreBase<Template, TemplateRepository>
{
    public TemplateStore(
        ILoggerFactory loggerFactory,
        TemplateRepository repository,
        JsonSerializerOptions serializerOptions,
        NavigationManager navigation)
        : base(loggerFactory, repository, serializerOptions, navigation)
    {
    }


    public override string ChangeEventName { get; } = ChangeFeedConstants.TemplateChangeEventName;

}
