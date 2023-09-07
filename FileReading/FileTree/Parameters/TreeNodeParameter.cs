using FileReading.FileTree.Access;
using FileReading.FileTree.Routing;
using FileReading.ReadingData.Types.Parameters;
using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.FileTree.Parameters;

public class TreeNodeParameter : ITreeParameter
{
	public TreeNode NodeReference { get; }
    public TreeNode ParentNode { get; }
	public IParameter BaseParameter { get; }
    
	public TreePath Path => TreePath.CreatePathBetween(ParentNode, NodeReference);

    public IReadOnlyList<TreeParameter> ReferenceParameters { get; }

    IReadOnlyList<ITreeParameter> ITreeParameter.ReferenceParameters => ReferenceParameters.Cast<ITreeParameter>().ToImmutableList();

    public TreeNodeParameter(TreeNode parentNode, IParameter baseParameter, TreeNode nodeReference)
	{
		NodeReference = nodeReference;
		ParentNode = parentNode;
		BaseParameter = baseParameter;
		ReferenceParameters = Path.Where(n => n.HasParameters).Select(n => new TreeParameter(parentNode, new CustomBaseParameter(n.Node.Name, n.Parameters[0]))).ToImmutableList();
	}

    public DataValue GetValue(AccessStack accessStack)
	{
		var path = Path;
		DataValue current = accessStack[path.Reverse-1].Value;

		try
		{
			using var paramEnum = ReferenceParameters.GetEnumerator();
			foreach (var route in path)
			{
				if(route.HasParameters)
				{
					paramEnum.MoveNext();
					var currentParameter = paramEnum.Current;
					current = current.Children.First(c => c.ValueEquals(currentParameter.GetValue(accessStack)));
				}
				else
				{
					current = current.Children.First(c => c.Metadata.Get<TreeNode>("node") == route.Node);
				}
			}
		}
		catch
		{
			current = DataValue.Empty;
		}
		return current;
    }

    public string GetXmlText()
	{
		var builder = new StringBuilder();
		builder.Append('{');
		builder.Append(NodeReference.Name);
        builder.Append(string.Join(", ", ReferenceParameters.Select(p => $"({p.GetXmlText()})")));
        builder.Append('}');
		return builder.ToString();
    }
}
