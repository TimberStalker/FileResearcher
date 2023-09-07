using FileReading.FileTree;
using FileReading.ReadingData.Types;
using FileResearcher.Utils;
using System;
using System.Collections;
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
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileResearcher.Controls
{
    /// <summary>
    /// Interaction logic for ReadTree.xaml
    /// </summary>
    public partial class ReadTree : UserControl, INotifyPropertyChanged
    {
        public event Action<object>? SelectedItemChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        [Bindable(true)]
        public TreeRoot? TreeRoot
        {
            get => (TreeRoot)GetValue(TreeRootProperty);
            set => SetValue(TreeRootProperty, value);
        }
        public Command<NodeDragWrapper.CreateDragNodeEventArgs> CreateDragNode { get; }
        public TreeNode? SelectedItem => (TreeNode?)Tree.SelectedItem;


        public static readonly DependencyProperty TreeRootProperty =
            DependencyProperty.Register("TreeRoot", typeof(TreeRoot), typeof(ReadTree), new FrameworkPropertyMetadata(null));

        public ReadTree()
        {
            InitializeComponent();
            CreateDragNode = new Command<NodeDragWrapper.CreateDragNodeEventArgs>(e =>
            {
                e!.Effects = DragDropEffects.Move;
                e.Node = e.CreationArgs as TreeNode;
            });
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Tree.SelectedItemChanged += (s, e) =>
            {
                SelectedItemChanged?.Invoke(this);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItem"));
            };
        }

        //protected override void OnDragOver(DragEventArgs e)
        //{
        //    base.OnDragOver(e);
        //    e.Effects = DragDropEffects.None;

        //    if (e.Data.GetDataPresent(typeof(DataType)))
        //    {
        //        e.Effects = DragDropEffects.Copy;
        //    }

        //    e.Handled = true;
        //}

        //protected override void OnDrop(DragEventArgs e)
        //{
        //    base.OnDrop(e);
        //}
        //protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        //{
        //    base.OnGiveFeedback(e);

        //    if (e.Effects.HasFlag(DragDropEffects.Copy))
        //    {
        //        Mouse.SetCursor(Cursors.Cross);
        //    }
        //    else
        //    {
        //        Mouse.SetCursor(Cursors.No);
        //    }

        //    e.Handled = true;
        //}

        //private void TreeViewItem_DragOver(object sender, DragEventArgs e)
        //{

        //}

        //private void TreeViewItem_Drop(object sender, DragEventArgs e)
        //{

        //}

        //private void TreeViewItem_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DataObject data = new DataObject(typeof(TreeNode), ((TreeViewItem)sender).DataContext);

        //        DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
        //    }

        //    e.Handled = true;
        //}

        //private void TreeViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        //{

        //}
    }
}
