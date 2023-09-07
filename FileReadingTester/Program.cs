using FileReading.ReadingData.Types;
using FileReading.ReadingData.Types.Primitives;
using FileReading.FileTree;
using FileReading.Utils;
using System.Text;
using System.Xml;
using FileReading.Expressions;
using FileReading.ReadingData.Values;
using static FileReading.FileTree.TreeBuilder;
using FileReading.FileTree.Parameters;

var sectionTreeBuilder = new TreeBuilder("sectionTree");
sectionTreeBuilder.AddChild<StringDataType>("title", ("length", $"8"));
sectionTreeBuilder.AddChild<IntDataType>("length");
sectionTreeBuilder.AddChild<IntDataType>("end");

var section = new CustomDataType("Section", sectionTreeBuilder.BuildTree())
{
    CustomStringFormat = "Title:'{title}' - Length:{length} End:{end}"
};

var headerTreeBuilder = new TreeBuilder("headerTree");
headerTreeBuilder.AddChild<StringDataType>("title", ("length", $"8"));
headerTreeBuilder.AddChild<IntDataType>("length");
headerTreeBuilder.AddChild<IntDataType>("end");
headerTreeBuilder.AddChild<IntDataType>("depth");

var header = new CustomDataType("Header", headerTreeBuilder.BuildTree())
{
    CustomStringFormat = "Title:'{title}' - Length:{length} End:{end} Depth:{depth}"
};

var vectorTreeBuilder = new TreeBuilder("vectorTree");
vectorTreeBuilder.AddChild<FloatDataType>("x");
vectorTreeBuilder.AddChild<FloatDataType>("y");
vectorTreeBuilder.AddChild<FloatDataType>("z");

var vector = new CustomDataType("Vector", vectorTreeBuilder.BuildTree())
{
    CustomStringFormat = "({x}, {y}, {z})"
};

var boundingBoxTreeBuilder = new TreeBuilder("boundingBoxTree");
boundingBoxTreeBuilder.AddChild(vector, "start");
boundingBoxTreeBuilder.AddChild(vector, "end");

var boundingBox = new CustomDataType("BoundingBox", boundingBoxTreeBuilder.BuildTree())
{
    CustomStringFormat = "Start:{start} | End:{end}"
};

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

//--------------------------------------
//-------------Tree---------------------
//--------------------------------------

var treeBuilder = new TreeBuilder("Base");
var cp = treeBuilder.AddChild<StringDataType>("cp", ("length", $"4"));
var model = cp.AddChild(section, "model");
var modelHeader = model.AddChild(header, "modelHeader");
var mdlDat = model.AddChild(section, "modelData");
var mdlDatHeader = mdlDat.AddChild(header, "modelDataHeader");

var modelsCount = mdlDat.AddChild<IntDataType>("modelsCount");
var elementsCount = mdlDat.AddChild<IntDataType>("modelsCount");

mdlDat.AddChild<IntDataType>("skip");

var modelBoundingBox = mdlDat.AddChild(boundingBox, "modelBoundingBox");

var stringTable = model.AddChild(section, "stringTable");
var stringTableCount = stringTable.AddChild<IntDataType>("stringTableCount");

var stringTableOffsets = stringTable.AddChild<ArrayDataType>("stringTableOffsets", ("length", $"{stringTableCount} + 1"));
var stringTableOffset = stringTableOffsets.AddChild<IntDataType>("stringTableOffset");

var overalOffset = stringTable.AddChildReference<IntDataType>("overalOffset", ("array", stringTableOffset, stringTableCount));

var stringTableStrings = stringTable.AddChild<ArrayDataType>("stringTableStrings", ("length", stringTableCount));
var stringTableString = stringTableStrings.AddChild<StringDataType>("stringTableString", ("nullTerminated", $"true"));
var stringTableNull = stringTableStrings.AddChild<CharDataType>("stringTableNull");

var modelsSection = model.AddChild(section, "modelsSection");

var models = modelsSection.AddChild<ArrayDataType>("models", ("length", modelsCount));
var modelsMatrix = models.AddChild(matrix, "matrix");
var modelsBoundingBox = models.AddChild(boundingBox, "boundingBox");
var modelsNameIndex = models.AddChild<IntDataType>("nameIndex");
var modelsName = models.AddChildReference<StringDataType>("name", (stringTableString, modelsNameIndex));
var modelsModelIndex = models.AddChild<IntDataType>("modelIndex");
var modelsChildElementCount = models.AddChild<IntDataType>("childElementCount");
var modelsHierarchyIndex = models.AddChild<IntDataType>("hierarchyIndex");
models.AddChild<IntDataType>("unknown1");
models.AddChild<IntDataType>("unknown2");
models.AddChild<IntDataType>("unknown3");

var elementsSection = model.AddChild(section, "modelsSection");

var elements = elementsSection.AddChild<ArrayDataType>("elements", ("length", elementsCount));
var elementsModelIndex = elements.AddChild<IntDataType>("modelIndex");
var elementsMatrix = elements.AddChild(matrix, "matrix");
var elementsBoundingBox = elements.AddChild(boundingBox, "boundingBox");
var elementsNameIndex = elements.AddChild<IntDataType>("nameIndex");
var elementsName = elements.AddChildReference<StringDataType>("name", (stringTableString, elementsNameIndex));
var elementsElementIndex = elements.AddChild<IntDataType>("elementsElementIndex");
var elementsParentIndex = elements.AddChild<IntDataType>("childParentIndex");
elements.AddChild<IntDataType>("unknown1");
elements.AddChild<IntDataType>("unknown2");
elements.AddChild<IntDataType>("unknown3");
elements.AddChild<ShortDataType>("unknown4");
elements.AddChild<ShortDataType>("unknown5");

//---------------------------------------
//--------------EndTree------------------
//---------------------------------------
var tree = treeBuilder.BuildTree();

int depth = tree.Children[0].Depth;

byte[] bytes = File.ReadAllBytes(@"C:\Users\ChrisG\Desktop\BlurModified\models\vehicles\corvette_c3_rat.model");
//byte[] bytes = File.ReadAllBytes(@"C:\Users\ChrisG\Desktop\BlurModified\animations\igm_driver_ml.skeleton");

var window = new ByteWindow(bytes);
var result1 = tree.ReadBytes(window).ToArray();
using (var writer = XmlWriter.Create(Console.Out, new XmlWriterSettings { Encoding = Encoding.ASCII, Indent = true, CheckCharacters = false }))
{
    writer.WriteStartDocument();
    writer.WriteStartElement("Tree");

    tree.ToXml(writer);

    writer.WriteEndElement();
    writer.WriteEndDocument();
}
using (var writer = XmlWriter.Create(Console.Out, new XmlWriterSettings { Encoding = Encoding.ASCII, Indent = true, CheckCharacters = false }))
{
    writer.WriteStartDocument();
    writer.WriteStartElement("Root");

    foreach (var item in result1)
    {
        item.ToXml(writer);
    }

    writer.WriteEndElement();
    writer.WriteEndDocument();
}
Console.WriteLine(window.Position.ToString("X"));