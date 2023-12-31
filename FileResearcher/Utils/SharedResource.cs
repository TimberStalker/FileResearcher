﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Utils;

public class SharedResource<TValue>
{
    TValue value;
    public event Action<TValue, TValue>? OnValueChanged; 
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
}
