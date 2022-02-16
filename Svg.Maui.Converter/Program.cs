#nullable enable
using System;
using System.IO;
using Microsoft.Maui.Graphics.Skia;
using Svg.Maui;

var fullPath = Path.GetFullPath(args.Length == 1 ? args[0] : "./");
var files = Directory.GetFiles(fullPath, "*.svg");
var bitmapExportService = new PlatformBitmapExportService();

foreach (var path in files)
{
    var name = Path.GetFileNameWithoutExtension(path);
    Console.WriteLine(name);
    try
    {
        SvgDrawable.Convert(path, name + ".png", bitmapExportService);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
