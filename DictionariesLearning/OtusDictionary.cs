using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionariesLearning
{
    public class OtusDictionary
    {
        private const int Size = 32;

        private KeyValuePair?[] _items = new KeyValuePair[Size];

        private class KeyValuePair
        {
            public int Key { get; }
            public string Value { get; }

            public KeyValuePair(int key, string value)
            {
                Key = key;
                Value = value;
            }
        }

        public void Add(int key, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Значение не может быть null.");
            }

            int index = GetIndex(key, _items.Length);

            if (_items[index] != null)
            {
                ResizeAndRehash();

                index = GetIndex(key, _items.Length);
            }

            _items[index] = new KeyValuePair(key, value);
        }

        public string Get(int key)
        {
            int index = GetIndex(key, _items.Length);

            if (_items[index] == null || _items[index]!.Key != key)
            {
                throw new KeyNotFoundException("Ключ не найден.");
            }

            return _items[index]!.Value;
        }

        private int GetIndex(int key, int size)
        {
            return key % size;
        }

        private void ResizeAndRehash()
        {
            int newSize = _items.Length * 2;

            var newItems = new KeyValuePair?[newSize];

            foreach (var item in _items)
            {
                if (item != null)
                {
                    int newIndex = GetIndex(item.Key, newSize);

                    if (newItems[newIndex] != null)
                    {
                        ResizeAndRehash();

                        return;
                    }

                    newItems[newIndex] = item;
                }
            }

            _items = newItems;
        }
    }
}
