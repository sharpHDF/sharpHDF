/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by Brian Nelson 2016.                                           *
 * See license in repo for more information                                  *
 * https://github.com/sharpHDF/sharpHDF                                      *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections;
using System.Collections.Generic;
using sharpHDF.Library.Interfaces;

namespace sharpHDF.Library.Objects
{
    public class ReadonlyNamedItemList<T> : IEnumerable<T> where T:IHasName
    {
        private readonly List<T> m_InternalList;
        private readonly Dictionary<string, T> m_Dictionary; 

        public ReadonlyNamedItemList()
        {
            m_InternalList = new List<T>();
            m_Dictionary = new Dictionary<string, T>();
        }

        internal void Add(T _item)
        {
            m_InternalList.Add(_item);
            m_Dictionary.Add(_item.Name, _item);
        }

        internal bool Remove(T _item)
        {
            m_Dictionary.Remove(_item.Name);
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

        public T this[string _name]
        {
            get
            {
                if (m_Dictionary.ContainsKey(_name))
                {
                    return m_Dictionary[_name];
                }

                return default(T);
            }
        }

    }
}
