using FileReading.ReadingData.Values;
using FileReading.FileTree;
using FileReading.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReading.ReadingData.Types.Parameters;
using System.Drawing;

namespace FileReading.ReadingData.Types;

public class CustomDataType : DataType
{
    string name;
    Color color;

    public string CustomStringFormat { get; set; } = "";
    public override string Name => name;
    public TreeRoot FileTree { get; }
    public override DataValue Zero => new CustomDataValue(this, FileTree.Children.Select(n => ((TreeDataType)n).ReadType.Zero).ToArray());
    public bool HasCustomStringFormat => !string.IsNullOrEmpty(CustomStringFormat);
    public override Color Color => color;
    public CustomDataType(string name, TreeRoot fileTree) : this(Guid.NewGuid(), name, fileTree) { }
    public CustomDataType(Guid id, string name, TreeRoot fileTree) : base(id)
    {
        FileTree = fileTree;
        this.name = name;
        color = Color.FromArgb(0xbc, 0xbc, 0xbc);
    }

    public void SetName(string name) => this.name = name;
    public void SetColor(Color color) => this.color = color;

    public override DataValue Read(ByteWindow window, IReadOnlyDictionary<Parameter, DataValue> parameters)
    {
        var reads = FileTree.ReadBytes(window).ToArray();

        var result = new CustomDataValue(this, reads);

        return result;
    }

    public override bool TryParse(string input, out DataValue? value)
    {
        throw new NotImplementedException();
    }
}
