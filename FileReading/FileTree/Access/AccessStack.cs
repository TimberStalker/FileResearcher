using FileReading.ReadingData.Types;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree.Access;

public class AccessStack
{
    public DataValue Root { get; }
    internal Stack<AccessNode> accessStack = new Stack<AccessNode>();
    internal AccessNode RootNode => accessStack.First();
    internal AccessNode CurrentNode => accessStack.Peek();

    internal AccessNode this[Index index] => accessStack.ElementAt(index.GetOffset(accessStack.Count)); 
    public AccessStack(DataValue root)
    {
        Root = root;
    }
    public void PushNode(TreeNode node, DataValue value)
    {
        accessStack.Push(new AccessNode(node, value));
    }
    public void PushNode(TreeNode node, DataValue value, DataValue key)
    {
        accessStack.Push(new AccessNode(node, value, key));
    }
    public void PopNode()
    {
        accessStack.Pop();
    }
}
