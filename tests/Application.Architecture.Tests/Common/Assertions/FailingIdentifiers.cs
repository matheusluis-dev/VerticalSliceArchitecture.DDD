namespace Application.Architecture.Tests.Common.Assertions;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed class FailingIdentifiers : IList<string>
{
    private readonly List<string> _failingIdentifiers = [];

    public string GetErrorMessage()
    {
        if (_failingIdentifiers.Count == 0)
            return string.Empty;

        return $$"""

            Inconsistences were found in those elements:
            {{_failingIdentifiers.ToUnorderedStringList('-', 2)}}
            
            """;
    }

    public string this[int index]
    {
        get => _failingIdentifiers[index];
        set => _failingIdentifiers[index] = value;
    }

    public int Count => _failingIdentifiers.Count;

    public bool IsReadOnly => false;

    public void Add(string item)
    {
        _failingIdentifiers.Add(item);
    }

    public void AddRange(params IEnumerable<string> items)
    {
        _failingIdentifiers.AddRange(items);
    }

    public void Clear()
    {
        _failingIdentifiers.Clear();
    }

    public bool Contains(string item)
    {
        return _failingIdentifiers.Contains(item, StringComparer.Ordinal);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
        _failingIdentifiers.CopyTo(array, arrayIndex);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _failingIdentifiers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(string item)
    {
        return _failingIdentifiers.IndexOf(item);
    }

    public void Insert(int index, string item)
    {
        _failingIdentifiers.Insert(index, item);
    }

    public bool Remove(string item)
    {
        return _failingIdentifiers.Remove(item);
    }

    public void RemoveAt(int index)
    {
        _failingIdentifiers.RemoveAt(index);
    }
}
