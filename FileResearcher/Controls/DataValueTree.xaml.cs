using FileReading.ReadingData.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileResearcher.Controls;
/// <summary>
/// Interaction logic for DataValueTree.xaml
/// </summary>
public partial class DataValueTree : UserControl
{
    public IEnumerable<DataValue> Values
    {
        get { return (IEnumerable<DataValue>)GetValue(ValuesProperty); }
        set { SetValue(ValuesProperty, value); }
    }


    public static readonly DependencyProperty ValuesProperty =
        DependencyProperty.Register("Values", typeof(IEnumerable<DataValue>), typeof(DataValueTree), new PropertyMetadata(Enumerable.Empty<DataValue>()));


    public DataValueTree()
    {
        InitializeComponent();
    }
}
