using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Controls;

namespace FileResearcher.Controls.Overlays;
public class DragObjectOverlay : Adorner
{
    Size renderSize;

    RenderTargetBitmap imagePreview;

    public Vector startOffset;
    public Point DrawPos
    {
        get { return (Point)GetValue(DrawPosProperty); }
        set { SetValue(DrawPosProperty, value); }
    }

    public static readonly DependencyProperty DrawPosProperty =
        DependencyProperty.Register("DrawPos", typeof(Point), typeof(DragObjectOverlay), new FrameworkPropertyMetadata(new Point()) { AffectsRender = true });

    public DragObjectOverlay(Vector startOffset, UIElement adornedElement) : base(adornedElement)
    {
        renderSize = adornedElement.DesiredSize;
        IsHitTestVisible = false;
        imagePreview = new RenderTargetBitmap((int)renderSize.Width + 5, (int)renderSize.Height, 96, 96, PixelFormats.Default);
        imagePreview.Render(AdornedElement);
    }
    protected override void OnRender(DrawingContext drawingContext)
    {
        //drawingContext.DrawText(new FormattedText(DrawPos.ToString(),
        //    System.Globalization.CultureInfo.CurrentCulture,
        //    FlowDirection.LeftToRight,
        //    new Typeface("Cascadia Mono"),
        //    12,
        //    new SolidColorBrush(Color.FromRgb(0, 0, 0)),
        //    1.25),
        //    new Point(0, 0));

        var brush = new ImageBrush(imagePreview)
        {
            Opacity = 0.5
        };

        drawingContext.DrawRectangle(brush, new Pen(), new Rect(DrawPos - new Vector(10, renderSize.Height/2), renderSize));
    }
}
