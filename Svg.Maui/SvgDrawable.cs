#nullable enable
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using ShimSkiaSharp;
using Svg.Model;
using System.Reflection;

namespace Svg.Maui;

public class SvgDrawable : IDrawable
{
	private static readonly IAssetLoader s_assetLoader = new MauiAssetLoader();

	private IPicture? _picture;

    public IPicture? Picture => _picture;

	private SvgDrawable()
    {
    }

    public static SvgDrawable? CreateFromResource(string name, Assembly assembly)
    {
        using var stream = assembly.GetManifestResourceStream(name);
        return CreateFromStream(stream);
    }

    public static SvgDrawable? CreateFromStream(Stream? stream)
    {
        var skPicture = ToLoadModelFromStream(stream);
        if (skPicture is { })
        {
            var picture = skPicture.Record(skPicture.CullRect);
            if (picture is { })
            {
                var drawable = new SvgDrawable { _picture = picture };
                return drawable;
            }
        }

        return null;
    }

    private static SKPicture? ToLoadModelFromStream(Stream? stream)
    {
        if (stream == null)
        {
            return null;
        }

        var document = SvgExtensions.Open(stream);
        return document is { } 
            ? SvgExtensions.ToModel(document, s_assetLoader, out _, out _) 
            : null;
    }

    public static SvgDrawable? Open(string path)
    {
        var stream = File.OpenRead(path);
        if (stream is null)
        {
            return null;
        }

        var drawable = CreateFromStream(stream);
        if (drawable?.Picture is null)
        {
            return null;
        }

        return drawable;
    }

    public static void Save(string path, SvgDrawable? drawable, IBitmapExportService bitmapExportService)
    {
        if (drawable?.Picture is null)
        {
            return;
        }

        var x = drawable.Picture.X;
        var y = drawable.Picture.Y;
        var width = drawable.Picture.Width;
        var height = drawable.Picture.Height;

        var bmp = bitmapExportService.CreateContext((int)width, (int)height);
        if (bmp is null)
        {
            return;
        }

        var dirtyRect = new RectangleF(x, y, width, height);

        drawable.Draw(bmp.Canvas, dirtyRect);

        bmp.WriteToFile(path);
    }

    public static void Convert(string inputPath, string outputPath, IBitmapExportService bitmapExportService)
    {
        var drawable = Open(inputPath);
        if (drawable is not null)
        {
            Save(outputPath, drawable, bitmapExportService);
        }
    }

    public void Draw(ICanvas canvas, RectangleF dirtyRect)
	{
		_picture?.Draw(canvas);
    }
}
