namespace EstimatorX.Core.Comparison;

public class KeyEqualityComparer<TComparer, TKey> : IEqualityComparer<TComparer>
{
    private readonly Func<TComparer, TKey> _keySelector;
    private readonly IEqualityComparer<TKey> _comparer;

    public KeyEqualityComparer(Func<TComparer, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
    }

    public bool Equals(TComparer x, TComparer y)
    {
        return _comparer.Equals(_keySelector(x), _keySelector(y));
    }

    public int GetHashCode(TComparer obj)
    {
        return _comparer.GetHashCode(_keySelector(obj));
    }
}
