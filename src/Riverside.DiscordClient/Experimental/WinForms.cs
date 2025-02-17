using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using PInvoke;
using Windows.UI.ViewManagement;
using static MicaDiscord.CustomPInvoke;
using WindowMessage = PInvoke.User32.WindowMessage;
namespace MicaDiscord;

public partial class WinForms : Form
{
    static readonly bool IsAccentColorEnabled = true;
    bool _IsMaximized;
    Rectangle beforeMax;
    bool IsMaximized
    {
        get => _IsMaximized;
        set
        {
            if (_IsMaximized != value)
            {
                _IsMaximized = value;
                if (value)
                {
                    beforeMax = new Rectangle
                    {
                        Location = Location,
                        Size = Size
                    };
                    var screen = Screen.FromControl(this).WorkingArea;
                    Location = new Point(screen.Left, screen.Top);
                    Size = screen.Size;
                } else
                {
                    Location = beforeMax.Location;
                    Size = beforeMax.Size;
                }
            }
        }
    }
    const string Radius = "0.5rem";
    public WinForms()
    {
        //_ = new NativeListener(this);
        InitializeComponent();
        //themeButton1.ContentAlignment = ContentAlignment.MiddleLeft;
        //_ = themeButton1.ContentAlignment;
        //webView21.BackColor = Color.Transparent;
        HandleCreated += (_, _) =>
        {
            User32.GetWindowRect(Handle, out var Rect);
            User32.SetWindowPos(Handle,
                IntPtr.Zero,
                Rect.left, Rect.top,
                Rect.right - Rect.left,
                Rect.bottom - Rect.top,
                User32.SetWindowPosFlags.SWP_FRAMECHANGED
            );
        };
        WebView.NavigationCompleted += async delegate
        {
            var DiscordEffectApplied = Settings.Default.ReplaceDiscordBackground;
            if (DiscordEffectApplied)
            {

                var Dark = (await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('html')[0].classList.contains('theme-dark')")) == "true";
                var UISettings = new UISettings();
                var PrimaryColor = UISettings.GetColorValue(Dark ? UIColorType.AccentLight2 : UIColorType.AccentDark2);//CustomPInvoke.GetAccentColor(Dark ? AccentColorTypes.ImmersiveSaturatedHighlight : AccentColorTypes.ImmersiveSaturatedSelectionBackground);
                var DisabledColor = Color.Red;//CustomPInvoke.GetAccentColor(AccentColorTypes.ImmersiveSaturatedCommandRowDisabled);
                var HoverColor = UISettings.GetColorValue(Dark ? UIColorType.AccentLight1 : UIColorType.AccentDark1);//CustomPInvoke.GetAccentColor(AccentColorTypes.ImmersiveSaturatedCommandRowHover);
                //if (IsAccentColorEnabled) (Resources["Color"] as SolidColorBrush ?? throw new NullReferenceException()).Color = PrimaryColor;

                //RefreshDarkMode(dark: Dark);
                //this.Dark = Dark;
                var LightColorCSS = Dark && Settings.Default.ModeAwareCSS;
                var invc = LightColorCSS ? 250 : 0;
                var regc = LightColorCSS ? 0 : 255;
                var regcgray = LightColorCSS ? 50 : 200;
                var floating = Dark ? 0 : 255;
                var ErrorAccentColor = CustomPInvoke.GetAccentColor(AccentColorTypes.ImmersiveSaturatedInlineErrorText);
                await WebView.CoreWebView2.ExecuteScriptAsync($@"
(function () {{
    let s = document.createElement('style');
    s.innerHTML = `
.theme-{(Dark ? "dark" : "light")} {{
    --background-layering: rgba({invc},{invc},{invc},0.05);
    --background-layering-half: rgba({invc},{invc},{invc},0.025);
    --background-primary: var(--background-layering);
    --background-secondary: transparent;
    --background-secondary-alt: rgba({invc},{invc},{invc},0.075);
    --background-tertiary: transparent;
    --background-message-hover: rgba({invc},{invc},{invc},0.07);
    --background-floating: rgba({floating},{floating},{floating},0.75);
    --deprecated-store-bg: rgba({invc},{invc},{invc},0.05);
    --channeltextarea-background: var(--background-layering);
    --scrollbar-auto-track: transparent;
    --scrollbar-thin-track: #0000;
    --scrollbar-thin-thumb: rgba({invc},{invc},{invc},0.25);
    --scrollbar-auto-thumb: rgba({invc},{invc},{invc},0.25);
    --background-modifier-hover: var(--background-layering-half);
    --background-modifier-selected: var(--background-layering);
    --background-mentioned-hover: hsla(38,calc(var(--saturation-factor, 1)*95.7%),54.1%,{0.08 + (LightColorCSS ? 0.1 : 0)});
    --win-accent-color: rgba({PrimaryColor.R}, {PrimaryColor.G}, {PrimaryColor.B}, {PrimaryColor.A});
    --win-accent-disabled-color: rgba({DisabledColor.R}, {DisabledColor.G}, {DisabledColor.B}, {DisabledColor.A});
    --win-accent-disabled-color-half: rgba({DisabledColor.R}, {DisabledColor.G}, {DisabledColor.B}, {DisabledColor.A / 2});
    --win-error-accent-color: rgba({ErrorAccentColor.R}, {ErrorAccentColor.G}, {ErrorAccentColor.B}, {ErrorAccentColor.A});
    --win-hover-accent-color: rgba({HoverColor.R}, {HoverColor.G}, {HoverColor.B}, {HoverColor.A});
    --text-link: var(--win-accent-color);
{(
    IsAccentColorEnabled ? @"
    --interactive-active: var(--win-accent-color);
    --interactive-normal: var(--win-accent-color);
    --interactive-hover: var(--win-hover-accent-color);
    --interactive-disabled: var(--win-accent-disabled-color-half);
    --text-muted: var(--win-accent-disabled-color-half);
    --channels-default: var(--win-accent-disabled-color);
" : ""
)}
    
}}

.app-3xd6d0 {{
    margin-top: 30px;
}}
.content-3spvdd {{
    --background-primary: rgb({regcgray},{regcgray},{regcgray});
}}
code, article {{
    --background-secondary: rgba({invc},{invc},{invc},0.05);
}}
.lookFilled-yCfaCM.colorPrimary-2AuQVo, .lookFilled-yCfaCM.colorGrey-2iAG-B {{
    background-color: rgba({invc},{invc},{invc},0.05) !important;
}}
.content-2a4AW9 {{
    --background-secondary: var(--background-layering-half);
    border-color: black;
}}

.form-3gdLxP {{
    margin-top: 0px !important;
    padding-top: 16px !important;
}}
.form-3gdLxP:before {{
    width: 0px !important;
    height: 0px !important;
}}
.scroller-kQBbkU::-webkit-scrollbar-track {{
    margin-bottom: 0px !important;
}}
.scrollerSpacer-3AqkT9 {{
    display: none;
}}
.content-2a4AW9, .members-3WRCEx, .membersWrap-3NUR2t {{
    min-height: 100%;
    padding: 0px !important;
}}
.chat-2ZfjoI, .container-2cd8Mz, .container-36u7Lw, .applicationStore-2nk7Lo {{
    border-radius: {Radius} 0px 0px 0px;
}}
.chatContent-3KubbW {{
    background-color: var(--background-layering-half) !important;
    border-color: black;
}}
.container-2cd8Mz {{
    background-color: rgba({invc},{invc},{invc},0.05) !important;
}}

.callContainer-HtHELf {{
    background-color: rgba({regc},{regc},{regc},0);
}}

.panels-3wFtMD {{
    border-radius: {Radius} 0px 0px 0px;
}}
.message-2CShn3 {{
    border-radius: {Radius};
}}
.sidebar-1tnWFu {{
border-radius: {Radius} {Radius} 0px 0px;
}}
.popout-TdhJ6Z {{
    --background-tertiary: rgba({floating},{floating},{floating},0.75);
}}
.popout-1KHNAq {{
    --background-secondary: rgba({floating},{floating},{floating},0.75);
}}
.lookFilled-1GseHa.select-1Ia3hD {{
    --background-secondary: rgba({invc},{invc},{invc},0.05);
    --background-tertiary: rgba({invc},{invc},{invc},0.75);
}}
.unread-36eUEm, .item-2LIpTv {{
    border-radius: 4px;
}}
.unread-36eUEm /* Unread Text Channels */
{{
    background-color: var(--win-accent-color);
}}
.item-2LIpTv {{
    margin: 0px 0px 0px 2px;
    width: 4px;
    background: var(--win-accent-color);
}}
.gradientContainer-phMG8d /* In-Call Gradient effect on top and bottom */
{{
    height: 40px;
    background-image: linear-gradient(rgba({invc},{invc},{invc},0.05),transparent);
}}
.modeSelected-3DmyhH::before /* Selected Text Channel */
{{
    content: """";
    position: absolute;
    height: 18px;
    width: 4px;
    border-radius: 4px;
    top: 50%;
    left: 0;
    margin-top: -9px;
    background: var(--win-accent-color);
}}
.modeUnread-3Cxepe .channelName-3KPsGw {{
    font-weight: 600;
}}
.hljs-comment {{
    color: seagreen !important;
}}
.layer-2aCOJ3 /* Thread Menu */
{{
    background-color: var(--background-floating);
    border-radius: {Radius}
}}
`.trim();
    document.head.appendChild(s);
}})()
".Trim());
                await WebView.CoreWebView2.ExecuteScriptAsync($@"
(function () {{
    
}})()
".Trim());
                //RefreshFrame();
            }
        };
        //foreach (var (btn, sysbtn) in new (ThemeButton btn, HitTestValues sysbtn)[]
        //{
        //    (Minimize, HitTestValues.HTMINBUTTON),
        //    (Maximize, HitTestValues.HTMAXBUTTON),
        //    (Close, HitTestValues.HTCLOSE)
        //})
        //{
        //    void MouseMessage(User32.WindowMessage Message)
        //    {
        //        User32.ReleaseCapture();
        //        User32.SendMessage(Handle, Message, (IntPtr)sysbtn, IntPtr.Zero);
        //    }
        //    btn.MouseDown += (_, _) => MouseMessage(User32.WindowMessage.WM_NCLBUTTONDOWN);
        //    btn.MouseUp += (_, _) => MouseMessage(User32.WindowMessage.WM_NCLBUTTONUP);
        //    btn.MouseMove += (_, _) => MouseMessage(User32.WindowMessage.WM_MOUSEMOVE);
        //    btn.MouseEnter += (_, _) => MouseMessage(User32.WindowMessage.WM_MOUSEFIRST);
        //    btn.MouseLeave += (_, _) => MouseMessage(User32.WindowMessage.WM_MOUSELAST);
        //}
        //_ = new NativeButtonListener(Minimize, HitTestValues.HTMINBUTTON);
        //_ = new NativeButtonListener(Maximize, HitTestValues.HTMAXBUTTON);
        //_ = new NativeButtonListener(Close, HitTestValues.HTCLOSE);
        Minimize.Click += (_, _) => WindowState = FormWindowState.Minimized;
        Maximize.Click += (_, _) => IsMaximized = !IsMaximized;
        CloseBtn.Click += (_, _) => Close();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.Clear(Color.Black);
    }

    private void TitleBarMouseMove(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            User32.ReleaseCapture();
            User32.SendMessage(Handle, User32.WindowMessage.WM_NCLBUTTONDOWN, (IntPtr)HitTestValues.HTCAPTION, IntPtr.Zero);
        }
    }
    private void TitleBarMouseDoubleClick(object sender, MouseEventArgs e)
        => IsMaximized = !IsMaximized;
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.Style |= (int) User32.WindowStyles.WS_OVERLAPPED;
            //cp.Style |= ~(int)User32.WindowStyles.WS_BORDER;
            return cp;
        }
    }
    

