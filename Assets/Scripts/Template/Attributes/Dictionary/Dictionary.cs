using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrawedDictionary
{
    /// <summary>
    /// ����� �������� Dictionary. ����� ��� ���� ����� PropertyDrawer ��� ���������� Generic Dictionary
    /// </summary>
    public class PDictionary { };

    /// <summary>
    /// Inspector Dictionary 
    /// </summary>
    [System.Serializable]
    public class Dictionary<K, V> : PDictionary
    {
        public string _KType, _VType;
        public List<K> _keys = new List<K>();
        public List<V> _values = new List<V>();
        System.Collections.Generic.Dictionary<K, V> _dictionary = new System.Collections.Generic.Dictionary<K, V>();

        /// <summary>
        /// <i>�����������:</i> �������� ���� K V � ���������� �� � ������. ���� ������ �� _keys, _values (�� Unity ���������) � ���������� � System.Collections.Generic.Dictionary
        /// </summary>
        public Dictionary()
        {
            UpdateTypes();
            UpdateDictionary();
        }
        /// <summary>
        /// �������� ���� K V � ���������� �� � ������.
        /// </summary>
        public void UpdateTypes()
        {
            _KType = typeof(K).FullName.ToString();
            _VType = typeof(V).FullName.ToString();
        }
        /// <summary>
        /// ��������� ������ �� _keys � _values � System.Collections.Generic.Dictionary. ��������� �� Null � ���������� ������. 
        /// </summary>
        public void UpdateDictionary()
        {
            _dictionary.Clear();
            for (int i = 0; i < _keys.Count; i++)
            {
                if (_keys[i] != null)
                {
                    if (!_dictionary.ContainsKey(_keys[i]))
                    {
                        _dictionary.Add(_keys[i], _values[i]);
                    }
                    else
                    {
                        _keys.RemoveAt(i);
                        _values.RemoveAt(i);
                        UpdateDictionary();
                        return;
                    }
                }
                else
                {
                    _keys.RemoveAt(i);
                    _values.RemoveAt(i);
                    UpdateDictionary();
                    return;
                }
            }
        }

        /// <summary>
        /// ����� ����� ��� ��������� ������ ��������� � �����. (����� ��� ���������� ����� ������ ���� Object ��� ��� ��� ����� ���� null) 
        /// </summary>
        public void AddNull(K key, V value)
        {
            _keys.Add(key);
            _values.Add(value);
        }
        /// <summary>
        /// �������� �������� �� _keys � _value � ������� id.
        /// </summary>
        public void RemoveID(int id)
        {
            UpdateTypes();
            _keys.RemoveAt(id);
            _values.RemoveAt(id);
            UpdateDictionary();
            UpdateTV();
        }

        /// <summary>
        /// �������� �������� �� �����.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(K key)
        {
            UpdateTypes();
            if (_dictionary.ContainsKey(key))
            {
                _dictionary.Remove(key);
            }
            UpdateTV();
        }

        /// <summary>
        /// ���������� ��������.
        /// </summary>
        public void Add(K key, V value)
        {
            UpdateTypes();
            if (_keys.Count != 0 && _dictionary.Count == 0)
            {
                UpdateDictionary();
            }
            if (!_keys.Contains(key))
            {
                _dictionary.Add(key, value);
            }
            UpdateTV();
        }
        /// <summary>
        /// ��������� ������ � List`s � ���������� � ��� ������ �� System.Collections.Generic.Dictionary 
        /// </summary>
        public void UpdateTV()
        {
            _keys.Clear();
            _values.Clear();
            foreach (var item in _dictionary)
            {
                _keys.Add(item.Key);
                _values.Add(item.Value);
            }
        }

        public V this[K index]
        {
            get
            {
                if (_dictionary.Count == 0)
                {
                    UpdateDictionary();
                }
                if (_dictionary.ContainsKey(index))
                {
                    return _dictionary[index];
                }
                else
                {
                    Debug.LogError($"\"{index}\" not exist in dictionary");
                    return default(V);
                }
            }

            set
            {
                if (_dictionary.Count == 0)
                {
                    UpdateDictionary();
                }
                _dictionary[index] = value;
            }
        }
    }
}