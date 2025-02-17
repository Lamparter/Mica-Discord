using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Riverside.DiscordClient;

public class RowGrid : CustomGrid
{
    public static readonly DependencyProperty FillWidthProperty = DependencyProperty.Register(
        "FillWidth",
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
    public static void SetFillWidth(UIElement element, bool value) => element.SetValue(FillWidthProperty, value);
    public static bool GetFillWidth(UIElement element) => (bool)element.GetValue(FillWidthProperty);

    protected override Size MeasureOverride(Size availableSize)
    {
        IEnumerable<(UIElement Element, GridType GridType, double GridValue)> definition = ChildrenAsElements.Select(Element => (Element, GetGridType(Element), GetGridValue(Element)));

        double TotalPixel = definition.Where(x => x.GridType == GridType.Pixel).Select(x =>
        {
            x.Element.Measure(new Size(availableSize.Width, x.GridValue));
            return x.Element.DesiredSize.Height;
        }).Sum();

        double TotalAuto = 0;
        foreach (var x in definition.Where(x => x.GridType == GridType.Auto))
        {
            x.Element.Measure(new Size(availableSize.Width, availableSize.Height - TotalAuto - TotalPixel));
            TotalAuto += x.Element.DesiredSize.Height;
        }

        double TotalStar = definition.Sum(x => x.GridType == GridType.Star ? x.GridValue : 0);

        double Star = TotalStar == 0 ? 0 : (availableSize.Height - TotalPixel - TotalAuto) / TotalStar;

        if (Star < 0) Star = 0;

        foreach (var x in definition.Where(x => x.GridType == GridType.Star))
        {
            x.Element.Measure(new Size(availableSize.Width, x.GridValue * Star));
        }


        var Width = GetFillWidth(this) ? availableSize.Width : Math.Min(Children.Count == 0 ? 0 : ChildrenAsElements.Max(x => x.DesiredSize.Width), availableSize.Width);

        return new Size(Width, Star > 0 ? availableSize.Height : TotalPixel + TotalAuto);
    }

    protected override Size ArrangeOverride(Size finalSize)
        => Arranging(ChildrenAsElements, finalSize);

    protected virtual Size Arranging(IEnumerable<UIElement> Children, Size finalSize)
    {
        IEnumerable<(UIElement Element, GridType GridType, double GridValue)> definition = Children.Select(Element => (Element, GetGridType(Element), GetGridValue(Element)));

        double TotalPixel = definition.Where(x => x.GridType == GridType.Pixel).Select(x =>
        {
            x.Element.Measure(new Size(finalSize.Width, x.GridValue));
            return x.Element.DesiredSize.Height;
        }).Sum();

        double TotalAuto = 0;
        foreach (var x in definition.Where(x => x.GridType == GridType.Auto))
        {
            x.Element.Measure(new Size(finalSize.Width, finalSize.Height - TotalAuto - TotalPixel));
            TotalAuto += x.Element.DesiredSize.Height;
        }

        double TotalStar = definition.Sum(x => x.GridType == GridType.Star ? x.GridValue : 0);

        double Star = TotalStar == 0 ? 0 : (finalSize.Height - TotalPixel - TotalAuto) / TotalStar;

        if (Star < 0) Star = 0;

        foreach (var x in definition.Where(x => x.GridType == GridType.Star))
        {
            x.Element.Measure(new Size(finalSize.Width, x.GridValue * Star));
        }
        
        if (Star < 0) Star = 0;
        
        IEnumerable<(UIElement Element, double Width)> RowHeight = definition.Select(x =>
            (
                x.Element,
                x.GridType switch
                {
                    GridType.Auto => x.Element.DesiredSize.Height,
                    GridType.Pixel => x.GridValue,
                    GridType.Star => Star * x.GridValue,
                    _ => 0,
                }
            )
        );

        double Y = 0;
        foreach (var (Element, Height) in RowHeight)
        {
            Element.Arrange(new Rect(0, Y, finalSize.Width, Height));
            Y += Height;
        }

        return finalSize;
    }

    //public RowGrid AddChild(RowDefinition RowDefinition, UIElement Element)
    //{
    //    SetRowDefinition(Element, RowDefinition);
    //    Children.Add(Element);
    //    return this;
    //}
    //public RowGrid AddChild(GridLength GridLength, UIElement Element)
    //    => AddChild(new RowDefinition { Height = GridLength }, Element);
}
