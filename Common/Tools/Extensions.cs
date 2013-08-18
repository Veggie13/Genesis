using System.Windows.Forms;
using System.Collections.Generic;

namespace Genesis.Common.Tools
{
    public static class Extensions
    {
        public static bool IsDataPresent<T>(this DragEventArgs e) where T : class
        {
            return e.Data.GetDataPresent(typeof(T));
        }

        public static T GetData<T>(this DragEventArgs e) where T : class
        {
            return e.Data.GetData(typeof(T)) as T;
        }

        public static void AddRange<T>(this ICollection<T> me, IEnumerable<T> items)
        {
            foreach (T item in items)
                me.Add(item);
        }
    }
}
