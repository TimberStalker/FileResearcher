using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace FileResearcher.Utils;
public static class TraversalHelpers
{
    public static T GetVisualChild<T>(this DependencyObject parent) where T : notnull, Visual
    {
        T child = default(T);

        int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < numVisuals; i++)
        {
            Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
            child = v as T;
            if (child == null)
            {
                child = GetVisualChild<T>(v);
            }
            if (child != null)
            {
                break;
            }
        }
        return child;
    }
    public static T? GetVisualChild<T>(this DependencyObject parent, Index index) where T : notnull, Visual
    {
        if(index.IsFromEnd)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);
            return VisualTreeHelper.GetChild(parent, index.GetOffset(childCount)) as T;
        }
        else
        {
            return VisualTreeHelper.GetChild(parent, index.Value) as T;
        }
    }
    public static T? GetVisualParent<T>(this DependencyObject child) where T : notnull, Visual
    {
        var parent = child;

        while (parent != null)
        {
            parent = VisualTreeHelper.GetParent(parent);
            if (parent is T value) return value;
        }
        return null;
    }
}
