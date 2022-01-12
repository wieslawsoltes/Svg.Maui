#nullable enable
using System;
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Svg.Maui;

Initialize();

var fullPath = Path.GetFullPath(@"..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg");
var files = Directory.GetFiles(fullPath, "*.svg");

foreach (var path in files)
{
    var name = Path.GetFileNameWithoutExtension(path);
    Console.WriteLine(name);
    try
    {
        Convert(path, name + ".png");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

static void Initialize()
{
    Logger.RegisterService(new LoggingService());
    GraphicsPlatform.RegisterGlobalService(SkiaGraphicsService.Instance);
    Fonts.Register(new SkiaFontService("", ""));
}

static SvgDrawable? Open(string path)
{
    var stream = File.OpenRead(path);
    if (stream is null)
    {
        return null;
    }

    var drawable = SvgDrawable.CreateFromStream(stream);
    if (drawable?.Picture is null)
    {
        return null;
    }

    return drawable;
}

static void Save(string path, SvgDrawable? drawable)
{
    if (drawable?.Picture is null)
    {
        return;
    }

    var width = drawable.Picture.Width;
    var height = drawable.Picture.Height;
    var bmp = GraphicsPlatform.CurrentService.CreateBitmapExportContext((int)width, (int)height);
    if (bmp is null)
    {
        return;
    }

    var dirtyRect = new RectangleF(0, 0, width, height);

    drawable.Draw(bmp.Canvas, dirtyRect);

    bmp.WriteToFile(path);
}

static void Convert(string inputPath, string outputPath)
{
    var drawable = Open(inputPath);
    if (drawable is not null)
    {
        Save(outputPath, drawable);
    }
}
