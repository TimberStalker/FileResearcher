using FileReading.FileTree;
using FileResearcher.Controls.Overlays;
using FileResearcher.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileResearcher.Controls;
/// <summary>
/// Interaction logic for NodeDragWrapper.xaml
/// </summary>
[ContentProperty("InnerContent")]
public partial class NodeDragWrapper : UserControl
{
    private DragObjectOverlay? dragAdorner;

    [Bindable(true)]
    public Visual? AdornerChild
    {
        get { return (Visual?)GetValue(AdornerChildProperty); }
        set { SetValue(AdornerChildProperty, value); }
    }
    [Bindable(true)]
    public Command<CreateDragNodeEventArgs> CreateDraggedNode
    {
        get { return (Command<CreateDragNodeEventArgs>)GetValue(CreateDraggedNodeProperty); }
        set { SetValue(CreateDraggedNodeProperty, value); }
    }
    [Bindable(true)]
    public object CreateNodeArgs
    {
        get { return (object)GetValue(CreateNodeArgsProperty); }
        set { SetValue(CreateNodeArgsProperty, value); }
    }
    [Bindable(true)]
    public ICommand? DragSuccessful
    {
        get { return (ICommand?)GetValue(DragSuccessfulProperty); }
        set { SetValue(DragSuccessfulProperty, value); }
    }
    public object InnerContent
    {
        get { return GetValue(InnerContentProperty); }
        set { SetValue(InnerContentProperty, value); }
    }

    public static readonly DependencyProperty AdornerChildProperty =
        DependencyProperty.Register("AdornerChild", typeof(Visual), typeof(NodeDragWrapper), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty CreateDraggedNodeProperty =
        DependencyProperty.Register("CreateDraggedNode", typeof(Command<CreateDragNodeEventArgs>), typeof(NodeDragWrapper), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty CreateNodeArgsProperty =
        DependencyProperty.Register("CreateNodeArgs", typeof(object), typeof(NodeDragWrapper), new PropertyMetadata(null));

    public static readonly DependencyProperty DragSuccessfulProperty =
        DependencyProperty.Register("DragSuccessful", typeof(ICommand), typeof(NodeDragWrapper), new PropertyMetadata(null));

    public static readonly DependencyProperty InnerContentProperty =
        DependencyProperty.Register("InnerContent", typeof(object), typeof(NodeDragWrapper), new FrameworkPropertyMetadata(null));

    public NodeDragWrapper()
    {
        InitializeComponent();
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed)
        {
            var createNodeArgs = new CreateDragNodeEventArgs(CreateNodeArgs);
            if(CreateDraggedNode?.CanExecute(createNodeArgs) == true)
            {
                e.Handled = true;
                CreateDraggedNode.Execute(createNodeArgs);
                if(createNodeArgs.Node is not null)
                {

                    var dragAdornerLayer = AdornerLayer.GetAdornerLayer(AdornerChild ?? this);
                    dragAdorner = new DragObjectOverlay((Vector)e.GetPosition(this), this);
                    dragAdornerLayer.Add(dragAdorner);

                    var data = new DataObject(createNodeArgs.Node);
                    data.SetData(this);
                    var result = DragDrop.DoDragDrop(this, data, createNodeArgs.Effects);
                    
                    if(result != DragDropEffects.None)
                    {
                        DragSuccessful?.Execute(null);
                    }

                    dragAdornerLayer.Remove(dragAdorner);
                    dragAdorner = null;
                }
            }
        }
    }
    protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
    {
        var mousePos = CursorHelper.GetCurrentCursorPosition(this);
        if(dragAdorner != null)
        {
            dragAdorner.DrawPos = mousePos;
        }
        ;
        if(e.Effects == DragDropEffects.Move)
        {
            Mouse.SetCursor(Cursors.Pen);
        } else if (e.Effects == DragDropEffects.Link)
        {
            Mouse.SetCursor(Cursors.Cross);
        } else
        {
            Mouse.SetCursor(Cursors.Arrow);
        }
        e.Handled = true;
    }
    protected override void OnDragEnter(DragEventArgs e)
    {
    }
    protected override void OnDragOver(DragEventArgs e)
    {
        e.Effects = DragDropEffects.None;
        var source = e.Data.GetData(typeof(NodeDragWrapper)) as NodeDragWrapper;
        if (source != this && AllowDrop)
        {
            e.Effects = source?.AllowDrop == true ? DragDropEffects.Move : DragDropEffects.Link;
        }
        e.Handled = AllowDrop;
    }
    protected override void OnDragLeave(DragEventArgs e)
    {
    }
    protected override void OnDrop(DragEventArgs e)
    {
    }

    public class CreateDragNodeEventArgs : EventArgs
    {
        public object? CreationArgs { get; }
        public TreeNode? Node { get; set; }
        public DragDropEffects Effects { get; set; }
        public CreateDragNodeEventArgs(object? creationArgs)
        {
            CreationArgs = creationArgs;
        }
    }
}
