#nullable enable
using System;
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Svg.Maui;

Logger.RegisterService(new LoggingService());
GraphicsPlatform.RegisterGlobalService(SkiaGraphicsService.Instance);
Fonts.Register(new SkiaFontService("", ""));

var fullPath = Path.GetFullPath(@"..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg");
var files = Directory.GetFiles(fullPath, "*.svg");

foreach (var path in files)
{
    var name = Path.GetFileNameWithoutExtension(path);
    Console.WriteLine(name);
    try
    {
        SvgDrawable.Convert(path, name + ".png");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
