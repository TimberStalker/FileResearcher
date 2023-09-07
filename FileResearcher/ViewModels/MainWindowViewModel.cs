using FileReading.FileTree;
using FileReading.FileTree.Parameters;
using FileReading.ReadingData.Types;
using FileReading.ReadingData.Types.Primitives;
using FileReading.ReadingData.Values;
using FileReading.Utils;
using FileResearcher.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace FileResearcher.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    int byteOffset = 0;
    public byte[] Bytes { get; set; }
    public TreeRoot ReadTree { get; set; }
    public List<DataType> DataTypes { get; set; }

    public DataValue[] DataValues { get; set; }

    public ICommand SetBytesFromFileCommand { get; }
    public ICommand ReadBytesWithTree { get; }
    
    public DataValue[] FlattenedValues => DataValues.SelectMany(d => d.Flatten()).ToArray();


    public MainWindowViewModel()
    {
        SetBytesFromFileCommand = new Command(SetBytesFromFile);
        ReadBytesWithTree = new Command(() =>
        {
            DataValues = ReadTree.ReadBytes(new ByteWindow(Bytes)).ToArray();
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChangedProperty(nameof(DataValues));
                ChangedProperty(nameof(FlattenedValues));
            });
        });
        Bytes = File.ReadAllBytes(@"C:\Users\ChrisG\Desktop\BlurModified\models\vehicles\corvette_c3_rat.model");

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

        ReadTree = treeBuilder.BuildTree();
        DataValues = Array.Empty<DataValue>();
    }

    private void SetBytesFromFile()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();

        if (openFileDialog.ShowDialog() == true)
        {
            using var stream = openFileDialog.OpenFile();
            stream.Read(Bytes = new byte[stream.Length], 0, Bytes.Length);
        }
        Application.Current.Dispatcher.Invoke(() =>
        {
            ChangedProperty("Bytes");
        });
    }
}
