using FileResearcherLib.DataTypes;
using FileResearcherLib.Expressions;
using FileResearcherLib.ReadTrees;
using FileResearcherLib.Utils;
using System;
using System.Text;

namespace FileResearcherLibTester;

public class UnitTest1
{
    [Fact]
    public void Test2()
    {
        Assert.True(true);
    }
    [Fact]
    public void Test1()
    {
        var int32Type = new AtomicDataType<int>("Int32", (window, parameters) =>
        {
            return window.ReadBytes(4, BitConverter.ToInt32);
        });

        var floatType = new AtomicDataType<float>("Float", (window, parameters) =>
        {
            return window.ReadBytes(4, BitConverter.ToSingle);
        });

        var stringType = new AtomicDataType<string>("String")
        {
            ReadParameters =
            {
                new ReadParameter("length", int32Type, () => Expression.FromConstant(int32Type.CreateValue(0))).GetRef(out var p_StringLength)
            },
            ReadBytesFunctor = (str, window, parameters) =>
            {
                if (parameters.TryGetValue(p_StringLength, out var lengthExpression))
                {
                    if(lengthExpression.Evaluate() is AtomicDataValue<int> value)
                    {
                        int length = value.Value;
                        return Encoding.ASCII.GetString(window.ReadBytes(length).bytes);
                    }
                    throw new Exception("String length parameter must be an int.");
                }
                throw new Exception("String must have a length");
            }
        };

        var cStringType = new AtomicDataType<string>("CString", (window, parameters) =>
        {
            List<byte> bytes = new List<byte>();
            var read = window.ReadBytes(1); 
            while (read.bytes[0] != 0x0)
            {
                bytes.Add(read.bytes[0]);
                read = window.ReadBytes(1);
            }
            return Encoding.ASCII.GetString(bytes.ToArray());
        });

        var vector3Type = new CompoundDataType()
        {
            Name = "Vector3",
            ReadTree = new ReadTreeNode("Vector3")
            {
                ChildNodes =
                {
                    new ReadTreeNode("x") { Reader = new DataTypeReader(floatType) },
                    new ReadTreeNode("y") { Reader = new DataTypeReader(floatType) },
                    new ReadTreeNode("z") { Reader = new DataTypeReader(floatType) }
                }
            }
        };

        var readTree = new ReadTreeNode()
        {
            ChildNodes =
            {
                new ReadTreeNode("length", out var strLengthNode)
                {
                    Reader = new DataTypeReader(int32Type)
                },
                new ReadTreeNode("text1")
                {
                    Reader = new DataTypeReader(stringType)
                    {
                        Parameters =
                        {
                            { p_StringLength, Expression.FromTreeNode(strLengthNode) }
                        }
                    }
                },
                new ReadTreeNode("text2")
                {
                    Reader = new DataTypeReader(stringType)
                    {
                        Parameters =
                        {
                            { p_StringLength, Expression.FromConstant(int32Type.CreateValue(5)) }
                        }
                    }
                }
            }
        };

        var window = new ByteWindow("\x5\0\0\0HelloWorld\0"u8.ToArray());

        var result = readTree.ReadBytes(window);
        Assert.Equal(3, result.Children.Count);
        var intPart = result.Children[0];
        if(intPart.Self is AtomicDataValue<int> i)
        {
            Assert.Equal(5, i.Value);
        }
        else
        {
            Assert.Fail($"Not int value. Instead is {intPart.GetType().Name}");
        }

        var stringPart = result.Children[1];
        if(stringPart.Self is AtomicDataValue<string> s)
        {
            Assert.Equal("Hello", s.Value);
        }
        else
        {
            Assert.Fail("Not string value.");
        }

        var stringPart2 = result.Children[2];
        if(stringPart2.Self is AtomicDataValue<string> s2)
        {
            Assert.Equal("World", s2.Value);
        }
        else
        {
            Assert.Fail("Not string value.");
        }
    }
}