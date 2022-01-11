#nullable enable
using System;
using System.Collections.Generic;
using System.Numerics;
using ShimSkiaSharp;
using Microsoft.Maui.Graphics;

namespace Svg.Maui;

public static class MauiModelExtensions
{
    public static Point Transform(this Matrix3x2 m, Point point)
    {
        return new Point(
            (point.X * m.M11) + (point.Y * m.M21) + m.M31,
            (point.X * m.M12) + (point.Y * m.M22) + m.M32);
    }

    public static Point ToPoint(this SKPoint point)
    {
        return new Point(point.X, point.Y);
    }

    public static Point[] ToPoints(this IList<SKPoint> skPoints)
    {
        var points = new Point[skPoints.Count];

        for (int i = 0; i < skPoints.Count; i++)
        {
            points[i] = skPoints[i].ToPoint();
        }

        return points;
    }

    public static Size ToSize(this SKSize size)
    {
        return new Size(size.Width, size.Height);
    }

    public static RectangleF ToRect(this SKRect rect)
    {
        return new RectangleF(rect.Left, rect.Top, rect.Width, rect.Height);
    }

    public static Matrix3x2 ToMatrix(this SKMatrix matrix)
    {
        // The Persp0, Persp1 and Persp2 are not used.
        return new Matrix3x2(
            matrix.ScaleX,
            matrix.SkewY,
            matrix.SkewX,
            matrix.ScaleY,
            matrix.TransX,
            matrix.TransY);
    }

    public static IImage? ToBitmap(this SKImage image)
    {
        if (image.Data is null)
        {
            return null;
        }
        return GraphicsPlatform.CurrentService.LoadImageFromBytes(image.Data);
    }

    public static LineCap ToPenLineCap(this SKStrokeCap strokeCap)
    {
        switch (strokeCap)
        {
            default:
            case SKStrokeCap.Butt:
                return LineCap.Butt;

            case SKStrokeCap.Round:
                return LineCap.Round;

            case SKStrokeCap.Square:
                return LineCap.Square;
        }
    }

    public static LineJoin ToPenLineJoin(this SKStrokeJoin strokeJoin)
    {
        switch (strokeJoin)
        {
            default:
            case SKStrokeJoin.Miter:
                return LineJoin.Miter;

            case SKStrokeJoin.Round:
                return LineJoin.Round;

            case SKStrokeJoin.Bevel:
                return LineJoin.Bevel;
        }
    }

    public static HorizontalAlignment ToTextAlignment(this SKTextAlign textAlign)
    {
        switch (textAlign)
        {
            default:
            case SKTextAlign.Left:
                return HorizontalAlignment.Left;

            case SKTextAlign.Center:
                return HorizontalAlignment.Center;

            case SKTextAlign.Right:
                return HorizontalAlignment.Right;
        }
    }

    // TODO: IFontStyle.Weight
    /*
    public static AM.FontWeight ToFontWeight(this SKFontStyleWeight fontStyleWeight)
    {
        switch (fontStyleWeight)
        {
            default:
            case SKFontStyleWeight.Invisible:
                // TODO: FontStyleWeight.Invisible
                throw new NotSupportedException();
            case SKFontStyleWeight.Thin:
                return AM.FontWeight.Thin;

            case SKFontStyleWeight.ExtraLight:
                return AM.FontWeight.ExtraLight;

            case SKFontStyleWeight.Light:
                return AM.FontWeight.Light;

            case SKFontStyleWeight.Normal:
                return AM.FontWeight.Normal;

            case SKFontStyleWeight.Medium:
                return AM.FontWeight.Medium;

            case SKFontStyleWeight.SemiBold:
                return AM.FontWeight.SemiBold;

            case SKFontStyleWeight.Bold:
                return AM.FontWeight.Bold;

            case SKFontStyleWeight.ExtraBold:
                return AM.FontWeight.ExtraBold;

            case SKFontStyleWeight.Black:
                return AM.FontWeight.Black;

            case SKFontStyleWeight.ExtraBlack:
                return AM.FontWeight.ExtraBlack;
        }
    }
    */

