using FileReading.FileTree;
using FileReading.ReadingData.Types.Primitives;
using FileResearcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileResearcher.Controls.DebugModels;
public class ReadTreeDebugModel : ViewModelBase
{
	public TreeRoot TreeRoot { get; }
    public System.Drawing.Color DrawingColor => Color.FromArgb(0x238b2b);
    public System.Windows.Media.Color MediaColor => System.Windows.Media.Color.FromRgb(0x23, 0x8b, 0x2b);
    public ReadTreeDebugModel()
	{
        var builder = new TreeBuilder("Root");

        builder.AddChild(StringDataType.Instance, "Text");
        builder.AddChild(FloatDataType.Instance, "Num");
        var arr = builder.AddChild(ArrayDataType.Instance, "Arr");

        arr.AddChild(IntDataType.Instance, "ArrItem");

        var child2 = builder.AddChild(IntDataType.Instance, "Int");

        child2.AddChildReference(IntDataType.Instance, "i1", child2);
        child2.AddChild(BoolDataType.Instance, "b1");

        builder.AddChild(StringDataType.Instance, "Text2");

        TreeRoot = builder.BuildTree();
    }
}
