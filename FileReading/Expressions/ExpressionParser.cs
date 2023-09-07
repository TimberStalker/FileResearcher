using FileReading.Expressions.Operands;
using FileReading.Expressions.Operands.BinaryOperands;
using FileReading.ReadingData.Types;
using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReading.Expressions;
public ref struct ExpressionParser
{
    public Span<Token> Tokens { get; }
    int position;

    public Token Current => Tokens[position];

    public IEnumerator<DataValue> Values { get; }

    public ExpressionParser(List<Token> tokens, IEnumerable<DataValue> values)
    {
        Tokens = tokens.Where(t => t.TokenBase != TokenBase.Whitespace).ToArray();
        position = 0;
        Values = values.GetEnumerator();
    }
    public void Dispose() => Values.Dispose();
    #region Eat
    public void Eat(TokenBase tokenBase)
    {
        if (Current == tokenBase)
            position++;
        else throw new Exception();
    }
    public void Eat(TokenBase tokenBase, out Token token)
    {
        token = Current;
        if (Current == tokenBase)
            position++;
        else throw new Exception();
    }
    public bool TryEat(TokenBase tokenBase)
    {
        if (Current == tokenBase)
        {
            position++;
            return true;
        }
        return false;
    }
    public bool TryEat(TokenBase tokenBase, out Token token)
    {
        token = Current;
        if (Current == tokenBase)
        {
            position++;
            return true;
        }
        return false;
    }
    #endregion

    public Operand Expression()
    {
        return AddSubtract();
    }
    public Operand AddSubtract()
    {
        var result = MultiplyDivide();

        Token token;
        while (TryEat(TokenBase.Plus, out token) || TryEat(TokenBase.Dash, out token))
        {
            result = token.TokenBase switch {
                TokenBase.Plus => new AddOperand(result, MultiplyDivide()),
                TokenBase.Dash => new SubtractOperand(result, MultiplyDivide()),
                _ => throw new Exception()
            };
        }

        return result;
    }
    public Operand MultiplyDivide()
    {
        var result = Indexer();

        Token token;
        while (TryEat(TokenBase.Asterisk, out token) || TryEat(TokenBase.ForwardSlash, out token))
        {
            result = token.TokenBase switch
            {
                TokenBase.Asterisk => new MultiplyOperand(result, Indexer()),
                TokenBase.ForwardSlash => new DivideOperand(result, Indexer()),
                _ => throw new Exception()
            };
        }

        return result;
    }
    public Operand Indexer()
    {
        var result = Ranged();

        while(TryEat(TokenBase.OpenSquareBracket))
        {
            var indexer = Expression();
            Eat(TokenBase.ClosedSquareBracket);
            result = new IndexerOperand(result, indexer);
        }

        return result;
    }
    public Operand Ranged()
    {
        var result = Final();

        if(TryEat(TokenBase.DoubleDot))
        {
            var result2 = Final();
            result = new RangeValueOperand(result, result2);
        }

        return result;
    }
    public Operand Final()
    {
        if (Current == TokenBase.Numeric || Current == TokenBase.Dot) return ParseNumber();
        if (TryEat(TokenBase.SingleQuote))
        {
            bool special = TryEat(TokenBase.BackSlash);
            Eat(TokenBase.String, out var stringToken);
            Eat(TokenBase.SingleQuote);
            if (special)
            {
                char.Parse("");
                return new ValueOperand(new PrimitiveDataValue<char>(CharDataType.Instance, stringToken.Text[0] switch
                {
                    'n' => '\n',
                    't' => '\t',
                    'r' => '\r',
                    var c => c,
                }));
            }
            else
            {
                return new ValueOperand(new PrimitiveDataValue<char>(CharDataType.Instance, stringToken.Text[0]));
            }
        }
        if(TryEat(TokenBase.Substitute))
        {
            Values.MoveNext();
            return new ValueOperand(Values.Current);
        }
        if (TryEat(TokenBase.DoubleQuote))
        {
            Eat(TokenBase.String, out var stringToken);
            Eat(TokenBase.DoubleQuote);

            return new ValueOperand(new PrimitiveDataValue<string>(StringDataType.Instance, stringToken.Text));
        }
        else if (Current == TokenBase.OpenParenthesis)
        {
            Eat(TokenBase.OpenParenthesis);
            var result = Expression();
            Eat(TokenBase.ClosedParenthesis);
            return result;
        }

        throw new Exception();
    }
    public Operand ParseNumber()
    {
        bool hasIPart = TryEat(TokenBase.Numeric, out var iPart);
        DataValue? result;
        if(TryEat(TokenBase.Dot))
        {
            bool hasFPart = TryEat(TokenBase.Numeric, out var fPart);
            StringBuilder builder = new StringBuilder();

            if (hasIPart) builder.Append(iPart.Text);
            builder.Append('.');
            if (hasFPart) builder.Append(fPart.Text);

            var text = builder.ToString();

            if(FloatDataType.Instance.TryParse(text, out result))
            {
                return new ValueOperand(result);
            } else if (DoubleDataType.Instance.TryParse(text, out result))
            {
                return new ValueOperand(result);
            }

            throw new Exception();
        }

        if(ByteDataType.Instance.TryParse(iPart.Text, out result))
        {
            return new ValueOperand(result);
        } else if (ShortDataType.Instance.TryParse(iPart.Text, out result))
        {
            return new ValueOperand(result);
        }
        else if (IntDataType.Instance.TryParse(iPart.Text, out result))
        {
            return new ValueOperand(result);
        }
        else if (LongDataType.Instance.TryParse(iPart.Text, out result))
        {
            return new ValueOperand(result);
        }

        throw new Exception();
    }

}
