using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace VAllens.LookupTable
{
    /// <summary>
    /// 忽略大小写的比较
    /// </summary>
    public class CaseInsensitiveComparer<T> : IEqualityComparer<T>
    {
        private readonly CaseInsensitiveComparer _comparer;

        public CaseInsensitiveComparer()
        {
            _comparer = CaseInsensitiveComparer.DefaultInvariant;
        }

        public CaseInsensitiveComparer(CultureInfo culture)
        {
            _comparer = new CaseInsensitiveComparer(culture);
        }

        public bool Equals(T x, T y)
        {
            return _comparer.Compare(x, y) == 0;
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}