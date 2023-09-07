using FileReading.ReadingData.Types;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree.Access;

internal class AccessNode
{
	public TreeNode Node { get; }
    public DataValue Value { get; }
    public DataValue? Key { get; }

    [MemberNotNullWhen(true, "Key")] public bool HasKey => Key is not null;
	internal AccessNode(TreeNode node, DataValue value, DataValue? key = null)
	{
		Node = node;
		Value = value;
        Key = key;
	}
}