    public static FontStyleType ToFontStyle(this SKFontStyleSlant fontStyleSlant)
    {
        switch (fontStyleSlant)
        {
            default:
            case SKFontStyleSlant.Upright:
                // TODO: FontStyleSlant.Upright
                return FontStyleType.Normal;

            case SKFontStyleSlant.Italic:
                return FontStyleType.Italic;

            case SKFontStyleSlant.Oblique:
                return FontStyleType.Oblique;
        }
    }

    // TODO: Typeface
    /*
    public static AM.Typeface? ToTypeface(this SKTypeface? typeface)
    {
        if (typeface is null)
        {
            return null;
        }

        var familyName = typeface.FamilyName;
        var weight = typeface.FontWeight.ToFontWeight();
        // TODO: typeface.FontWidth
        var slant = typeface.Style.ToFontStyle();

        return new AM.Typeface(familyName, slant, weight);
    }
    */

    public static Color ToColor(this SKColor color)
    {
        return Color.FromRgba(color.Red, color.Green, color.Blue, color.Alpha);
    }

    public static Color ToColor(this SKColorF color)
    {
        return new Color(
            (byte)(color.Alpha * 255f),
            (byte)(color.Red * 255f),
            (byte)(color.Green * 255f),
            (byte)(color.Blue * 255f));
    }

    public static Color[] ToColors(this SKColor[] colors)
    {
        var skColors = new Color[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            skColors[i] = colors[i].ToColor();
        }

        return skColors;
    }

    public static Color[] ToColors(this SKColorF[] colors)
    {
        var skColors = new Color[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            skColors[i] = colors[i].ToColor();
        }

        return skColors;
    }

    // TODO: FilterQuality
    /*
    public static AVMI.BitmapInterpolationMode ToBitmapInterpolationMode(this SKFilterQuality filterQuality)
    {
        switch (filterQuality)
        {
            default:
            case SKFilterQuality.None:
                return AVMI.BitmapInterpolationMode.Default;

            case SKFilterQuality.Low:
                return AVMI.BitmapInterpolationMode.LowQuality;

            case SKFilterQuality.Medium:
                return AVMI.BitmapInterpolationMode.MediumQuality;

            case SKFilterQuality.High:
                return AVMI.BitmapInterpolationMode.HighQuality;
        }
    }
    */

    private static SolidPaint ToSolidColorBrush(this ColorShader colorShader)
    {
        var color = colorShader.Color.ToColor();
        return new SolidPaint(color);
    }

    // TODO: ShaderTileMode
    /*
    public static AM.GradientSpreadMethod ToGradientSpreadMethod(this SKShaderTileMode shaderTileMode)
    {
        switch (shaderTileMode)
        {
            default:
            case SKShaderTileMode.Clamp:
                return AM.GradientSpreadMethod.Pad;

            case SKShaderTileMode.Repeat:
                return AM.GradientSpreadMethod.Repeat;

            case SKShaderTileMode.Mirror:
                return AM.GradientSpreadMethod.Reflect;
        }
    }
    */

    // TODO: LinearGradientShader
    /*
    public static AM.IBrush? ToLinearGradientBrush(this LinearGradientShader linearGradientShader)
    {
        if (linearGradientShader.Colors is null || linearGradientShader.ColorPos is null)
        {
            return null;
        }

        var spreadMethod = linearGradientShader.Mode.ToGradientSpreadMethod();
        var start = linearGradientShader.Start.ToPoint();
        var end = linearGradientShader.End.ToPoint();

        if (linearGradientShader.LocalMatrix is { })
        {
            // TODO: linearGradientShader.LocalMatrix
            var localMatrix = linearGradientShader.LocalMatrix.Value.ToMatrix();
            start = localMatrix.Transform(start);
            end = localMatrix.Transform(end);
        }

        var startPoint = new A.RelativePoint(start, A.RelativeUnit.Absolute);
        var endPoint = new A.RelativePoint(end, A.RelativeUnit.Absolute);

        var gradientStops = new List<AMII.ImmutableGradientStop>();
        for (int i = 0; i < linearGradientShader.Colors.Length; i++)
        {
            var color = linearGradientShader.Colors[i].ToColor();
            var offset = linearGradientShader.ColorPos[i];
            var gradientStop = new AMII.ImmutableGradientStop(offset, color);
            gradientStops.Add(gradientStop);
        }

        return new AMII.ImmutableLinearGradientBrush(
            gradientStops,
            1,
            spreadMethod,
            startPoint,
            endPoint);
    }
    */

