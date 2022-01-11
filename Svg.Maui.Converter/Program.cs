#nullable enable
using System;
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Svg.Maui;

Initialize();

//*
var fullPath = Path.GetFullPath(@"..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg");
Console.WriteLine(fullPath);
var files = Directory.GetFiles(fullPath, "*.svg");
foreach (var path in files)
{
    var name = Path.GetFileNameWithoutExtension(path);
    Console.WriteLine(name);
    try
    {
        Save(path, name + ".png");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        // Console.WriteLine(ex.StackTrace);
    }
}
//*/

/*
var name = "__tiger";
//var path = @$"..\..\..\..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg\{name}.svg";
var path = @$"..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg\{name}.svg";
Save(path, name + ".png");
//*/

static void Save(string inputPath, string outputPath)
{
    var stream = File.OpenRead(inputPath);
    if (stream is null)
    {
        return;
    }

    var drawable = SvgDrawable.CreateFromStream(stream);
    if (drawable?.Picture is null)
    {
        return;
    }

    var width = drawable.Picture.Width;
    var height = drawable.Picture.Height;

    using var bmp = GraphicsPlatform.CurrentService.CreateBitmapExportContext((int)width, (int)height);
    if (bmp is null)
    {
        return;
    }

    var dirtyRect = new RectangleF(0, 0, width, height);

    drawable.Draw(bmp.Canvas, dirtyRect);

    bmp.WriteToFile(outputPath);
}

static void Initialize()
{
    //Logger.RegisterService(new ConsoleLoggingService());
    Logger.RegisterService(new LoggingService());
    GraphicsPlatform.RegisterGlobalService(SkiaGraphicsService.Instance);
    Fonts.Register(new SkiaFontService("", ""));
}

class LoggingService : ILoggingService
{
    void ILoggingService.Log(LogType logType, string message)
    {
        // Console.WriteLine(message);
    }

    void ILoggingService.Log(LogType logType, string message, Exception exception)
    {
        // Console.WriteLine(message);
    }
}
