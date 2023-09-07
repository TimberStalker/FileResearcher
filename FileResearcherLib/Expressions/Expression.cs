using FileResearcherLib.DataTypes;
using FileResearcherLib.ReadTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.Expressions;
public static class Expression
{
    public static IExpression FromString(ExpressionStringHandler expressionString)
    {
        throw new NotImplementedException();
    }
    public static IExpression FromConstant(IDataValue constant)
    {
        return new ConstantExpression(constant);
    }
    public static IExpression FromTreeNode(ReadTreeNode node) => new TreeSourceExpression(ReadTreePath.CreatePath(node), node.Root.ValueProvider);

    [InterpolatedStringHandler]
    public ref struct ExpressionStringHandler
    {
        StringBuilder builder;
        public ExpressionStringHandler(int literalLength, int formattedCount)
        {
            builder = new StringBuilder(literalLength);
        }

        public void AppendLiteral(string s)
        {
            builder.Append(s);
        }

        public void AppendFormatted(ReadTreePath path)
        {
        }
        public void AppendFormatted(ReadTreeNode node)
        {
            AppendFormatted(ReadTreePath.CreatePath(node));
        }

        internal string GetFormattedText() => builder.ToString();
    }
}
