#nullable enable
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Svg.Maui;

var name = "paths-data-01-t";
//var path = @$"..\..\..\..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg\{name}.svg";
var path = @$"..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg\{name}.svg";
var stream = File.OpenRead(path);
var drawable = SvgDrawable.CreateFromStream(stream);
if (drawable?.Picture is null)
{
    return;
}

using var bmp = SkiaGraphicsService.Instance.CreateBitmapExportContext((int)drawable.Picture.Width, (int)drawable.Picture.Height);
if (bmp is null)
{
    return;
}

var dirtyRect = new RectangleF(0, 0, drawable.Picture.Width, drawable.Picture.Height);

drawable.Draw(bmp.Canvas, dirtyRect);

bmp.WriteToFile(name + ".png");
