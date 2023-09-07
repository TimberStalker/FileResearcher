using FileReading.FileTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileResearcher.Resources;
public class TreeNodeTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        var element = container as FrameworkElement;

        if (element != null && item != null && item is TreeNode)
        {
            return (DataTemplate)element.FindResource(item.GetType().Name);
        }

        return new HierarchicalDataTemplate();
    }
}
