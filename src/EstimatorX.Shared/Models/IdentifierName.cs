namespace EstimatorX.Shared.Models;

public class IdentifierName
{
    public string Id { get; set; }

    public string Name { get; set; }
   

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}
