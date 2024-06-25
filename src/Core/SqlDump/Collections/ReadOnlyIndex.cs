using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SqlDump.Core.SqlDump.Collections;

/// <summary>
/// Dictionary collection where elements can be accessed by key or index.
/// </summary>
/// <typeparam name="TKey">Key type</typeparam>
/// <typeparam name="TValue">Value type</typeparam>
public sealed class ReadOnlyIndex<TKey, TValue> : IReadOnlyDictionary<TKey, TValue> 
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;
    private readonly KeyValuePair<TKey, TValue>[] _array;

    public ReadOnlyIndex(IEnumerable<KeyValuePair<TKey, TValue>> entries,
        IEqualityComparer<TKey>? keyComparer = null)
    {
        _array = entries.ToArray();
        _dictionary = new Dictionary<TKey, TValue>(_array, keyComparer ?? EqualityComparer<TKey>.Default);
    }

    /// <summary>
    /// Gets the number of items in the collection.
    /// </summary>
    public int Count => _array.Length;

    /// <summary>
    /// Gets the key at the given index.
    /// </summary>
    /// <param name="index">Index</param>
    /// <returns>Key</returns>
    public TKey KeyAt(int index) => _array[index].Key;

    /// <summary>
    /// Gets the value at the given index.
    /// </summary>
    /// <param name="index">Index</param>
    /// <returns>Value</returns>
    public TValue ValueAt(int index) => _array[index].Value;

    /// <summary>
    /// Gets the item in the collection by key.
    /// </summary>
    /// <param name="key">Key</param>
    public TValue this[TKey key] => _dictionary[key];

    /// <inheritdoc />
    public IEnumerable<TKey> Keys => _dictionary.Keys;

    /// <inheritdoc />
    public IEnumerable<TValue> Values => _dictionary.Values;

    /// <summary>
    /// Determines if the given key is valid.
    /// </summary>
    /// <param name="key">Key to test.</param>
    /// <returns><c>bool</c></returns>
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    /// <summary>
    /// Tries to get a value by key.
    /// </summary>
    /// <param name="key">Key whose value to retrieve.</param>
    /// <param name="value">The value if key exists.</param>
    /// <returns><c>true</c> if key exists and was assigned to value.</returns>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => 
        _dictionary.TryGetValue(key, out value);
    
    /// <inheritdoc />
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public override string ToString() => $"{Count}";
}