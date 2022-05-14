using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Utils;

public class ResourcePool<TKey, TValue> : IEnumerable<KeyValuePair<ResourcePool<TKey, TValue>.UniqueResource, TValue>> where TKey : notnull
{
    public TValue this[UniqueResource resource] => items[resource];
    public TValue this[TKey key] => items[resources[key]];

    Dictionary<UniqueResource, TValue> items;
    Dictionary<TKey, UniqueResource> resources;

    public ResourcePool()
    {
        resources = new Dictionary<TKey, UniqueResource>();
        items = new Dictionary<UniqueResource, TValue>();
    }

    public bool TryGetValue(UniqueResource resource, out TValue? value) => items.TryGetValue(resource, out value);
    public bool TryGetValue(TKey key, out TValue? value)
    {
        if(resources.TryGetValue(key, out var resource))
        {
            return TryGetValue(resource, out value);
        }
        value = default;
        return false;
    }

    public bool TryAdd(TKey key, TValue value, out UniqueResource? resource)
    {
        resource = null;
        if(resources.ContainsKey(key))
        {
            return false;
        }

        resource = new UniqueResource(key, resources.ContainsKey);
        resource.OnValueChanged += ResourceKeyChanged;
        resources.Add(key, resource);
        items.Add(resource, value);
        return true;
    }

    private void ResourceKeyChanged(TKey newValue, TKey oldValue)
    {
        resources[newValue] = resources[oldValue];
        resources.Remove(oldValue);
    }

    public bool CanModify(TKey key) => resources.ContainsKey(key);

    public IEnumerator<KeyValuePair<ResourcePool<TKey, TValue>.UniqueResource, TValue>> GetEnumerator() => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public class UniqueResource : SharedResource<TKey>
    {
        protected Func<TKey, bool> CanModify;

        public override TKey Value
        {
            get => value;
            set
            {
                if (CanModify?.Invoke(value) ?? true)
                {
                    base.Value = value;
                }
            }
        }

        internal UniqueResource(TKey value, Func<TKey, bool> canModify) : base(value)
        {
            this.CanModify = canModify;
        }
    }
}
