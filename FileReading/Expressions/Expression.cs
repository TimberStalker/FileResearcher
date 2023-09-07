using FileReading.FileTree;
using FileReading.ReadingData.Types;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions;
public class Expression
{
	public string Text { get; set; }

	public Expression()
	{
		Text = "";
	}
	public DataValue Evaluate(IEnumerable<DataValue> values)
	{
		var tokens = ExpressionLexer.GetTokens(Text);

		using var parser = new ExpressionParser(tokens, values);
		var operand = parser.Expression();

		return operand.Evaluate();
    }
}
