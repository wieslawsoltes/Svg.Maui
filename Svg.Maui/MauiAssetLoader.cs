#nullable enable
using Microsoft.Maui.Graphics;
using System.IO;
using ShimSkiaSharp;
using Svg.Model;

namespace Svg.Maui;

public class MauiAssetLoader : IAssetLoader
{
	public SKImage LoadImage(Stream stream)
	{
		var image = GraphicsPlatform.CurrentService.LoadImageFromStream(stream);
		return new SKImage
		{
			Data = ImageExtensions.AsBytes(image),
			Width = (float)image.Width,
			Height = (float)image.Height
		};
	}
}
