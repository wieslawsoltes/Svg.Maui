#nullable enable
using Svg.Maui;

namespace MauiAppSample
{
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

            var fullPath = @"c:\DOWNLOADS\GitHub\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg";
            //var fullPath = @"c:\Users\Administrator\Documents\GitHub\SVG\Tests\W3CTestSuite\svg\";
            var files = Directory.GetFiles(fullPath, "*.svg").Select(x =>
            {
                return new Item()
                {
                    Name = Path.GetFileNameWithoutExtension(x),
                    Path = x,
                    Drawable = null
                };
            });

            collectionView.ItemsSource = files;

            collectionView.SelectionChanged += CollectionView_SelectionChanged;
        }

        private void CollectionView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as Item;
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
}
