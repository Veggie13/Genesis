using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.Tools
{
    public interface IListWithAddRange<T> : IList<T>
    {
        void AddRange(IEnumerable<T> items);
    }
}
