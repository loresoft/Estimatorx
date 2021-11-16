namespace EstimatorX.Core.Comparison;

public static class DeltaCompare
{
    public static Delta<T> Compare<T>(IEnumerable<T> existing, IEnumerable<T> current, IEqualityComparer<T> comparer = null)
    {
        if (comparer == null)
            comparer = EqualityComparer<T>.Default;

        var existingList = existing.ToList();
        var currentList = current.ToList();

        var matched = existingList.Intersect(currentList, comparer).ToList();
        var created = currentList.Except(existingList, comparer).ToList();
        var deleted = existingList.Except(currentList, comparer).ToList();

        return new Delta<T>
        {
            Created = created,
            Deleted = deleted,
            Matched = matched
        };
    }
}
