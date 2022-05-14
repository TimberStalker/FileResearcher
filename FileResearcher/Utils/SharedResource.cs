using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Utils
{
    public class SharedResource<T>
    {
        protected T value;
        public virtual T Value
        {
            get => value;
            set
            {
                var oldValue = this.value;
                this.value = value;
                OnValueChanged?.Invoke(this.value, oldValue);
            }
        }

        public event Action<T, T>? OnValueChanged;

        protected SharedResource(T value)
        {
            this.value = value;
        }

        public static SharedResource<T> Create(T value)
        {
            return new SharedResource<T>(value);
        }

        public static explicit operator T(SharedResource<T> resource)
        {
            return resource.Value;
        }
    }
}
