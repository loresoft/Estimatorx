using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class UserReadModel : EntityReadModel, ISupportSearch
{
    public string? Email { get; set; }

    public string? Name { get; set; }

    public bool IsAdministrator { get; set; }

    public bool IsDisabled { get; set; }


    public static IEnumerable<string> SearchFields()
        => [nameof(Email), nameof(Name)];

    public static string SortField()
        => nameof(Email);

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return Email ?? string.Empty;

        if (string.IsNullOrWhiteSpace(Email))
            return Name ?? "Anonymous";

        return $"{Name} <{Email}>";
    }
}
