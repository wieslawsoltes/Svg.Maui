#nullable enable
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using ShimSkiaSharp;
using Svg.Model;

namespace Svg.Maui;

public class MauiAssetLoader : IAssetLoader
{
	public SKImage LoadImage(Stream stream)
	{
        var image = PlatformImage.FromStream(stream);
        return new SKImage
		{
			Data = ImageExtensions.AsBytes(image),
			Width = (float)image.Width,
			Height = (float)image.Height
		};
	}
}
