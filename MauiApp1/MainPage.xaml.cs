#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Svg.Maui;

namespace MauiApp1;
public class Item
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public SvgDrawable? Drawable { get; set; }
}

public partial class MainPage : ContentPage
{
    private SvgDrawable? _drawable;

    public MainPage()
	{
		InitializeComponent();

        // var name = "MauiApp1.Resources.Svg.__AJ_Digital_Camera.svg";
        // var name = "MauiApp1.Resources.Svg.__tiger.svg";
        // var name = "MauiApp1.Resources.Svg.SVG_logo.svg";
        // var name = "MauiApp1.Resources.Svg.pservers-grad-01-b.svg";
        // var assembly = GetType().GetTypeInfo().Assembly;
        // _drawable = SvgDrawable.CreateFromResource(name, assembly);
        // if (_drawable?.Picture is { })
        // {
        //     graphicsView.Drawable = _drawable;
        //     graphicsView.WidthRequest = _drawable.Picture.Width;
        //     graphicsView.HeightRequest = _drawable.Picture.Height;
        // }

        var fullPath = @"c:\DOWNLOADS\GitHub\\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg";
        var files = Directory.GetFiles(fullPath, "*.svg").Select(x =>
        {
            return new Item()
            {
                Name = Path.GetFileNameWithoutExtension(x),
                Path = x,
                Drawable = null
            };
        });

        listView.ItemsSource = files;

        listView.ItemSelected += ListView_ItemSelected;

        listView.ItemTapped += ListView_ItemTapped;
    }

    private void ListView_ItemTapped(object? sender, ItemTappedEventArgs e)
    {
        var item = e.Group as Item;
        if (item?.Path is { })
        {
            var drawable = item.Drawable;
            if (drawable is null)
            {
                var stream = File.OpenRead(item.Path);
                if (stream is { })
                {
                    drawable = SvgDrawable.CreateFromStream(stream);
                    item.Drawable = drawable;
                }
            }

            if (drawable?.Picture is { })
            {
                graphicsView.Drawable = drawable;
                graphicsView.WidthRequest = drawable.Picture.Width;
                graphicsView.HeightRequest = drawable.Picture.Height;
                graphicsView.Invalidate();
            }
        }
    }

    private void ListView_ItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {

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
