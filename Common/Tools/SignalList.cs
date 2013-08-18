using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Common.Tools
{
    public class SignalList<T> : IList<T>
    {
        #region Private Members
        private List<T> _list;
        #endregion

        #region Constructors
        public SignalList()
        {
            _list = new List<T>();
        }

        public SignalList(IEnumerable<T> list)
        {
            _list = new List<T>(list);
        }
        #endregion

        #region Events
        public delegate void ItemsEvent(IEnumerable<Tuple<int, T>> items);
        public event ItemsEvent AboutToAddItems = delegate { };
        public event ItemsEvent ItemsAdded = delegate { };
        public event ItemsEvent AboutToRemoveItems = delegate { };
        public event ItemsEvent ItemsRemoved = delegate { };
        #endregion

        #region Public Methods
        public void AddRange(IEnumerable<T> items)
        {
            var itemsRange = multiItems(Count, items);
            AboutToAddItems(itemsRange);
            _list.AddRange(items);
            ItemsAdded(itemsRange);
        }
        #endregion

        #region IList
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            var it = singleItem(index, item);
            AboutToAddItems(it);
            _list.Insert(index, item);
            ItemsAdded(it);
        }

        public void RemoveAt(int index)
        {
            T item = _list[index];
            var it = singleItem(index, item);
            AboutToRemoveItems(it);
            _list.RemoveAt(index);
            ItemsRemoved(it);
        }

        public T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public void Add(T item)
        {
            var it = singleItem(Count, item);
            AboutToAddItems(it);
            _list.Add(item);
            ItemsAdded(it);
        }

        public void Clear()
        {
            var all = multiItems(0, _list);
            AboutToRemoveItems(all);
            _list.Clear();
            ItemsRemoved(all);
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            var it = singleItem(IndexOf(item), item);
            AboutToRemoveItems(it);
            if (_list.Remove(item))
            {
                ItemsRemoved(it);
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region Private Helpers
        private IEnumerable<Tuple<int, T>> multiItems(int index, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                yield return new Tuple<int, T>(index++, item);
            }
        }

        private IEnumerable<Tuple<int, T>> singleItem(int index, T item)
        {
            yield return new Tuple<int, T>(index, item);
        }
        #endregion
    }
}
