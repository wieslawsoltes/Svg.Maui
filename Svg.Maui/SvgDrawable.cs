#nullable enable
using System.IO;
using Microsoft.Maui.Graphics;
using ShimSkiaSharp;
using Svg.Model;
using System.Reflection;
using System;

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
            var picture = skPicture.Record();
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

    public void Draw(ICanvas canvas, RectangleF dirtyRect)
	{
		_picture?.Draw(canvas);
    }
}
