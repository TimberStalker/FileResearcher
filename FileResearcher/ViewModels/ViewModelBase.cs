using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void ChangedProperty(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    public void Changed(in object value, [CallerArgumentExpression("value")] string propName = "") => ChangedProperty(propName);
    public void Changed(params string[] properties)
    {
        foreach (var prop in properties)
        {
            ChangedProperty(prop);
        }
    }
}
