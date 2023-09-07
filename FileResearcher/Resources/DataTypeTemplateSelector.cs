using FileReading.FileTree;
using FileReading.ReadingData.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileResearcher.Resources;
public class DataTypeTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        var element = container as FrameworkElement;

        if (element != null && item != null && item is CustomDataType)
        {
            return (DataTemplate)element.FindResource("Custom");
        }
        else if(element != null && item != null && item is DataType)
        {
            return (DataTemplate)element.FindResource("Default");
        }

        return new DataTemplate();
    }
}
