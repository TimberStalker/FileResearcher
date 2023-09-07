using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree.Routing;

public readonly struct TreePath : IEnumerable<TreeRoute>
{
    public readonly int Reverse { get; }
    readonly TreeRoute[] Path { get; }

    public readonly int Length => Path.Length;
    public readonly TreeRoute Root => Path[0];
    public readonly TreeRoute Destination => Path[^1];
    public readonly TreeRoute this[int i] => Path[i];

    public TreePath(IEnumerable<TreeRoute> path, int reverse = 0)
    {
        Path = path.ToArray();
        Reverse = reverse;
    }

    public readonly IEnumerator<TreeRoute> GetEnumerator()
    {
        foreach (var item in Path) yield return item;
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static TreePath Empty { get; } = new TreePath(Array.Empty<TreeRoute>());

    static bool FindPath(in TreeNode destination, ref Stack<TreeRoute> stack)
    {
        foreach (var child in stack.Peek().Children)
        {
            stack.Push(child.ChildRoute);
            if (child == destination || FindPath(destination, ref stack))
            {
                return true;
            }
            stack.Pop();
        }
        return false;
    }
    public static TreePath CreatePathTo(TreeNode destination) => CreatePathBetween(destination.Root, destination);
    public static TreePath CreatePathBetween(TreeNode source, TreeNode destination)
    {
        if (source == destination) return new TreePath(Array.Empty<TreeRoute>());

        var destinationPath = new Stack<TreeRoute>();
        destinationPath.Push(destination.Root.ChildRoute);

        FindPath(destination, ref destinationPath);

        if (destinationPath.Any(r => r.Node == source))
        {
            return new TreePath(destinationPath.SkipWhile(x => x.Node != source));
        }

        var sourcePath = new Stack<TreeRoute>();
        sourcePath.Push(source.Root.ChildRoute);

        FindPath(source, ref sourcePath);

        var sourceEnum = sourcePath.Reverse().GetEnumerator();

        var dest = destinationPath.Reverse().SkipWhile(n =>
        {
            sourceEnum.MoveNext();
            var isSame = n == sourceEnum.Current;
            return isSame;
        }).ToArray();

        return new TreePath(dest, sourcePath.Count - (destinationPath.Count - dest.Length));
    }
}
