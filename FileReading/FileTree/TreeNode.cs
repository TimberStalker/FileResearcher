using FileReading.FileTree.Access;
using FileReading.FileTree.Routing;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using System.Collections;
using System.Diagnostics;
using System.Xml;

namespace FileReading.FileTree;
[DebuggerDisplay("{Name}[{Children.Count}] - {Depth}^{Level}")]
public abstract class TreeNode : IEnumerable<TreeNode>
{
    List<TreeNode> children;

    public string Name { get; set; } = "";
    public Guid Id { get; private init; }
    public TreeRoot Root { get; set; }
    public TreeNode? Parent { get; set; }
    public IReadOnlyList<TreeNode> Children => children;
    public TreeRoute ChildRoute { get; set; }

    public TreePath Path => TreePath.CreatePathTo(this);
    public int Size => 1 + Children.Sum(c => c.Size);
    public int Depth => GetDepthFrom(Root);
    public int Level => (Parent?.Level ?? 0) + 1;

    public event Action<TreeNode>? LocationChanged;
    public TreeNode(Guid id, TreeRoot root)
    {
        Id = id;
        Root = root;
        children = new List<TreeNode>();
        ChildRoute = new TreeRoute(this);
    }
    public TreeNode(Guid id, TreeRoot root, TreeRoute childRoute)
    {
        Id = id;
        Root = root;
        children = new List<TreeNode>();
        ChildRoute = childRoute;
    }
    public abstract void ToXml(XmlWriter writer);
    public int GetDepthFrom(TreeNode node)
    {
        var path = TreePath.CreatePathBetween(node, this);
        int depth = 0;
        for (int i = 0; i < path.Length - 1; i++)
        {
            foreach (var child in path[i].Node.Children)
            {
                if (child == path[i + 1].Node)
                {
                    depth++;
                    break;
                }
                depth += child.Size;
            }
        }
        return depth;
    }
    public virtual void RemoveChild(TreeNode node)
    {
        children.Remove(node);
        node.Parent = null;
        node.LocationChanged?.Invoke(this);
    }
    public virtual void RemoveChildAt(int index)
    {
        var node = children[index];
        children.RemoveAt(index);
        node.Parent = null;
        node.LocationChanged?.Invoke(this);
    }
    public virtual void AddChild(TreeNode node)
    {
        children.Add(node);
        node.Parent = this;
        node.LocationChanged?.Invoke(this);
    }
    public virtual void InsertChild(int index, TreeNode node)
    {
        children.Insert(index, node);
        node.Parent = this;
        node.LocationChanged?.Invoke(this);
    }

    public abstract DataValue ReadBytes(DataValue parent, ByteWindow byteWindow, AccessStack accessStack);
    public IEnumerator<TreeNode> GetEnumerator() => Children.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