    static void SetBackdrop(IntPtr Handle, BackdropType BackdropType) => SetBackdrop(Handle, (int)BackdropType);
    static void SetBackdrop(IntPtr Handle, int BackdropType)
    {
        CustomPInvoke.SetWindowAttribute(
            Handle,
            CustomPInvoke.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            BackdropType);
    }
    //Control Control { get; }
    //public NativeListener(Control Control)
    //{
    //    this.Control = Control;
    //    Control.HandleCreated += (_, _) => AssignHandle(Control.Handle);
    //    Control.HandleDestroyed += (_, _) => ReleaseHandle();
    //}

    protected override void WndProc(ref Message m)
    {
        IntPtr Handle;
        switch ((WindowMessage)m.Msg)
        {
            case WindowMessage.WM_ACTIVATE:
                Handle = this.Handle;
                DwmApi.DwmExtendFrameIntoClientArea(Handle, new PInvoke.UxTheme.MARGINS
                {
                    cxLeftWidth = -1,
                    cxRightWidth = -1,
                    cyBottomHeight = -1,
                    cyTopHeight = -1,
                });
                SetBackdrop(Handle, BackdropType.Mica);
                CustomPInvoke.SetWindowAttribute(
                    Handle,
                    CustomPInvoke.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                    1
                );
                m.Result = IntPtr.Zero;
                break;
            case WindowMessage.WM_NCHITTEST:
                Handle = this.Handle;
                const int TopTitleHeight = 30;
                const int FormBorderWidth = 8;

                // Hit test the frame for resizing and moving.
                //LRESULT HitTestNCA(HWND hWnd, WPARAM wParam, LPARAM lParam)
                // Get the point coordinates for the hit test.
                if (DwmApi.DwmDefWindowProc(Handle, (uint)m.Msg, m.WParam, m.LParam, out var _))
                    break;
                Point ptMouse = new(m.LParam.ToInt32());

                // Get the window rectangle.
                User32.GetWindowRect(Handle, out var rcWindow);

                // Get the frame rectangle, adjusted for the style without a caption.
                RECT rcFrame = new();
                unsafe
                {
                    User32.AdjustWindowRectEx(&rcFrame, User32.WindowStyles.WS_CAPTION & ~User32.WindowStyles.WS_BORDER, false, default); // User32.WindowStyles.WS_OVERLAPPEDWINDOW & ~
                }


                // Determine if the hit test is for resizing. Default middle (1,1).
                ushort uRow = 1;
                ushort uCol = 1;
                bool fOnResizeBorder = false;

                // Determine if the point is at the top or bottom of the window.
                if (ptMouse.Y >= rcWindow.top && ptMouse.Y < rcWindow.top + TopTitleHeight)
                {
                    fOnResizeBorder = (ptMouse.Y < (rcWindow.top - rcFrame.top));
                    uRow = 0;
                }
                else if (ptMouse.Y < rcWindow.bottom && ptMouse.Y >= rcWindow.bottom - FormBorderWidth)
                {
                    uRow = 2;
                }

                // Determine if the point is at the left or right of the window.
                if (ptMouse.X >= rcWindow.left && ptMouse.X < rcWindow.left + FormBorderWidth)
                {
                    uCol = 0; // left side
                }
                else if (ptMouse.X < rcWindow.right && ptMouse.X >= rcWindow.right - FormBorderWidth)
                {
                    uCol = 2; // right side
                }

                // Hit test (HTTOPLEFT, ... HTBOTTOMRIGHT)

                var output = ((uRow == 0 && uCol == 1 && fOnResizeBorder == false)
                    ? HitTestValues.HTCAPTION : HitTests[uRow][uCol]);

                m.Result = (IntPtr)output;
                //}

                if (output == HitTestValues.HTNOWHERE) goto default;
                break;
            case WindowMessage.WM_SYSCOMMAND    :
                if (m.WParam.ToInt32() == (int)User32.SysCommands.SC_MAXIMIZE)
                {
                    Location = new Point(0, 0);
                    Size = new Size(Screen.GetWorkingArea(this).Width,
                                    Screen.GetWorkingArea(this).Height);
                    break;
                }
                goto default;
            case WindowMessage.WM_NCCALCSIZE:
                m.Result = IntPtr.Zero;
                break;
            default:
                base.WndProc(ref m);
                break;
        }

    }
    readonly static HitTestValues[][] HitTests = new HitTestValues[][] {
        new HitTestValues[] { HitTestValues.HTTOPLEFT, HitTestValues.HTTOP,    HitTestValues.HTTOPRIGHT },
        new HitTestValues[] { HitTestValues.HTLEFT,       HitTestValues.HTNOWHERE,     HitTestValues.HTRIGHT },
        new HitTestValues[] { HitTestValues.HTBOTTOMLEFT, HitTestValues.HTBOTTOM, HitTestValues.HTBOTTOMRIGHT },
    };
}
enum HitTestValues
{
    /// <summary>
    /// In the border of a window that does not have a sizing border.
    /// </summary>
    HTBORDER = 18,