    // TODO: RadialGradientShader
    /*
    public static AM.IBrush? ToRadialGradientBrush(this RadialGradientShader radialGradientShader)
    {
        if (radialGradientShader.Colors is null || radialGradientShader.ColorPos is null)
        {
            return null;
        }

        var spreadMethod = radialGradientShader.Mode.ToGradientSpreadMethod();
        var center = radialGradientShader.Center.ToPoint();
        var gradientOrigin = center;

        if (radialGradientShader.LocalMatrix is { })
        {
            // TODO: radialGradientBrush.LocalMatrix
            var localMatrix = radialGradientShader.LocalMatrix.Value.ToMatrix();
            gradientOrigin = localMatrix.Transform(gradientOrigin);
            center = localMatrix.Transform(center);
        }

        var gradientOriginPoint = new A.RelativePoint(gradientOrigin, A.RelativeUnit.Absolute);
        var centerPoint = new A.RelativePoint(center, A.RelativeUnit.Absolute);
        var radius = radialGradientShader.Radius;

        var gradientStops = new List<AMII.ImmutableGradientStop>();
        for (int i = 0; i < radialGradientShader.Colors.Length; i++)
        {
            var color = radialGradientShader.Colors[i].ToColor();
            var offset = radialGradientShader.ColorPos[i];
            var gradientStop = new AMII.ImmutableGradientStop(offset, color);
            gradientStops.Add(gradientStop);
        }

        return new AMII.ImmutableRadialGradientBrush(
            gradientStops,
            1,
            spreadMethod,
            centerPoint,
            gradientOriginPoint,
            radius);
    }
    */

    // TODO: TwoPointConicalGradientShader
    /*
    public static AM.IBrush? ToRadialGradientBrush(this TwoPointConicalGradientShader twoPointConicalGradientShader)
    {
        if (twoPointConicalGradientShader.Colors is null || twoPointConicalGradientShader.ColorPos is null)
        {
            return null;
        }

        var spreadMethod = twoPointConicalGradientShader.Mode.ToGradientSpreadMethod();
        var center = twoPointConicalGradientShader.Start.ToPoint();
        var gradientOrigin = twoPointConicalGradientShader.End.ToPoint();

        if (twoPointConicalGradientShader.LocalMatrix is { })
        {
            // TODO: radialGradientBrush.LocalMatrix
            var localMatrix = twoPointConicalGradientShader.LocalMatrix.Value.ToMatrix();
            gradientOrigin = localMatrix.Transform(gradientOrigin);
            center = localMatrix.Transform(center);
        }

        var gradientOriginPoint = new A.RelativePoint(gradientOrigin, A.RelativeUnit.Absolute);
        var centerPoint = new A.RelativePoint(center, A.RelativeUnit.Absolute);

        // NOTE: twoPointConicalGradientShader.StartRadius is always 0.0
        var startRadius = twoPointConicalGradientShader.StartRadius;

        // TODO: Avalonia is passing 'radius' to 'SKShader.CreateTwoPointConicalGradient' as 'startRadius'
        // TODO: but we need to pass it as 'endRadius' to 'SKShader.CreateTwoPointConicalGradient'
        var endRadius = twoPointConicalGradientShader.EndRadius;
        var radius = 0.5; // endRadius

        var gradientStops = new List<AMII.ImmutableGradientStop>();
        for (int i = 0; i < twoPointConicalGradientShader.Colors.Length; i++)
        {
            var color = twoPointConicalGradientShader.Colors[i].ToColor();
            var offset = twoPointConicalGradientShader.ColorPos[i];
            var gradientStop = new AMII.ImmutableGradientStop(offset, color);
            gradientStops.Add(gradientStop);
        }

        return new AMII.ImmutableRadialGradientBrush(
            gradientStops,
            1,
            spreadMethod,
            centerPoint,
            gradientOriginPoint,
            radius);
    }
    */

