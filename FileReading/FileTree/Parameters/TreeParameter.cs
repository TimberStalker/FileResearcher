using FileReading.Expressions;
using FileReading.FileTree.Access;
using FileReading.ReadingData.Types.Parameters;
using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace FileReading.FileTree.Parameters;

public class TreeParameter : ITreeParameter
{
    public TreeNode ParentNode { get; }
	public IParameter BaseParameter { get; }

	public string StringValue { get; set; } = "";

	public IList<TreeNodeParameter> ReferenceParameters { get; }

	IReadOnlyList<ITreeParameter> ITreeParameter.ReferenceParameters => ReferenceParameters.Cast<ITreeParameter>().ToImmutableList();

	public TreeParameter(TreeNode parentNode, IParameter baseParameter)
	{
		ParentNode = parentNode;
		BaseParameter = baseParameter;
		ReferenceParameters = new List<TreeNodeParameter>();
	}

    public DataValue GetValue(AccessStack accessStack)
	{
		var expression = new Expression()
		{
			Text = StringValue
		};

		var refParams = ReferenceParameters.Select(p => p.GetValue(accessStack));
		var result = expression.Evaluate(refParams);

		if(result.BaseType.Equals(BaseParameter.DataType))
		{
			return result;
		} else if (BaseParameter.DataType.ConvertableFrom(result.BaseType))
		{
			return BaseParameter.DataType.Convert(result);
		}

		return DataValue.Empty;
    }
	public string GetXmlText()
	{
		int index = 0;
		return Regex.Replace(StringValue, @"\x1A", m => ReferenceParameters[index++].GetXmlText());
	}
	public void ToXml(XmlWriter writer)
	{
		writer.WriteStartElement("Value");

		writer.WriteAttributeString("Expression", GetXmlText());

		writer.WriteEndElement();
	}
}
