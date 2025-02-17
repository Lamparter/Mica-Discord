using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Web.WebView2.WinForms;
using System.Runtime.InteropServices;
using PInvoke;
namespace MicaDiscord;

static class Constants
{
    public static Color Layering = Color.FromArgb((int)(255 * 0.1), 255, 255, 255);
    public static Color DoubleLayering = Color.FromArgb((int)(255 * 0.2), 255, 255, 255);
}
public class WebView3 : WebView2
{
    public WebView3()
    {
        NavigationCompleted += (_, _) => Focus();
    }
    [DllImport("user32.dll")]
    static extern IntPtr SetFocus(IntPtr hWnd);
    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        if (CoreWebView2 != null)
        {
            SetFocus(User32.GetWindow(Handle, User32.GetWindowCommands.GW_CHILD));
        }
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.Clear(Color.Black);
    }
}
public class ThemeLabel : UserControl
{
    SolidBrush BackBrush { get; } = new SolidBrush(DefaultBackColor);
    SolidBrush ForeBrush { get; } = new SolidBrush(DefaultForeColor);
    public string ContentText
    {
        get => base.Text;
        set => base.Text = value;
    }
    StringFormat DefaultFormat { get; } = new StringFormat
    {
        Alignment = StringAlignment.Near,
        LineAlignment = StringAlignment.Near
    };
    public ContentAlignment ContentAlignment {
        get => (ContentAlignment)(DefaultFormat.Alignment switch
        {
            StringAlignment.Near => 1,
            StringAlignment.Center => 2,
            StringAlignment.Far => 4,
            _ => throw new ArgumentOutOfRangeException()
        } << (DefaultFormat.LineAlignment switch
        {
            StringAlignment.Near => 0,
            StringAlignment.Center => 4,
            StringAlignment.Far => 8,
            _ => throw new ArgumentOutOfRangeException()
        }));
        set
        {
            var numval = (ushort)value;
            //if (value == ContentAlignment.MiddleLeft) System.Diagnostics.Debugger.Break();
            
            DefaultFormat.LineAlignment = numval switch
            {
                >= 0x100 => StringAlignment.Far,
                >= 0x10 => StringAlignment.Center,
                >= 0 => StringAlignment.Near,
                //_ => throw new ArgumentOutOfRangeException(nameof(value), "Wrong Vertical Alignment Format")
            };
            DefaultFormat.Alignment = (numval >> (numval switch
            {
                >= 0x100 => 8,
                >= 0x10 => 4,
                >= 0 => 0,
                //_ => throw new ArgumentOutOfRangeException(nameof(value), "Wrong Vertical Alignment Format")
            })) switch
            {
                1 => StringAlignment.Near,
                2 => StringAlignment.Center,
                4 => StringAlignment.Far,
                _ => throw new ArgumentOutOfRangeException(nameof(value), "Wrong Horizontal Alignment Format")
            };
            Invalidate();
        }
    }
    //public string Text;
    public ThemeLabel()
    {
        
        Margin = new Padding(0);
        Padding = new Padding(0);
        Height = 30;
        BackColor = Color.Transparent;
        ForeColor = Color.White;
        
    }
    protected override void OnPaintBackground(PaintEventArgs e) => e.Graphics.Clear(Color.Black);
    protected override void OnForeColorChanged(EventArgs e) => ForeBrush.Color = ForeColor;
    protected override void OnBackColorChanged(EventArgs e) => BackBrush.Color = BackColor;
    protected override void OnTextChanged(EventArgs e)
    {
        Invalidate();
        base.OnTextChanged(e);
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var rect = ClientRectangle;
        e.Graphics.FillRectangle(BackBrush, rect);
        if (BackgroundImage is not null) e.Graphics.DrawImageUnscaled(BackgroundImage, rect);
        e.Graphics.DrawString(
            ContentText,
            Font,
            ForeBrush,
            rect,
            DefaultFormat
        );
    }
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams l_cp;
            l_cp = base.CreateParams;
            l_cp.ExStyle |= (int)User32.WindowStylesEx.WS_EX_TRANSPARENT;
            return l_cp;
        }
    }
}
public class ThemeButton : ThemeLabel
{
    public static Color DefaultHoverColor = Constants.Layering;
    public static Color DefaultPressedColor = Constants.Layering;
    public bool IsMouseHovering { get; private set; }
    public bool IsMouseDown { get; private set; }
    public Color HoverColor
    {
        get => HoverBrush.Color;
        set => HoverBrush.Color = value;
    }
    public Color PressedColor
    {
        get => PressedBrush.Color;
        set => PressedBrush.Color = value;
    }
    SolidBrush HoverBrush { get; } = new SolidBrush(DefaultHoverColor);
    SolidBrush PressedBrush { get; } = new SolidBrush(DefaultPressedColor);
    public ThemeButton()
    {
        Size = new Size(30, 30);
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        IsMouseHovering = true;
        Invalidate();
        base.OnMouseEnter(e);
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        IsMouseHovering = false;
        Invalidate();
        base.OnMouseLeave(e);
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        IsMouseDown = true;
        Invalidate();
        base.OnMouseDown(e);
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        IsMouseDown = false;
        Invalidate();
        base.OnMouseUp(e);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        var rect = ClientRectangle;
        if (IsMouseHovering) e.Graphics.FillRectangle(HoverBrush, rect);
        base.OnPaint(e);
        if (IsMouseDown) e.Graphics.FillRectangle(PressedBrush, rect);
    }
}