    // TODO: Shader
    /*
    public static AM.IBrush? ToBrush(this SKShader? shader)
    {
        switch (shader)
        {
            case ColorShader colorShader:
                return ToSolidColorBrush(colorShader);

            case LinearGradientShader linearGradientShader:
                return ToLinearGradientBrush(linearGradientShader);

            case RadialGradientShader radialGradientShader:
                return ToRadialGradientBrush(radialGradientShader);

            case TwoPointConicalGradientShader twoPointConicalGradientShader:
                return ToRadialGradientBrush(twoPointConicalGradientShader);

            case PictureShader pictureShader:
                // TODO: pictureShader
                return null;

            default:
                return null;
        }
    }
    */

    // TODO: 
    /*
    private static AM.IPen ToPen(this SKPaint paint)
    {
        var brush = ToBrush(paint.Shader);
        var lineCap = paint.StrokeCap.ToPenLineCap();
        var lineJoin = paint.StrokeJoin.ToPenLineJoin();

        var dashStyle = default(AMII.ImmutableDashStyle);
        if (paint.PathEffect is DashPathEffect dashPathEffect && dashPathEffect.Intervals is { })
        {
            var dashes = new List<double>();
            foreach (var interval in dashPathEffect.Intervals)
            {
                dashes.Add(interval / paint.StrokeWidth);
            }
            var offset = dashPathEffect.Phase / paint.StrokeWidth;
            dashStyle = new AMII.ImmutableDashStyle(dashes, offset);
        }

        return new AMII.ImmutablePen(
            brush,
            paint.StrokeWidth,
            dashStyle,
            lineCap,
            lineJoin,
            paint.StrokeMiter
        );
    }
    */

    // TODO:
    public static void SetFill(this SKShader? shader, ICanvas canvas)
    {
        switch (shader)
        {
            case ColorShader colorShader:
                var color = colorShader.Color.ToColor();
                canvas.FillColor = color;
                break; 

            case LinearGradientShader linearGradientShader:
                // TODO:
                break;

            case RadialGradientShader radialGradientShader:
                // TODO:
                break;

            case TwoPointConicalGradientShader twoPointConicalGradientShader:
                // TODO:
                break;

            case PictureShader pictureShader:
                // TODO:
                break;

            default:
                // TODO:
                break;
        }
    }

    // TODO:
    public static void SetStroke(this SKPaint paint, ICanvas canvas)
    {
        switch (paint.Shader)
        {
            case ColorShader colorShader:
                var color = colorShader.Color.ToColor();
                canvas.StrokeColor = color;
                break;

            case LinearGradientShader linearGradientShader:
                // TODO:
                break;

            case RadialGradientShader radialGradientShader:
                // TODO:
                break;

            case TwoPointConicalGradientShader twoPointConicalGradientShader:
                // TODO:
                break;

            case PictureShader pictureShader:
                // TODO:
                break;

            default:
                // TODO:

                break;
        }

        var lineCap = paint.StrokeCap.ToPenLineCap();
        var lineJoin = paint.StrokeJoin.ToPenLineJoin();

        var dashPattern = default(float[]);
        if (paint.PathEffect is DashPathEffect dashPathEffect && dashPathEffect.Intervals is { })
        {
            var dashes = new List<double>();
            foreach (var interval in dashPathEffect.Intervals)
            {
                dashes.Add(interval / paint.StrokeWidth);
            }
            var offset = dashPathEffect.Phase / paint.StrokeWidth;
            // TODO: offset
        }

        canvas.StrokeSize = paint.StrokeWidth;
        canvas.StrokeDashPattern = dashPattern;
        canvas.StrokeLineJoin = lineJoin;
        canvas.StrokeLineCap = lineCap;
        canvas.MiterLimit = paint.StrokeMiter;
    }

