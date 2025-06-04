namespace OtusDictionary
{
    public class OtusDictionary
    {
        private KeyValuePair[] _storage;
        private const int InitialSize = 32;

        public OtusDictionary()
        {
            _storage = new KeyValuePair[InitialSize];
        }

        public void Add(int key, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Значение не может быть пустым.");
            }

            var index = GetIndex(key);

            if (_storage[index] == null)
            {
                _storage[index] = new KeyValuePair(key, value);
            }
            else if (_storage[index].Key == key)
            {
                throw new ArgumentException("Такой ключ уже существует.");
            }
            else
            {
                ResizeAndRehash(); // вызывается при нахождении коллизии
                Add(key, value);
            }
        }

        public string Get(int key)
        {
            var index = GetIndex(key);

            if (_storage[index] == null || _storage[index].Key != key)
            {
                throw new KeyNotFoundException($"Запрашиваемый ключ не найден.");
            }

            return _storage[index].Value;
        }

        public string this[int key]
        {
            get => Get(key);
            set => Add(key, value);
        }

        private int GetIndex(int key)
        {
            return Math.Abs(key) % _storage.Length;
        }

        private void ResizeAndRehash()
        {
            var oldStorage = _storage;
            _storage = new KeyValuePair[oldStorage.Length * 2];

            foreach (var item in oldStorage)
            {
                if (item != null)
                {
                    int newIndex = GetIndex(item.Key);
                    if (_storage[newIndex] != null)
                    {
                        ResizeAndRehash();
                        return;
                    }
                    _storage[newIndex] = item;
                }
            }
        }

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
    }
}
