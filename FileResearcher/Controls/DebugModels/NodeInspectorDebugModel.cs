using FileReading.FileTree;
using FileReading.ReadingData.Types.Primitives;
using System;

namespace FileResearcher.Controls.DebugModels;
public class NodeInspectorDebugModel
{
    public TreeDataType DebugType { get; }
    public TreeArray DebugArray { get; }
    public TreeReference DebugReference { get; }
    public NodeInspectorDebugModel()
    {
        var root = new TreeRoot(Guid.NewGuid())
        {
            Name = "Root",
        };

        DebugType = new TreeDataType(Guid.NewGuid(), root, StringDataType.Instance)
        {
            Name = "DataType"
        };
        DebugType.SetParameter(StringDataType.Instance.GetParameter("length"), "4");
        DebugArray = new TreeArray(Guid.NewGuid(), root, ArrayDataType.Instance)
        {
            Name = "Array",
        };
        DebugReference = new TreeReference(Guid.NewGuid(), root, StringDataType.Instance)
        {
            Name = "Reference"
        };
    }
}
