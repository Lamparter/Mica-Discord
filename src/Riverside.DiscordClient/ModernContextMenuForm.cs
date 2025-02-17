extern alias WV2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PInvoke;
using WV2::Microsoft.Web.WebView2.Core;
namespace MicaDiscord;

public partial class ModernContextMenuForm : Form
{
    public List<(string Text, System.IO.Stream? Image, CoreWebView2ContextMenuItemKind ItemKind, Action<CoreWebView2ContextMenuItem>? Edit, Action<CoreWebView2ContextMenuItem>? OnClick)>
        MenuList
    { get; } = new();
    public ModernContextMenuForm()
    {
        Init();
    }
    void Init(bool Reinit = false)
    {
        if (Reinit) Controls.Clear();
        InitializeComponent();
        TopMost = true;
        FormBorderStyle = FormBorderStyle.None;
        Load += (_, _) => Size = new Size(1, 1);
        Shown += (_, _) => Visible = false;
        if (Reinit)
        {
            Size = new Size(1, 1);
            Visible = false;
        }
        WebView.NavigationCompleted += (_, _) => Focus();
        WebView.CoreWebView2InitializationCompleted += (_, _) =>
        {
            var env = WebView.CoreWebView2.Environment;
            WebView.CoreWebView2.ProcessFailed += (_, e) =>
            {
                Init(Reinit: true);
            };
            WebView.CoreWebView2.ContextMenuRequested += (_, e) =>
            {
                e.MenuItems.Clear();
                foreach (var (Text, Image, ItemKind, Edit, OnClick) in MenuList)
                {
                    var menuitem = env.CreateContextMenuItem(Text, Image, ItemKind);
                    Edit?.Invoke(menuitem);
                    menuitem.CustomItemSelected += (_, _) => OnClick?.Invoke(menuitem);
                    e.MenuItems.Add(menuitem);
                }
            };
        };
        Show();
        Top = 0;
        Left = 0;
    }
    public void UpdatePosition(int X, int Y)
    {
        if (Visible == false) Show();
        Location = new Point(X, Y);
    }
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW;
            return cp;
        }
    }
}

static class Constants
{
    public static Color Layering = Color.FromArgb((int)(255 * 0.1), 255, 255, 255);
    public static Color DoubleLayering = Color.FromArgb((int)(255 * 0.2), 255, 255, 255);
}