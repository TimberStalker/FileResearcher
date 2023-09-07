using FileReading.ReadingData.Values;
using FileResearcher.Controls.Overlays;
using FileResearcher.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FileResearcher.Controls;
/// <summary>
/// Interaction logic for ByteViewer.xaml
/// </summary>
public partial class ByteViewer : UserControl
{
    int scrollValue = 0;

    public static int ByteWidth => 16;
    public int[] Offsets => (int[])GetValue(OffsetsProperty.DependencyProperty);
    protected override bool HandlesScrolling => true;
    public int ScrollValue
    {
        get => scrollValue;
        set
        {
            scrollValue = value;

        }
    }
    public byte[] Bytes
    {
        get => (byte[])GetValue(BytesProperty);
        set => SetValue(BytesProperty, value);
    }
    public DataValue[] DataValues
    {
        get { return (DataValue[])GetValue(DataValuesProperty); }
        set { SetValue(DataValuesProperty, value); }
    }

    public static readonly DependencyProperty BytesProperty =
        DependencyProperty.Register("Bytes", typeof(byte[]), typeof(ByteViewer), new FrameworkPropertyMetadata(Array.Empty<byte>(), BytesPropertyChanged));

    private static readonly DependencyPropertyKey OffsetsProperty =
        DependencyProperty.RegisterReadOnly("Offsets", typeof(int[]), typeof(ByteViewer), new FrameworkPropertyMetadata(Array.Empty<int>()));

    public static readonly DependencyProperty DataValuesProperty =
        DependencyProperty.Register("DataValues", typeof(DataValue[]), typeof(ByteViewer), new PropertyMetadata(Array.Empty<DataValue>(), DataValuesPropertyChanged));
    private static void BytesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var bytes = (byte[])e.NewValue;
        int offsetCount = bytes.Length == 0 ? 0 : ((bytes.Length - 1) / ByteWidth) + 1;
        sender.SetValue(OffsetsProperty, Enumerable.Range(0, offsetCount).Select(i => i * ByteWidth).ToArray());
    }

    private static void DataValuesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var viewer = sender as ByteViewer;

        var textBox = viewer.textView;
        var itemPanel = textBox.GetVisualChild<VirtualizingStackPanel>();

        var layer = AdornerLayer.GetAdornerLayer(itemPanel);
        if (layer is not null && e.NewValue is DataValue[] values)
        {
            Adorner[] toRemoveArray = layer.GetAdorners(itemPanel);
            if (toRemoveArray != null)
            {
                for (int x = 0; x < toRemoveArray.Length; x++)
                {
                    layer.Remove(toRemoveArray[x]);
                }
            }
            layer.Add(new DataValueOverlay(values, itemPanel));
        }
    }

    Lazy<ScrollViewer> offsetScrollViewer;
    Lazy<ScrollViewer> textScrollViewer;
    public ByteViewer()
    {
        InitializeComponent();
        offsetScrollViewer = new Lazy<ScrollViewer>(() => VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(offsetView, 0), 0) as ScrollViewer);
        textScrollViewer = new Lazy<ScrollViewer>(() => VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(textView, 0), 0) as ScrollViewer);
    }
    bool updateAdorner;

    private void scrollViews(object sender, ScrollEventArgs e)
    {
        offsetScrollViewer.Value.ScrollToVerticalOffset(e.NewValue);
        textScrollViewer.Value.ScrollToVerticalOffset(e.NewValue);
        updateAdorner = true;
    }
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        var value = mainScroll.Value -= e.Delta / 20;
        offsetScrollViewer.Value.ScrollToVerticalOffset(value);
        textScrollViewer.Value.ScrollToVerticalOffset(value);
        updateAdorner = true;
        e.Handled = true;
    }

    private void textView_LayoutUpdated(object sender, EventArgs e)
    {
        if (updateAdorner)
        {
            textView.GetVisualChild<AdornerLayer>().Update();
            updateAdorner = false;
        }
    }

    private void Grid_MouseMove(object sender, MouseEventArgs e)
    {
        textView.GetVisualChild<AdornerLayer>().Update();
    }
}
