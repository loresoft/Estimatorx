namespace EstimatorX.Core.Comparison;

public static class Equality<TComparer>
{
    public static IEqualityComparer<TComparer> CreateComparer<TKey>(Func<TComparer, TKey> keySelector)
    {
        return CreateComparer(keySelector, null);
    }

    public static IEqualityComparer<TComparer> CreateComparer<TKey>(Func<TComparer, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        return new KeyEqualityComparer<TComparer, TKey>(keySelector, comparer);
    }

}