    /// <summary>
    /// In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).
    /// </summary>
    HTBOTTOM = 15,

    /// <summary>
    /// In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
    /// </summary>
    HTBOTTOMLEFT = 16,

    /// <summary>
    /// In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
    /// </summary>
    HTBOTTOMRIGHT = 17,

    /// <summary>
    /// In a title bar.
    /// </summary>
    HTCAPTION = 2,

    /// <summary>
    /// In a client area.
    /// </summary>
    HTCLIENT = 1,

    /// <summary>
    /// In a Close button.
    /// </summary>
    HTCLOSE = 20,

    /// <summary>
    /// On the screen background or on a dividing line between windows (same as HTNOWHERE, except that the DefWindowProc function produces a system beep to indicate an error).
    /// </summary>
    HTERROR = -2,

    /// <summary>
    /// In a size box (same as HTSIZE).
    /// </summary>
    HTGROWBOX = 4,

    /// <summary>
    /// In a Help button.
    /// </summary>
    HTHELP = 21,

    /// <summary>
    /// In a horizontal scroll bar.
    /// </summary>
    HTHSCROLL = 6,

    /// <summary>
    /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
    /// </summary>
    HTLEFT = 10,

    /// <summary>
    /// In a menu.
    /// </summary>
    HTMENU = 5,

