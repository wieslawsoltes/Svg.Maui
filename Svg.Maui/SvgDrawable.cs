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
            var picture = LoadSvg(skPicture);
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

    private static IPicture? LoadSvg(SKPicture picture)
    {
        if (picture.Commands is null)
        {
            return null;
        }

        using var canvas = new PictureCanvas(
            picture.CullRect.Left,
            picture.CullRect.Top,
            picture.CullRect.Width,
            picture.CullRect.Height);

        foreach (var canvasCommand in picture.Commands)
        {
            Draw(canvasCommand, canvas);
        }

        return canvas.Picture;
    }

    private static bool IsStroked(SKPaint paint)
    {
        return paint.Style == SKPaintStyle.Stroke || paint.Style == SKPaintStyle.StrokeAndFill;
    }

    private static bool IsFilled(SKPaint paint)
    {
        return paint.Style == SKPaintStyle.Fill || paint.Style == SKPaintStyle.StrokeAndFill;
    }

    private static void Draw(CanvasCommand canvasCommand, ICanvas canvas)
    {
        switch (canvasCommand)
        {
            case ClipPathCanvasCommand clipPathCanvasCommand:
                {
                    var path = clipPathCanvasCommand.ClipPath.ToGeometry(false);
                    if (path is { })
                    {
                        // TODO: clipPathCanvasCommand.Operation;
                        // TODO: clipPathCanvasCommand.Antialias;
                        // TODO: WindingMode
                        canvas.ClipPath(path, WindingMode.NonZero);
                    }
                }
                break;

            case ClipRectCanvasCommand clipRectCanvasCommand:
                {
                    var rect = clipRectCanvasCommand.Rect.ToRect();
                    // TODO: clipRectCanvasCommand.Operation;
                    // TODO: clipRectCanvasCommand.Antialias;
                    canvas.ClipRectangle(rect);
                }
                break;

            case SaveCanvasCommand _:
                {
                    canvas.SaveState();
                }
                break;

            case RestoreCanvasCommand _:
                {
                    canvas.ResetState();
                }
                break;

            case SetMatrixCanvasCommand setMatrixCanvasCommand:
                {
                    // TODO: Workaround becasue ConcatenateTransform throws exception when using with Skia.
                    var matrix = setMatrixCanvasCommand.Matrix.ToMatrix();
                    var M11 = matrix.M11;
                    var M12 = matrix.M12;
                    var M21 = matrix.M21;
                    var M22 = matrix.M22;
                    var M31 = matrix.M31;
                    var M32 = matrix.M32;
                    var rotation = Math.Atan2(M12, M11);
                    var sheary = Math.Atan2(M22, M21) - Math.PI / 2 - rotation;
                    var translationx = M31;
                    var translationy = M32;
                    var scalex = Math.Sqrt(M11 * M11 + M12 * M12);
                    var scaley = Math.Sqrt(M21 * M21 + M22 * M22) * Math.Cos(sheary);

                    // NOTE: scale(x, y) * skew(0, shear) * rotate(angle) * translate(x, y)
                    canvas.Scale((float)scalex, (float)scaley);
                    // TODO: canvas.Skew(0f, sheary);
                    canvas.Rotate((float)rotation);
                    canvas.Translate(translationx, translationy);

                    // TODO: Throws exception when using with Skia.
                    // canvas.ConcatenateTransform(matrix);
                }
                break;

            case SaveLayerCanvasCommand saveLayerCanvasCommand:
                {
                    // TODO:
                }
                break;

            case DrawImageCanvasCommand drawImageCanvasCommand:
                {
                    if (drawImageCanvasCommand.Image is { })
                    {
                        var image = drawImageCanvasCommand.Image.ToBitmap();
                        if (image is { })
                        {
                            // TODO: source
                            var source = drawImageCanvasCommand.Source.ToRect();
                            var dest = drawImageCanvasCommand.Dest.ToRect();
                            // TODO: drawImageCanvasCommand.Paint?.FilterQuality
                            canvas.DrawImage(image, dest.X, dest.Y, dest.Width, dest.Height);
                        }
                    }
                }
                break;

            case DrawPathCanvasCommand drawPathCanvasCommand:
                {
                    if (drawPathCanvasCommand.Path is { } path && drawPathCanvasCommand.Paint is { } paint)
                    {
                        // TODO:
                        //(var brush, var pen) = drawPathCanvasCommand.Paint.ToBrushAndPen();

                        if (path.Commands?.Count == 1)
                        {
                            var pathCommand = path.Commands[0];
                            var success = false;

                            switch (pathCommand)
                            {
                                case AddRectPathCommand addRectPathCommand:
                                    {
                                        var rect = addRectPathCommand.Rect.ToRect();

                                        if (IsFilled(paint))
                                        {
                                            paint.Shader.SetFill(canvas);
                                            canvas.FillRectangle(rect);
                                        }

                                        if (IsStroked(paint))
                                        {
                                            paint.SetStroke(canvas);
                                            canvas.DrawRectangle(rect);
                                        }

                                        success = true;
                                    }
                                    break;

                                case AddRoundRectPathCommand addRoundRectPathCommand:
                                    {
                                        var rect = addRoundRectPathCommand.Rect.ToRect();
                                        var rx = addRoundRectPathCommand.Rx;
                                        var ry = addRoundRectPathCommand.Ry;

                                        // TODO: ry not supported

                                        if (IsFilled(paint))
                                        {
                                            paint.Shader.SetFill(canvas);
                                            canvas.FillRoundedRectangle(rect, rx);
                                        }

                                        if (IsStroked(paint))
                                        {
                                            paint.SetStroke(canvas);
                                            canvas.DrawRoundedRectangle(rect, rx);
                                        }

                                        success = true;
                                    }
                                    break;

                                case AddOvalPathCommand addOvalPathCommand:
                                    {
                                        var rect = addOvalPathCommand.Rect.ToRect();

                                        if (IsFilled(paint))
                                        {
                                            paint.Shader.SetFill(canvas);
                                            canvas.FillEllipse(rect);
                                        }

                                        if (IsStroked(paint))
                                        {
                                            paint.SetStroke(canvas);
                                            canvas.DrawEllipse(rect);
                                        }

                                        success = true;
                                    }
                                    break;

                                case AddCirclePathCommand addCirclePathCommand:
                                    {
                                        var x = addCirclePathCommand.X;
                                        var y = addCirclePathCommand.Y;
                                        var radius = addCirclePathCommand.Radius;

                                        if (IsFilled(paint))
                                        {
                                            paint.Shader.SetFill(canvas);
                                            canvas.FillCircle(x, y, radius);
                                        }

                                        if (IsStroked(paint))
                                        {
                                            paint.SetStroke(canvas);
                                            canvas.DrawCircle(x, y, radius);
                                        }

                                        success = true;
                                    }
                                    break;

                                case AddPolyPathCommand addPolyPathCommand:
                                    {
                                        if (addPolyPathCommand.Points is { })
                                        {
                                            var close = addPolyPathCommand.Close;
                                            var pathF = addPolyPathCommand.Points.ToGeometry(close);
      
                                            if (IsFilled(paint))
                                            {
                                                paint.Shader.SetFill(canvas);
                                                canvas.FillPath(pathF, WindingMode.NonZero);
                                            }

                                            if (IsStroked(paint))
                                            {
                                                paint.SetStroke(canvas);
                                                canvas.DrawPath(pathF);
                                            }

                                            success = true;
                                        }
                                    }
                                    break;
                            }

                            if (success)
                            {
                                break;
                            }
                        }

                        if (path.Commands?.Count == 2)
                        {
                            var pathCommand1 = path.Commands[0];
                            var pathCommand2 = path.Commands[1];

                            if (pathCommand1 is MoveToPathCommand moveTo && pathCommand2 is LineToPathCommand lineTo)
                            {
                                var p1 = new Point(moveTo.X, moveTo.Y);
                                var p2 = new Point(lineTo.X, lineTo.Y);

                                if (IsStroked(paint))
                                {
                                    paint.SetStroke(canvas);
                                    canvas.DrawLine(p1, p2);
                                }
                                break;
                            }
                        }

                        {
                            var pathF = path.ToGeometry();
                            if (pathF is { })
                            {
                                var windingMode = path.FillType.ToWindingMode();

                                if (IsFilled(paint))
                                {
                                    paint.Shader.SetFill(canvas);
                                    canvas.FillPath(pathF, windingMode);
                                }

                                if (IsStroked(paint))
                                {
                                    paint.SetStroke(canvas);
                                    canvas.DrawPath(pathF);
                                }
                            }
                        }
                    }
                }
                break;

            case DrawTextBlobCanvasCommand drawPositionedTextCanvasCommand:
                {
                    // TODO: DrawTextBlobCanvasCommand
                }
                break;

            case DrawTextCanvasCommand drawTextCanvasCommand:
                {
                    /*
                    if (drawTextCanvasCommand.Paint is { })
                    {
                        (var brush, _) = drawTextCanvasCommand.Paint.ToBrushAndPen();
                        var text = drawTextCanvasCommand.Paint.ToFormattedText(drawTextCanvasCommand.Text);
                        var x = drawTextCanvasCommand.X;
                        var y = drawTextCanvasCommand.Y;
                        var origin = new A.Point(x, y - drawTextCanvasCommand.Paint.TextSize);
                        //canvas._commands.Add(new TextDrawCommand(brush, origin, text));
                    }*/
                }
                break;

            case DrawTextOnPathCanvasCommand drawTextOnPathCanvasCommand:
                {
                    // TODO: DrawTextOnPathCanvasCommand
                }
                break;

            default:
                break;
        }
    }

    public void Draw(ICanvas canvas, RectangleF dirtyRect)
	{
		_picture?.Draw(canvas);

        /*
        var p = new SolidPaint()
        {
            BackgroundColor = Colors.Blue,
            ForegroundColor = Colors.Orange
        };
        canvas.SetFillPaint(p, Rectangle.Zero);

       canvas.FillRectangle(dirtyRect);
       canvas.DrawRectangle(dirtyRect);
        */


        //canvas.FillColor = Color.FromRgba(222, 222, 222, 255);
        //canvas.FillRectangle(dirtyRect);



        //canvas.FillColor = Color.FromRgba(255, 0, 0, 255);
        //canvas.FillRectangle(new RectangleF(0, 0, 100, 100));
        //
        //canvas.StrokeColor = Color.FromRgba(0, 0, 255, 255);
        //canvas.StrokeSize = 2;
        //canvas.DrawRectangle(new RectangleF(0, 0, 100, 100));



        /*
		IPattern pattern;

		// Create a 10x10 template for the pattern
		using (PictureCanvas pictureCanvas = new PictureCanvas(0, 0, 10, 10))
		{
			pictureCanvas.StrokeColor = Colors.Blue;
			pictureCanvas.DrawLine(0, 0, 10, 10);
			pictureCanvas.DrawLine(0, 10, 10, 0);

			var picture = pictureCanvas.Picture;
			picture.Draw(canvas);


			pattern = new PicturePattern(picture, 10, 10);
		}
		*/



        /*
		// Fill the rectangle with the 10x10 pattern
		PatternPaint patternPaint = new PatternPaint
		{
			Pattern = pattern
		};
		canvas.SetFillPaint(patternPaint, RectangleF.Zero);
		canvas.FillEllipse(10, 10, 250, 250);
		canvas.DrawEllipse(10, 10, 250, 250);

		*/
        //pattern.Draw(canvas);


        /*

		LinearGradientPaint linearGradientPaint = new LinearGradientPaint
		{
			StartColor = Colors.Yellow,
			EndColor = Colors.Green,
			StartPoint = new Point(0, 0),
			EndPoint = new Point(1, 1)
		};

		linearGradientPaint.AddOffset(0.25f, Colors.Red);
		linearGradientPaint.AddOffset(0.75f, Colors.Blue);

		RectangleF linearRectangle = new RectangleF(10, 10, 200, 100);
		canvas.SetFillPaint(linearGradientPaint, linearRectangle);
		canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
		canvas.FillRoundedRectangle(linearRectangle, 12);
		*/

    }
}
