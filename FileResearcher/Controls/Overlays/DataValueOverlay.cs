using FileReading.ReadingData.Values;
using FileResearcher.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FileResearcher.Controls.Overlays;
public sealed class DataValueOverlay : Adorner
{
    Point hoverPoint = new Point(-1, -1);
    public DataValueOverlay(DataValue[] values, UIElement adornedElement) : base(adornedElement)
    {
        Values = values;
        notFoundHit = new Flag();
    }

    public DataValue[] Values { get; }
    Flag notFoundHit;
    protected override void OnRender(DrawingContext drawingContext)
    {
        notFoundHit.Set();
        try
        {
            if (AdornedElement is ListViewItem item)
            {
                var offset = VisualTreeHelper.GetOffset(item);
                var text = (TextBlock)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(item, 0), 0);
                var rect = new Rect(text.DesiredSize);
                rect.Offset(offset);
                foreach (var value in Values)
                {
                    DrawFromListView(drawingContext, value, 0, rect);
                }
            }
            else if (AdornedElement is VirtualizingStackPanel panel)
            {
                var listview = panel.GetVisualParent<ListView>();

                var topItem = panel.GetVisualChild<ListViewItem>(0);
                int topIndex = listview.ItemContainerGenerator.IndexFromContainer(topItem);

                var offset = VisualTreeHelper.GetOffset(topItem);
                var text = (TextBlock)VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(topItem, 0), 0);
                var rect = new Rect(text.DesiredSize);
                rect.Offset(offset);

                foreach (var value in Values)
                {
                    DrawFromListView(drawingContext, value, topIndex * 16, rect);
                }
            }
        }
        catch
        {

        }
    }
    private void DrawFromListView(DrawingContext drawingContext, DataValue value, int ignoreBefore, Rect upperRect)
    {
        if (!value.Metadata.TryGet<Range>("range", out var range)) return;

        int start = range.Start.Value;
        int end = range.End.Value - 1;

        if (start < ignoreBefore) return;

        start -= ignoreBefore;
        end -= ignoreBefore;

        var startRect = new Rect(upperRect.Location, upperRect.Size);
        startRect.Scale(1 / 16d, 1);
        startRect.Offset(startRect.Width * (start % 16), startRect.Height * (start / 16));

        var endRect = new Rect(upperRect.Location, upperRect.Size);
        endRect.Scale(1 / 16d, 1);
        endRect.Offset(endRect.Width * (end % 16), endRect.Height * (end / 16));

        if (endRect.Bottom < 0) return;

        PathFigure figure;
        if (start / 16 == end / 16)
        {
            figure = new PathFigure(startRect.TopLeft, new PathSegment[]
            {
                        new LineSegment(endRect.TopRight, true),
                        new LineSegment(endRect.BottomRight, true),
                        new LineSegment(startRect.BottomLeft, true),
            }, true);
        }
        else
        {
            var right = upperRect.Right;
            var left = upperRect.Left;

            figure = new PathFigure(startRect.TopLeft, new PathSegment[]
            {
                        new LineSegment(new Point(right, startRect.Top), true),
                        new LineSegment(new Point(right, endRect.Top), true),
                        new LineSegment(endRect.TopRight, true),
                        new LineSegment(endRect.BottomRight, true),
                        new LineSegment(new Point(left, endRect.Bottom), true),
                        new LineSegment(new Point(left, startRect.Bottom), true),
                        new LineSegment(startRect.BottomLeft, true),
            }, true);
        }

        var geometery = new PathGeometry(new PathFigure[]
        {
                    figure
        });

        var color = value.BaseType.Color;
        if (geometery.FillContains(Mouse.GetPosition(this)) && notFoundHit.Consume())
        {
            drawingContext.DrawGeometry(new SolidColorBrush(Color.FromArgb(0x46, color.R, color.G, color.B)), new Pen(new SolidColorBrush(Color.FromArgb(0xFF, color.R, color.G, color.B)), 1), geometery);

            //drawingContext.DrawText(
            //    new FormattedText(
            //        value.ToString(), 
            //        System.Globalization.CultureInfo.CurrentCulture, 
            //        FlowDirection.LeftToRight, 
            //        new Typeface("Cascadia Mono"), 
            //        12, 
            //        new SolidColorBrush(Color.FromArgb(0xFF, 0, 0, 0)), 
            //        1), 
            //    geometery.Bounds.TopLeft);
        }
        else
        {
            drawingContext.DrawGeometry(new SolidColorBrush(Color.FromArgb(0x26, color.R, color.G, color.B)), new Pen(new SolidColorBrush(Color.FromArgb(0xE6, color.R, color.G, color.B)), 1), geometery);
        }
    }
}
