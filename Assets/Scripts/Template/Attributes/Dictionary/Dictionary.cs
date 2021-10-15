using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrawedDictionary
{
    /// <summary>
    /// Класс родитель Dictionary. Нужен для того чтобы PropertyDrawer мог отобразить Generic Dictionary
    /// </summary>
    public class PDictionary { };

    /// <summary>
    /// Inspector Dictionary 
    /// </summary>
    [System.Serializable]
    public class Dictionary<K, V> : PDictionary
    {
        public string KType, VType;
        public List<K> _keys = new List<K>();
        public List<V> _values = new List<V>();
        System.Collections.Generic.Dictionary<K, V> dictionary = new System.Collections.Generic.Dictionary<K, V>();

        /// <summary>
        /// <i>Конструктор:</i> Получает типы K V и записывает их в строки. Берёт данные из _keys, _values (Их Unity сохраняет) и записывает в System.Collections.Generic.Dictionary
        /// </summary>
        public Dictionary()
        {
            UpdateTypes();
            UpdateDictionary();
        }
        /// <summary>
        /// Получает типы K V и записывает их в строки.
        /// </summary>
        public void UpdateTypes()
        {
            KType = typeof(K).FullName.ToString();
            VType = typeof(V).FullName.ToString();
        }
        /// <summary>
        /// Загружает данные из _keys и _values в System.Collections.Generic.Dictionary. Проверяет на Null и повторение данных. 
        /// </summary>
        public void UpdateDictionary()
        {
            dictionary.Clear();
            for (int i = 0; i < _keys.Count; i++)
            {
                if (_keys[i] != null)
                {
                    if (!dictionary.ContainsKey(_keys[i]))
                    {
                        dictionary.Add(_keys[i], _values[i]);
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
        /// Метод нужен для добаления пустых элементов в листы. (Нужно для добалвения новых данных типа Object так как они могут быть null) 
        /// </summary>
        public void AddNull(K key, V value)
        {
            _keys.Add(key);
            _values.Add(value);
        }
        /// <summary>
        /// Удаления элемента из _keys и _value с номером id.
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
        /// Удаление элемента по ключу.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(K key)
        {
            UpdateTypes();
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }
            UpdateTV();
        }

        /// <summary>
        /// Добавление элемента.
        /// </summary>
        public void Add(K key, V value)
        {
            UpdateTypes();
            if (_keys.Count != 0 && dictionary.Count == 0)
            {
                UpdateDictionary();
            }
            if (!_keys.Contains(key))
            {
                dictionary.Add(key, value);
            }
            UpdateTV();
        }
        /// <summary>
        /// Обновляет данные в List`s и записывает в них данные из System.Collections.Generic.Dictionary 
        /// </summary>
        public void UpdateTV()
        {
            _keys.Clear();
            _values.Clear();
            foreach (var item in dictionary)
            {
                _keys.Add(item.Key);
                _values.Add(item.Value);
            }
        }

        public V this[K index]
        {
            get
            {
                if (dictionary.Count == 0)
                {
                    UpdateDictionary();
                }
                if (dictionary.ContainsKey(index))
                {
                    return dictionary[index];
                }
                else
                {
                    Debug.LogError($"\"{index}\" not exist in dictionary");
                    return default(V);
                }
            }

            set
            {
                if (dictionary.Count == 0)
                {
                    UpdateDictionary();
                }
                dictionary[index] = value;
            }
        }
    }
}