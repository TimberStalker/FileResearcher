using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Utils
{
    public class DynamicDictionary<TKey, TValue> : IEnumerable<KeyValuePair<DynamicDictionary<TKey, TValue>.Key, TValue>>, where TKey : notnull
    {
        Dictionary<Key, TValue> values;
        Dictionary<TKey, Key> keys;

        public TValue this[Key key] 
        {
            get => values[key];
            set => values[key] = value;
        }
        public TValue this[TKey key] {
            get => values[keys[key]];
            set => values[keys[key]] = value;
        }

        public int Count => values.Count;

        public DynamicDictionary()
        {
            values = new Dictionary<Key, TValue>();
            keys = new Dictionary<TKey, Key>();
        }
        public DynamicDictionary(params (TKey, TValue)[] addValues)
        {
            values = new Dictionary<Key, TValue>();
            keys = new Dictionary<TKey, Key>();

            foreach (var value in addValues)
            {
                TryAdd(value.Item1, value.Item2);
            }
        }
        public Key GetKey(TKey key) => keys[key];
        public bool TryAdd(TKey key, TValue value)
        {
            if(keys.ContainsKey(key)) return false;

            var realKey = new Key(key, CanSet);
            realKey.OnValueChanged += UpdateKeys;

            keys.Add(key, realKey);
            values.Add(realKey, value);

            return true;
        }

        public void UpdateKeys(TKey oldKey, TKey newKey)
        {
            keys[newKey] = keys[oldKey];
            keys.Remove(oldKey);
        }

        public bool CanSet(TKey key) => !keys.ContainsKey(key);

        public IEnumerator<KeyValuePair<Key, TValue>> GetEnumerator() => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public class Key : SharedResource<TKey>
        {
            Func<TKey, bool> canSet;

            public override TKey Value 
            { 
                get => base.Value;
                set 
                {
                    if (canSet(value))
                        base.Value = value;
                    else
                        throw new InvalidOperationException("Ditionary already contains that key.");
                } 
            }

            internal Key(TKey key, Func<TKey, bool> canSet) : base(key)
            {
                this.canSet = canSet;
            }
        }
    }
}
