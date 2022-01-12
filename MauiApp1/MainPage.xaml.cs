#nullable enable
using System;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Svg.Maui;

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
        //var name = "MauiApp1.Resources.Svg.pservers-grad-01-b.svg";
        var assembly = GetType().GetTypeInfo().Assembly;

        _drawable = SvgDrawable.CreateFromResource(name, assembly);
        if (_drawable?.Picture is { })
        {
            graphicsView.Drawable = _drawable;
            graphicsView.WidthRequest = _drawable.Picture.Width;
            graphicsView.HeightRequest = _drawable.Picture.Height;
        }
    }

    private async void ButtonOpen_Clicked(object sender, EventArgs args)
    {
        var file = await FilePicker.PickAsync();
        if (file is { })
        {
            var stream = await file.OpenReadAsync();
            if (stream is { })
            {
                _drawable = SvgDrawable.CreateFromStream(stream);
                if (_drawable?.Picture is { })
                {
                    graphicsView.Drawable = _drawable;
                    graphicsView.WidthRequest = _drawable.Picture.Width;
                    graphicsView.HeightRequest = _drawable.Picture.Height;
                }
            }
        }
    }
}
