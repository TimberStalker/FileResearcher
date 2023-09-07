using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions;
[Flags]
public enum TokenBase
{
    Whitespace = 0,

    Alphabet = 1 << 1,
    Numeric = 1 << 2,
    String = 1 << 3,

    Alphanumeric = Alphabet | Numeric,

    Dot = 1 << 4,
    Comma = 1 << 5,

    Open = 1 << 6,
    Closed = 1 << 7,

    Colon = 1 << 8,
    Semicolon = 1 << 9,

    Asterisk = 1 << 10,
    Slash = 1 << 11,
    Plus = 1 << 12,
    Dash = 1 << 13,
    Percent = 1 << 14,
    Caret = 1 << 15,
    Pound = 1 << 16,
    Equals = 1 << 17,

    ForwardSlash = Slash | Open,
    BackSlash = Slash | Closed,

    Ampersand = 1 << 18,
    Pipe = 1 << 19,

    Parenthesis = 1 << 20,
    SquareBracket = 1 << 21,
    CurlyBracket = 1 << 22,
    AngledBracket = 1 << 23,

    OpenParenthesis = Parenthesis | Open,
    ClosedParenthesis = Parenthesis | Closed,

    OpenSquareBracket = SquareBracket | Open,
    ClosedSquareBracket = SquareBracket | Closed,

    OpenCurlyBracket = CurlyBracket | Open,
    ClosedCurlyBracket = CurlyBracket | Closed,

    OpenAngledBracket = AngledBracket | Open,
    ClosedAngledBracket = AngledBracket | Closed,
    
    Substitute = 1 << 27,
    DoubleDot = 1 << 28,

    SingleQuote = 1 << 29,
    DoubleQuote = 1 << 30,

    EOF = 1 << 31,
}
