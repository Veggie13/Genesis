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
            int minY = Math.Min(original.GetLength(0), newArray.GetLength(0));
            int minX = Math.Min(original.GetLength(1), newArray.GetLength(1));

            for (int i = 0; i < minY; ++i)
                Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minX);

            return newArray;
        }

        public static Tuple<int, int> IndexOf<T>(this T[,] arr, T item)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] == null)
                    {
                        if (item == null)
                            return new Tuple<int, int>(i, j);
                    }
                    else if (arr[i, j].Equals(item))
                        return new Tuple<int, int>(i, j);
                }
            return new Tuple<int, int>(-1, -1);
        }
    }
}
