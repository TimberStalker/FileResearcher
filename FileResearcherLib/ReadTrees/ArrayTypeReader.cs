using FileResearcherLib.DataTypes;
using FileResearcherLib.Expressions;
using FileResearcherLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcherLib.ReadTrees;
//public class ArrayTypeReader : IByteReader
//{
//    public IDataType DataType { get; }
//    public static ReadParameter ArrayLength { get; } = new ReadParameter("arrayLength", , () => new ConstantExpression(0));
//    public IDictionary<ReadParameter, IExpression> Parameters { get; } = new Dictionary<ReadParameter, IExpression>()
//    {
//        { ArrayLength, ArrayLength.DefaultValueExpression() }
//    };
//    public ArrayTypeReader(IDataType dataType)
//    {
//        DataType = dataType;
//    }

//    public IDataValue ReadBytes(ByteWindow window)
//    {
//        for(int i = 0; i < 0; i++)
//        {
//            DataType.ReadBytes(window);
//        }
//    }
//}
