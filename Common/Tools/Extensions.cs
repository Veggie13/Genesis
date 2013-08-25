using System.Windows.Forms;
using System.Collections.Generic;
using System;

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
        
        public static T[,] Resize<T>(this T[,] original, int x, int y)
        {
            T[,] newArray = new T[x, y];
            int minX = Math.Min(original.GetLength(0), newArray.GetLength(0));
            int minY = Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minY; ++i)
                Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minX);

            return newArray;
        }
    }
}
