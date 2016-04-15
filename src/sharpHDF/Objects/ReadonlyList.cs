using System.Collections;
using System.Collections.Generic;

namespace CSharpHDF5.Objects
{
    public class ReadonlyList<T> : IEnumerable<T>
    {
        private readonly List<T> m_InternalList;

        public ReadonlyList()
        {
            m_InternalList = new List<T>();
        }

        internal void Add(T _item)
        {
            m_InternalList.Add(_item);
        }

        //internal void Clear()
        //{
        //    m_InternalList.Clear();
        //}

        internal bool Remove(T _item)
        {
            return m_InternalList.Remove(_item);
        }

        public bool Contains(T _item)
        {
            return m_InternalList.Contains(_item);
        }

        public void CopyTo(T[] _array, int _arrayIndex)
        {
            m_InternalList.CopyTo(_array, _arrayIndex);
        }

        public int Count
        {
            get { return m_InternalList.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_InternalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int _index]
        {
            get { return m_InternalList[_index]; }
        }
    }
}
