using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Riverside.DiscordClient;

public class ColumnGrid : CustomGrid
{

    public static readonly DependencyProperty FillHeightProperty = DependencyProperty.Register(
        "FillHeight",
        typeof(bool),
        typeof(CustomGrid),
        new PropertyMetadata(
            true,
            new PropertyChangedCallback((obj, evargs) =>
            {
                var Parent = (obj as FrameworkElement)?.Parent as CustomGrid;
                if (Parent == null) return;
            }
        )
        )
    );
    public static void SetFillHeight(UIElement element, bool value) => element.SetValue(FillHeightProperty, value);
    public static bool GetFillHeight(UIElement element) => (bool)element.GetValue(FillHeightProperty);

    protected override Size MeasureOverride(Size availableSize)
    {
        IEnumerable<(UIElement Element, GridType GridType, double GridValue)> definition = ChildrenAsElements.Select(Element => (Element, GetGridType(Element), GetGridValue(Element)));
        
        double TotalPixel = definition.Where(x => x.GridType == GridType.Pixel).Select(x =>
        {
            x.Element.Measure(new Size(x.GridValue, availableSize.Height));
            return x.Element.DesiredSize.Width;
        }).Sum();

        double TotalAuto = 0;
        foreach (var x in definition.Where(x => x.GridType == GridType.Auto))
        {
            x.Element.Measure(new Size(availableSize.Width - TotalAuto - TotalPixel, availableSize.Height));
            TotalAuto += x.Element.DesiredSize.Width;
        }

        double TotalStar = definition.Sum(x => x.GridType == GridType.Star ? x.GridValue : 0);

        double Star = TotalStar == 0 ? 0 : (availableSize.Width - TotalPixel - TotalAuto) / TotalStar;

        if (Star < 0) Star = 0;

        foreach (var x in definition.Where(x => x.GridType == GridType.Star))
        {
            x.Element.Measure(new Size(x.GridValue * Star, availableSize.Height));
        }

        

        var Height = GetFillHeight(this) ? availableSize.Height : Math.Min(Children.Count == 0 ? 0 : ChildrenAsElements.Max(x => x.DesiredSize.Height), availableSize.Height);

        return new Size(Star > 0 ? availableSize.Width : TotalPixel + TotalAuto, Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        IEnumerable<(UIElement Element, GridType GridType, double GridValue)> definition = ChildrenAsElements.Select(Element => (Element, GetGridType(Element), GetGridValue(Element)));

        double TotalPixel = definition.Where(x => x.GridType == GridType.Pixel).Select(x =>
        {
            x.Element.Measure(new Size(x.GridValue, finalSize.Height));
            return x.Element.DesiredSize.Width;
        }).Sum();

        double TotalAuto = 0;
        foreach (var x in definition.Where(x => x.GridType == GridType.Auto))
        {
            x.Element.Measure(new Size(finalSize.Width - TotalAuto - TotalPixel, finalSize.Height));
            TotalAuto += x.Element.DesiredSize.Width;
        }

        double TotalStar = definition.Sum(x => x.GridType == GridType.Star ? x.GridValue : 0);
        
        double Star = TotalStar == 0 ? 0 : (finalSize.Width - TotalPixel - TotalAuto) / TotalStar;

        if (Star < 0) Star = 0;

        foreach (var x in definition.Where(x => x.GridType == GridType.Star))
        {
            x.Element.Measure(new Size(x.GridValue * Star, finalSize.Height));
        }

        IEnumerable<(UIElement Element, double Width)> ColumnWidth = definition.Select(x =>
            (
                x.Element,
                x.GridType switch
                {
                    GridType.Auto => x.Element.DesiredSize.Width,
                    GridType.Pixel => x.GridValue,
                    GridType.Star => Star * x.GridValue,
                    _ => 0,
                }
            )
        );

        double X = 0;
        foreach (var (Element, Width) in ColumnWidth)
        {
            Element.Arrange(new Rect(X, 0, Width, finalSize.Height));
            X += Width;
        }

        return finalSize;
    }
    public ColumnGrid AddChild(GridType GridType, double GridValue, UIElement Element)
    {
        SetGridType(Element, GridType);
        SetGridValue(Element, GridValue);
        Children.Add(Element);
        return this;
    }
    public ColumnGrid AddChild(GridType GridType, UIElement Element)
        => AddChild(GridType: GridType, GridValue: 1, Element: Element);
    public ColumnGrid AddChild(UIElement Element)
        => AddChild(GridType: GridType.Star, Element: Element);
}