    // TODO:
    public static void SetFont(this SKPaint paint, ICanvas canvas)
    {
        switch (paint.Shader)
        {
            case ColorShader colorShader:
                var color = colorShader.Color.ToColor();
                canvas.FontColor = color;
                break;

            case LinearGradientShader linearGradientShader:
                // TODO:
                break;

            case RadialGradientShader radialGradientShader:
                // TODO:
                break;

            case TwoPointConicalGradientShader twoPointConicalGradientShader:
                // TODO:
                break;

            case PictureShader pictureShader:
                // TODO:
                break;

            default:
                // TODO:

                break;
        }

        var fontSize = paint.TextSize;
        canvas.FontSize = fontSize;

        if (paint.Typeface is { } typeface)
        {
            // TODO: var weight = typeface.FontWeight.ToFontWeight();

            // TODO: typeface.FontWidth

            // TODO: var slant = typeface.Style.ToFontStyle();

            var familyName = typeface.FamilyName;
            canvas.FontName = familyName;
        }
        else
        {
            canvas.SetToSystemFont();
        }
    }

    // TODO: Paint
    /*
    public static (AM.IBrush? brush, AM.IPen? pen) ToBrushAndPen(this SKPaint paint)
    {
        AM.IBrush? brush = null;
        AM.IPen? pen = null;

        if (paint.Style == SKPaintStyle.Fill || paint.Style == SKPaintStyle.StrokeAndFill)
        {
            brush = ToBrush(paint.Shader);
        }

        if (paint.Style == SKPaintStyle.Stroke || paint.Style == SKPaintStyle.StrokeAndFill)
        {
            pen = ToPen(paint);
        }

        // TODO: paint.IsAntialias
        // TODO: paint.Color.ToColor()
        // TODO: paint.ColorFilter
        // TODO: paint.ImageFilter
        // TODO: paint.PathEffect
        // TODO: paint.BlendMode
        // TODO: paint.FilterQuality.ToBitmapInterpolationMode()

        return (brush, pen);
    }
    */

    // TODO: Text
    /*
    public static AM.FormattedText ToFormattedText(this SKPaint paint, string text)
    {
        var typeface = paint.Typeface?.ToTypeface();
        var textAlignment = paint.TextAlign.ToTextAlignment();
        var fontSize = paint.TextSize;
        // TODO: paint.TextEncoding
        // TODO: paint.LcdRenderText
        // TODO: paint.SubpixelText

        var ft = new AM.FormattedText
        {
            Text = text,
            Typeface = typeface ?? AM.Typeface.Default,
            FontSize = fontSize,
            TextAlignment = textAlignment,
            TextWrapping = AM.TextWrapping.NoWrap
        };

        return ft;
    }
    */

    public static WindingMode ToWindingMode(this SKPathFillType pathFillType)
    {
        switch (pathFillType)
        {
            default:
            case SKPathFillType.Winding:
                return WindingMode.NonZero;

            case SKPathFillType.EvenOdd:
                return WindingMode.EvenOdd;
        }
    }

    // TODO: PathDirection
    /*
    public static AM.SweepDirection ToSweepDirection(this SKPathDirection pathDirection)
    {
        switch (pathDirection)
        {
            default:
            case SKPathDirection.Clockwise:
                return AM.SweepDirection.Clockwise;

            case SKPathDirection.CounterClockwise:
                return AM.SweepDirection.CounterClockwise;
        }
    }
    */

    public static PathF? ToGeometry(this IList<SKPoint> points, bool close)
    {
        var pathF = new PathF();

        if (points.Count > 0)
        {
            pathF.MoveTo(points[0].ToPoint());

            for (int i = 1; i < points.Count; i++)
            {
                pathF.LineTo(points[i].ToPoint());
            }

            if (close)
            {
                pathF.Close();
            }
        }

        return pathF;
    }

