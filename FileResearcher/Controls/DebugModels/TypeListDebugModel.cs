using FileReading.FileTree;
using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Controls.DebugModels;
public class TypeListDebugModel
{
    public List<DataType> DataTypes { get; }

	public TypeListDebugModel()
	{
        DataTypes = PrimitiveDataType.GetTypes().ToList();

        var sectionTreeBuilder = new TreeBuilder("sectionTree");
        sectionTreeBuilder.AddChild<StringDataType>("title", ("length", $"8"));
        sectionTreeBuilder.AddChild<IntDataType>("length");
        sectionTreeBuilder.AddChild<IntDataType>("end");

        var section = new CustomDataType("Section", sectionTreeBuilder.BuildTree())
        {
            CustomStringFormat = "Title:'{title}' - Length:{length} End:{end}"
        };
        DataTypes.Add(section);

        var headerTreeBuilder = new TreeBuilder("headerTree");
        headerTreeBuilder.AddChild<StringDataType>("title", ("length", $"8"));
        headerTreeBuilder.AddChild<IntDataType>("length");
        headerTreeBuilder.AddChild<IntDataType>("end");
        headerTreeBuilder.AddChild<IntDataType>("depth");

        var header = new CustomDataType("Header", headerTreeBuilder.BuildTree())
        {
            CustomStringFormat = "Title:'{title}' - Length:{length} End:{end} Depth:{depth}"
        };
        DataTypes.Add(header);

        var vectorTreeBuilder = new TreeBuilder("vectorTree");
        vectorTreeBuilder.AddChild<FloatDataType>("x");
        vectorTreeBuilder.AddChild<FloatDataType>("y");
        vectorTreeBuilder.AddChild<FloatDataType>("z");

        var vector = new CustomDataType("Vector", vectorTreeBuilder.BuildTree())
        {
            CustomStringFormat = "({x}, {y}, {z})"
        };
        DataTypes.Add(vector);

        var boundingBoxTreeBuilder = new TreeBuilder("boundingBoxTree");
        boundingBoxTreeBuilder.AddChild(vector, "start");
        boundingBoxTreeBuilder.AddChild(vector, "end");

        var boundingBox = new CustomDataType("BoundingBox", boundingBoxTreeBuilder.BuildTree())
        {
            CustomStringFormat = "{start} <> {end}"
        };
        DataTypes.Add(boundingBox);

        var matrixTreeBuilder = new TreeBuilder("matrixTree");
        matrixTreeBuilder.AddChild<FloatDataType>("scaleX");
        matrixTreeBuilder.AddChild(vector, "shear1");
        matrixTreeBuilder.AddChild<FloatDataType>("scaleY");
        matrixTreeBuilder.AddChild(vector, "shear2");
        matrixTreeBuilder.AddChild<FloatDataType>("scaleZ");
        matrixTreeBuilder.AddChild(vector, "position");

        var matrix = new CustomDataType("Matrix", matrixTreeBuilder.BuildTree())
        {
            CustomStringFormat = "Position:{position} - Scale:({scaleX}, {scaleY}, {scaleZ}) - Shear1:{shear1} Shear2:{shear2}"
        };
        DataTypes.Add(matrix);
    }
}
