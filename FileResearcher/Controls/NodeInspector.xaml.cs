using FileReading.FileTree;
using System.Windows;
using System.Windows.Controls;

namespace FileResearcher.Controls;
/// <summary>
/// Interaction logic for NodeInspector.xaml
/// </summary>
public partial class NodeInspector : UserControl
{
    public TreeNode CurrentNode
    {
        get { return (TreeNode)GetValue(CurrentNodeProperty); }
        set { SetValue(CurrentNodeProperty, value); }
    }

    public static readonly DependencyProperty CurrentNodeProperty =
        DependencyProperty.Register("CurrentNode", typeof(TreeNode), typeof(NodeInspector), new FrameworkPropertyMetadata(null));


    public NodeInspector()
    {
        InitializeComponent();
    }
}