    public static PathF? ToGeometry(this SKPath path)
    {
        if (path.Commands is null)
        {
            return null;
        }

        var pathF = new PathF();

        bool endFigure = false;
        bool haveFigure = false;

        for (int i = 0; i < path.Commands.Count; i++)
        {
            var pathCommand = path.Commands[i];
            var isLast = i == path.Commands.Count - 1;

            switch (pathCommand)
            {
                case MoveToPathCommand moveToPathCommand:
                {
                    if (endFigure == true && haveFigure == false)
                    {
                        return null;
                    }
                    if (haveFigure == true)
                    {
                        // TODO: pathF.Close();
                    }
                    if (isLast == true)
                    {
                        return pathF;
                    }
                    else
                    {
                        if (path.Commands[i + 1] is MoveToPathCommand)
                        {
                            return pathF;
                        }

                        if (path.Commands[i + 1] is ClosePathCommand)
                        {
                            return pathF;
                        }
                    }
                    endFigure = true;
                    haveFigure = false;
                    var x = moveToPathCommand.X;
                    var y = moveToPathCommand.Y;
                    var point = new Point(x, y);
                    // TODO: isFilled
                    pathF.MoveTo(point);
                }
                    break;

                case LineToPathCommand lineToPathCommand:
                {
                    if (endFigure == false)
                    {
                        return null;
                    }
                    haveFigure = true;
                    var x = lineToPathCommand.X;
                    var y = lineToPathCommand.Y;
                    var point = new Point(x, y);
                    pathF.LineTo(point);
                }
                    break;

                // TODO:
                /*
                case ArcToPathCommand arcToPathCommand:
                {
                    if (endFigure == false)
                    {
                        return null;
                    }
                    haveFigure = true;
                    var x = arcToPathCommand.X;
                    var y = arcToPathCommand.Y;
                    var point = new Point(x, y);
                    var rx = arcToPathCommand.Rx;
                    var ry = arcToPathCommand.Ry;
                    var size = new Size(rx, ry);
                    var rotationAngle = arcToPathCommand.XAxisRotate;
                    var isLargeArc = arcToPathCommand.LargeArc == SKPathArcSize.Large;
                    var clockwise = arcToPathCommand.Sweep == SKPathDirection.Clockwise;

                    pathF.AddArc(
                        point, 
                        new Point(point.X + size.Width, point.Y + size.Height), 
                        rotationAngle, 
                        isLargeArc,
                        clockwise);
                }
                    break;
                */

                case QuadToPathCommand quadToPathCommand:
                {
                    if (endFigure == false)
                    {
                        return null;
                    }
                    haveFigure = true;
                    var x0 = quadToPathCommand.X0;
                    var y0 = quadToPathCommand.Y0;
                    var x1 = quadToPathCommand.X1;
                    var y1 = quadToPathCommand.Y1;
                    var control = new Point(x0, y0);
                    var endPoint = new Point(x1, y1);
                    pathF.QuadTo(control, endPoint);
                }
                    break;

                case CubicToPathCommand cubicToPathCommand:
                {
                    if (endFigure == false)
                    {
                        return null;
                    }
                    haveFigure = true;
                    var x0 = cubicToPathCommand.X0;
                    var y0 = cubicToPathCommand.Y0;
                    var x1 = cubicToPathCommand.X1;
                    var y1 = cubicToPathCommand.Y1;
                    var x2 = cubicToPathCommand.X2;
                    var y2 = cubicToPathCommand.Y2;
                    var point1 = new Point(x0, y0);
                    var point2 = new Point(x1, y1);
                    var point3 = new Point(x2, y2);
                    pathF.CurveTo(point1, point2, point3);
                }
                    break;

                case ClosePathCommand _:
                {
                    if (endFigure == false)
                    {
                        return null;
                    }
                    if (haveFigure == false)
                    {
                        return null;
                    }
                    endFigure = false;
                    haveFigure = false;
                    pathF.Close();
                }
                    break;

                default:
                    break;
            }
        }

        if (endFigure)
        {
            if (haveFigure == false)
            {
                return null;
            }
            // TODO: pathF.Close();
        }

        return pathF;
    }

    // TODO: clipPath
    public static PathF? ToGeometry(this ClipPath clipPath, bool isFilled)
    {
        return null;
    }
}
