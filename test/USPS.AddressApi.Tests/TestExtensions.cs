using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USPS.AddressApi.Tests
{
    internal static class TestExtensions
    {
        internal static bool SequenceAndOrderEqual<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            if(!source.SequenceEqual(other))
            {
                return false;
            }
            
            var sourceIterator = source.GetEnumerator();
            var otherIterator = other.GetEnumerator();


             while (sourceIterator.MoveNext() && otherIterator.MoveNext())
            {
                if (!sourceIterator.Current!.Equals(otherIterator!.Current))
                    return false;
            }

           return true;
        }
    }
}