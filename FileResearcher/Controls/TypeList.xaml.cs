using FileReading.FileTree;
using FileReading.ReadingData.Types;
using FileReading.ReadingData.Types.Primitives;
using FileResearcher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileResearcher.Controls;
/// <summary>
/// Interaction logic for TypeList.xaml
/// </summary>
public partial class TypeList : UserControl
{
    public Command<NodeDragWrapper.CreateDragNodeEventArgs> CreateDragNode { get; }

    public List<DataType> DataTypes
    {
        get { return (List<DataType>)GetValue(DataTypesProperty); }
        set { SetValue(DataTypesProperty, value); }
    }

    public static readonly DependencyProperty DataTypesProperty =
        DependencyProperty.Register("DataTypes", typeof(List<DataType>), typeof(TypeList), new FrameworkPropertyMetadata(new List<DataType>()) { AffectsRender = true });

    public TypeList()
    {
        CreateDragNode = new(e =>
        {
            var dataType = e!.CreationArgs as DataType;
            if(dataType is ArrayDataType a)
            {
                e.Node = new TreeArray(Guid.NewGuid(), null!, a);
            }
            else
            {
                e.Node = new TreeDataType(Guid.NewGuid(), null!, dataType!);
            }
        });
        InitializeComponent();
    }
}
