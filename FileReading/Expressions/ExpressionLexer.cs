using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions;
public class ExpressionLexer
{
    public static List<Token> GetTokens(string text)
    {
        var tokens = new List<Token>();

        var window = new TextWindow(text);

        while(!window.HasReachedEnd)
        {
            switch(window.Peek()[0])
            {
                case ' ':
                    {
                        var value = window.SkipWhitespace(out Range range);
                        tokens.Add(new Token(TokenBase.Whitespace, value.ToString(), range));
                    }
                    break;
                case >= '0' and <= '9':
                    {
                        var value = window.ReadWhile(out Range range, c => char.IsLetterOrDigit(c));
                        if (value.ToArray().Any(v => char.IsLetter(v)))
                        {
                            tokens.Add(new Token(TokenBase.Alphanumeric, value.ToString(), range));
                        }
                        else
                        {
                            tokens.Add(new Token(TokenBase.Numeric, value.ToString(), range));
                        }
                    }
                    break;
                case (>= 'a' and <= 'z') or (>= 'A' and <= 'Z'):
                    {
                        var value = window.ReadWhile(out Range range, c => char.IsLetterOrDigit(c));
                        if (value.ToArray().Any(v => char.IsDigit(v)))
                        {
                            tokens.Add(new Token(TokenBase.Alphanumeric, value.ToString(), range));
                        }
                        else
                        {
                            tokens.Add(new Token(TokenBase.Alphabet, value.ToString(), range));
                        }
                    }
                    break;
                case '\'':
                    {
                        window.Read(out var r1);
                        tokens.Add(new Token(TokenBase.SingleQuote, "'", r1));

                        var slashOrText = window.Read(out var slashorTextRange);
                        if(slashOrText[0] == '\\')
                        {
                            tokens.Add(new Token(TokenBase.BackSlash, slashOrText.ToString(), slashorTextRange));

                            var chartext = window.Read(out var textRange);
                            tokens.Add(new Token(TokenBase.String, chartext.ToString(), textRange));
                        }
                        else
                        {
                            tokens.Add(new Token(TokenBase.String, slashOrText.ToString(), slashorTextRange));
                        }

                        window.Read(out var r2);
                        tokens.Add(new Token(TokenBase.SingleQuote, "'", r2));
                    }
                    break;
                case '"':
                    {
                        window.Read(out var r1);
                        tokens.Add(new Token(TokenBase.DoubleQuote, "\"", r1));

                        var value = window.ReadWhile(out Range range, c => c!='"');
                        tokens.Add(new Token(TokenBase.String, value.ToString(), range));

                        window.Read(out var r2);
                        tokens.Add(new Token(TokenBase.DoubleQuote, "\"", r2));
                    }
                    break;
                case '.':
                    {
                        if(window.Peek(2).SequenceEqual(new char[] { '.', '.' }))
                        {
                            tokens.Add(new Token(TokenBase.DoubleDot, window.Read(out var range, 2).ToString(), range));
                        }
                        else
                        {
                            tokens.Add(new Token(TokenBase.Dot, window.Read(out var range).ToString(), range));
                        }
                    }
                    break;
                case ',':
                case '&':
                case '|':
                case ':':
                case ';':
                case '\x1A':
                case '\\':
                case '*' or '/' or '+' or '-' or '^':
                case '(' or '[' or '{' or '<' or ')' or ']' or '}' or '>':
                    {
                        var value = window.Read(out Range range, 1);
                        tokens.Add(new Token(value[0] switch
                        {
                            '(' => TokenBase.OpenParenthesis,
                            '[' => TokenBase.OpenSquareBracket,
                            '{' => TokenBase.OpenCurlyBracket,
                            '<' => TokenBase.OpenAngledBracket,
                            ')' => TokenBase.ClosedParenthesis,
                            ']' => TokenBase.ClosedSquareBracket,
                            '}' => TokenBase.ClosedCurlyBracket,
                            '>' => TokenBase.ClosedAngledBracket,
                            ',' => TokenBase.Comma,
                            '*' => TokenBase.Asterisk,
                            '/' => TokenBase.ForwardSlash,
                            '+' => TokenBase.Plus,
                            '-' => TokenBase.Dash,
                            '^' => TokenBase.Caret,
                            '&' => TokenBase.Ampersand,
                            '|' => TokenBase.Pipe,
                            ':' => TokenBase.Colon,
                            ';' => TokenBase.Semicolon,
                            '\x1A' => TokenBase.Substitute,
                            '\\' => TokenBase.BackSlash,
                            _ => throw new NotImplementedException()
                        }, value.ToString(), range));
                    }
                    break;
            }
        }

        tokens.Add(new Token(TokenBase.EOF, "", text.Length..text.Length));
        return tokens;
    }
    ref struct TextWindow
    {
        ReadOnlySpan<char> characters;
        int pointer;
        public bool HasReachedEnd => pointer >= characters.Length;
        public TextWindow(string text)
        {
            characters = text.AsSpan();
            pointer = 0;
        }

        public ReadOnlySpan<char> Read(out Range range, int size = 1)
        {
            range = pointer..(pointer += size);
            return characters[range];
        }
        public ReadOnlySpan<char> Peek(int size = 1)
        {
            return characters[pointer..(pointer + size)];
        }
        public ReadOnlySpan<char> ReadWhile(out Range range, Func<char, bool> function)
        {
            int start = pointer;
            while (!HasReachedEnd && function(characters[pointer]))
                pointer++;

            range = start..pointer;
            return characters[range];
        }

        public ReadOnlySpan<char> SkipWhitespace(out Range range)
        {
            return ReadWhile(out range, c => c == ' ');
        }
    }
}
