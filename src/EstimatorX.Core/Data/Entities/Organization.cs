using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Data.Entities;

public class Organization : IHaveIdentifier<long>, ITrackCreated, ITrackUpdated
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IList<string>? HostMatches { get; set; }


    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string? UpdatedBy { get; set; }


    public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();

    public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    public virtual ICollection<Template> Templates { get; set; } = new HashSet<Template>();

    public virtual ICollection<UserOrganization> UserOrganizations { get; set; } = new HashSet<UserOrganization>();
}
