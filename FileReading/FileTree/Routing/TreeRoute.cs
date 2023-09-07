using FileReading.ReadingData.Types;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree.Routing;
[DebuggerDisplay("{Node} - {HasParameters}")]
public readonly struct TreeRoute
{
    public readonly TreeNode Node { get; }
    public readonly IReadOnlyList<DataType> Parameters { get; }
    public readonly IReadOnlyList<TreeNode> Children { get; }

    public readonly bool HasParameters => Parameters.Count > 0;
    
    public TreeRoute(TreeNode node)
    {
        Node = node;
        Children = Node.Children;
        Parameters = Array.Empty<DataType>();
    }
    public TreeRoute(TreeNode node, IReadOnlyList<TreeNode> children)
    {
        Node = node;
        Children = children;
        Parameters = Array.Empty<DataType>();
    }
    public TreeRoute(TreeNode node, IReadOnlyList<TreeNode> children, params DataType[] parameters)
    {
        Node = node;
        Children = children;
        Parameters = parameters;
    }

    public static bool operator ==(TreeRoute route1, TreeRoute route2) => route1.Node == route2.Node;
    public static bool operator !=(TreeRoute route1, TreeRoute route2) => route1.Node != route2.Node;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TreeRoute route && route.Node == Node && route.Parameters == Parameters;
    public override int GetHashCode() => base.GetHashCode();
}
