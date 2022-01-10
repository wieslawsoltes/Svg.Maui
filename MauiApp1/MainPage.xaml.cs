using Microsoft.Maui.Controls;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    private SvgDrawable? _drawable;

	public MainPage()
	{
		InitializeComponent();

        //var name = "MauiApp1.Resources.Svg.__AJ_Digital_Camera.svg";
        var name = "MauiApp1.Resources.Svg.__tiger.svg";
        //var name = "MauiApp1.Resources.Svg.SVG_logo.svg";
        var assembly = GetType().GetTypeInfo().Assembly;

        _drawable = SvgDrawable.CreateFromResource(name, assembly);
        if (_drawable is { })
        {
            graphicsView.Drwable = _drawable;
            graphicsView.WidthRequest = _drawable.Picture.Width;
            graphicsView.HeightRequest = _drawable.Picture.Height;
        }
    }
}
