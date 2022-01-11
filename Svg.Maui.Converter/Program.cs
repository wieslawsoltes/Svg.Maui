#nullable enable
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Svg.Maui;

var name = "__tiger";
//var path = @$"..\..\..\..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg\{name}.svg";
var path = @$"..\..\Svg.Skia\externals\SVG\Tests\W3CTestSuite\svg\{name}.svg";

var stream = File.OpenRead(path);
var drawable = SvgDrawable.CreateFromStream(stream);
if (drawable?.Picture is null)
{
    return;
}

GraphicsPlatform.RegisterGlobalService(SkiaGraphicsService.Instance);
Fonts.Register(new SkiaFontService("", ""));

var width = drawable.Picture.Width;
var height = drawable.Picture.Height;
using var bmp = GraphicsPlatform.CurrentService.CreateBitmapExportContext((int)width, (int)height);
if (bmp is null)
{
    return;
}

var dirtyRect = new RectangleF(0, 0, width, height);

drawable.Draw(bmp.Canvas, dirtyRect);

bmp.WriteToFile(name + ".png");
