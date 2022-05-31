using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Utils;

public class SharedResource<TValue> : IDisposable
{
    TValue value;
    bool isStale;
    public event Action<TValue, TValue>? OnValueChanged; 

    public bool IsStale => isStale;
    public virtual TValue Value
    {
        get => value;
        set
        {
            OnValueChanged?.Invoke(this.value, this.value = value);
        }
    }

    public SharedResource(TValue value)
    {
        this.value = value;
    }

    public void Dispose()
    {
        isStale = true;
    }
}