    /// <summary>
    /// In a Maximize button.
    /// </summary>
    HTMAXBUTTON = 9,

    /// <summary>
    /// In a Minimize button.
    /// </summary>
    HTMINBUTTON = 8,

    /// <summary>
    /// On the screen background or on a dividing line between windows.
    /// </summary>
    HTNOWHERE = 0,

    /// <summary>
    /// Not implemented.
    /// </summary>
    /* HTOBJECT = 19, */

    /// <summary>
    /// In a Minimize button.
    /// </summary>
    HTREDUCE = HTMINBUTTON,

    /// <summary>
    /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
    /// </summary>
    HTRIGHT = 11,

    /// <summary>
    /// In a size box (same as HTGROWBOX).
    /// </summary>
    HTSIZE = HTGROWBOX,

    /// <summary>
    /// In a window menu or in a Close button in a child window.
    /// </summary>
    HTSYSMENU = 3,

    /// <summary>
    /// In the upper-horizontal border of a window.
    /// </summary>
    HTTOP = 12,

    /// <summary>
    /// In the upper-left corner of a window border.
    /// </summary>
    HTTOPLEFT = 13,

    /// <summary>
    /// In the upper-right corner of a window border.
    /// </summary>
    HTTOPRIGHT = 14,

    /// <summary>
    /// In a window currently covered by another window in the same thread (the message will be sent to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).
    /// </summary>
    HTTRANSPARENT = -1,

    /// <summary>
    /// In the vertical scroll bar.
    /// </summary>
    HTVSCROLL = 7,

    /// <summary>
    /// In a Maximize button.
    /// </summary>
    HTZOOM = HTMAXBUTTON,
}
