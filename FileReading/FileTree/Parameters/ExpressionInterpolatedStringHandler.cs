using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static FileReading.FileTree.TreeBuilder;

namespace FileReading.FileTree.Parameters;
[InterpolatedStringHandler]
public class ExpressionInterpolatedStringHandler
{
    StringBuilder builder;

    int referenceIndex;
    public FileTreeExpressionParameter[] references;

    public IReadOnlyList<FileTreeExpressionParameter> Refrences => references;

    public ExpressionInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        builder = new StringBuilder(literalLength + formattedCount);
        references = new FileTreeExpressionParameter[formattedCount];
    }

    public void AppendLiteral(string s)
    {
        builder.Append(s);
    }

    public void AppendFormatted(FileTreeExpressionParameter t)
    {
        builder.Append('\x1A');
        references[referenceIndex++] = t;
    }
    
    public void AppendFormatted(TreeBuilder t, params FileTreeExpressionParameter[] alignment)
    {
        builder.Append('\x1A');
        references[referenceIndex++] = t;
    }

    public string GetTextPart() => builder.ToString();
}
