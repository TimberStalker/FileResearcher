using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions;
[DebuggerDisplay("{TokenBase} {Text} {Range}")]
public struct Token
{
    public TokenBase TokenBase { get; }
    public string Text { get; }
    public Range Range { get; }

    public Token(TokenBase tokenBase, string text, Range range)
    {
        TokenBase = tokenBase;
        Text = text;
        Range = range;
    }

    public static bool operator ==(Token token, TokenBase tokenBase)
    {
        return token.TokenBase == tokenBase;
    }
    public static bool operator !=(Token token, TokenBase tokenBase)
    {
        return token.TokenBase != tokenBase;
    }
    public static int operator ^(Token token, TokenBase tokenBase)
    {
        return (int)token.TokenBase ^ (int)tokenBase;
    }
}
