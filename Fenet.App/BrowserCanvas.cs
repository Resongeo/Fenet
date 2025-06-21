using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Fenet.Core;
using SkiaSharp;

namespace Fenet.App;

public class BrowserCanvas : Control
{
    private readonly BrowserEngine _browserEngine = new();
    private readonly GlyphRun _noSkiaFallback;

    public BrowserCanvas()
    {
        ClipToBounds = true;
        const string noSkiaText = "Skia is not available";
        var glyphs = noSkiaText
            .Select(ch => Typeface.Default.GlyphTypeface.GetGlyph(ch))
            .ToArray();
        _noSkiaFallback = new GlyphRun(Typeface.Default.GlyphTypeface, 12, noSkiaText.AsMemory(), glyphs);
    }
    
    public async Task Navigate(string url)
    {
        await _browserEngine.LoadUrlAsync(url);
    }

    public override void Render(DrawingContext context)
    {
        var bounds = new Rect(0, 0, Bounds.Width, Bounds.Height);
        context.Custom(new CustomDrawOp(bounds, _noSkiaFallback));
    }
}

internal class CustomDrawOp(Rect bounds, GlyphRun noSkiaFallback) : ICustomDrawOperation
{
    public Rect Bounds { get; } = bounds;

    private readonly IImmutableGlyphRunReference? _noSkiaFallback
        = noSkiaFallback.TryCreateImmutableGlyphRunReference();

    public void Render(ImmediateDrawingContext context)
    {
        var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();

        if (leaseFeature == null)
        {
            context.DrawGlyphRun(Brushes.Black, _noSkiaFallback!);
            return;
        }

        using var lease = leaseFeature.Lease();
        var canvas = lease.SkCanvas;

        canvas.Save();

        canvas.Clear(SKColors.White);
        
        var rect = new SKRect(10f, 10f, 50f, 50f);
        var paint = new SKPaint
        {
            IsStroke = false,
            IsAntialias = true,
            Color = SKColors.SeaGreen
        };
        canvas.DrawRoundRect(new SKRoundRect(rect, 10f), paint);
        
        canvas.Restore();
    }

    // TODO: Implement these properly
    public bool HitTest(Point p) => true;
    public bool Equals(ICustomDrawOperation? other) => false;
    public void Dispose()
    {
    }
}