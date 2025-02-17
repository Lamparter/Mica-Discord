extern alias WV2;
using WV2::Microsoft.Web.WebView2.Core;
using PInvoke;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
#if WINDOWS10_0_17763_0_OR_GREATER
using Windows.UI.ViewManagement;
#endif
using AccentColorTypes = CustomPInvoke.UxTheme.AccentColorTypes;
using System.IO;
using System.Windows.Input;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
#if WINDOWS10_0_17763_0_OR_GREATER
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using AppWindow = Microsoft.UI.Windowing.AppWindow;
using WinUIColor = Windows.UI.Color;
#endif
using Colors = System.Windows.Media.Colors;
using SysDrawColor = System.Drawing.Color;
using Color = System.Windows.Media.Color;
using Button = System.Windows.Controls.Button;
using Control = System.Windows.Forms.Control;
namespace MicaDiscord;
#if WINDOWS10_0_17763_0_OR_GREATER
using AppSetting = MicaDiscord.Setting;
#endif
public partial class MainWindow : Window
{
    public static int WindowsMajorNumber = Environment.OSVersion.Version.Major;
    public static int WindowsBuildNumber = Environment.OSVersion.Version.Build;
#if WINDOWS10_0_17763_0_OR_GREATER
    static string StartupPath = "."; // Path.GetDirectoryName(typeof(MainWindow).Assembly.Location) ?? throw new NullReferenceException();
    static UISettings UISettings { get; } = new();
    public static bool IsNewTitleBarSupported => IsWin11; //AppWindowTitleBar.IsCustomizationSupported();
#else
    public static bool IsNewTitleBarSupported => false;
#endif
    public static string DefinedCSS
#if DEBUG
        => File.Exists("../../../The CSS.css") ? File.ReadAllText("../../../The CSS.css") : File.ReadAllText("The CSS.css");// Read File Every time
#else
        = File.ReadAllText($"{StartupPath}/The CSS.css"); // Read only once
#endif
    public static string DefinedJavascript
#if DEBUG
        => File.Exists("../../../MicaDiscordScript.js") ? File.ReadAllText("../../../MicaDiscordScript.js") : File.ReadAllText("MicaDiscordScript.js");// Read File Every time
#else
        = File.ReadAllText($"{StartupPath}/MicaDiscordScript.js"); // Read only once
#endif
    public static bool NotSupportedBuild => !IsWin11;
    public static bool IsWin11 => WindowsBuildNumber is >=22000;
    public static bool IsWin7 => WindowsMajorNumber is >= 7 and < 8;

    bool DiscordEffectApplied = false;
    bool _Dark = true;
    Action? ThemeChanged = null;
    bool Dark
    {
        set
        {
            _Dark = value;
            ThemeChanged?.Invoke();
        }
        get => _Dark;
    }

    void OnLoaded(object sender, RoutedEventArgs e)
    {
        ThemeChanged += () => (Resources["Color"] as SolidColorBrush ?? throw new NullReferenceException()).Color = Dark ? Colors.White : Colors.Black;
#if WINDOWS10_0_17763_0_OR_GREATER
        Dark = IsDarkBackground(UISettings.GetColorValue(UIColorType.Background));
        if (IsNewTitleBarSupported)
        {
            AppWindow = AppWindow.GetFromWindowId(WindowId);
            var AppTitleBar = AppWindow.TitleBar;
            AppTitleBar.ExtendsContentIntoTitleBar = true;

            var leftmargin = TitleText.Margin;
            leftmargin.Left += AppTitleBar.LeftInset;
            TitleText.Margin = leftmargin;

            var rightmargin = TitleBarDragable.Margin;
            rightmargin.Right = AppTitleBar.RightInset;
            TitleBarDragable.Margin = rightmargin;
            void UpdateColor()
            {
                var TranColor = new WinUIColor { A = 0 };
                if (Dark)
                {
                    TranColor.R = 0;
                    TranColor.G = 0;
                    TranColor.B = 0;
                } else
                {
                    TranColor.R = 255;
                    TranColor.G = 255;
                    TranColor.B = 255;
                }
                AppTitleBar.ButtonBackgroundColor = TranColor;
                AppTitleBar.ButtonInactiveBackgroundColor = TranColor;
                byte color = (byte)(255 - TranColor.R);
                TranColor.R = color;
                TranColor.G = color;
                TranColor.B = color;
                TranColor.A = 255 / 10;
                AppTitleBar.ButtonHoverBackgroundColor = TranColor;
                TranColor.A = 255 / 5;
                AppTitleBar.ButtonPressedBackgroundColor = TranColor;
                TranColor.A = 255;
                AppTitleBar.ButtonForegroundColor = TranColor;
                AppTitleBar.ButtonHoverForegroundColor = TranColor;
                AppTitleBar.ButtonInactiveForegroundColor = TranColor;
                AppTitleBar.ButtonPressedForegroundColor = TranColor;
            }
            UpdateColor();
            ThemeChanged += UpdateColor;
            void UpdateDragRectangles()
            {
                var Title1Location = TitleText.TransformToVisual(this).Transform(new System.Windows.Point());
                var Title2Location = TitleBarDragable.TransformToVisual(this).Transform(new System.Windows.Point());
                AppTitleBar.SetDragRectangles(new Windows.Graphics.RectInt32[]
                {
                    new Windows.Graphics.RectInt32
                    {
                        X = (int)(Title1Location.X - leftmargin.Left),
                        Y = 0,
                        Width = (int)(TitleText.ActualWidth + leftmargin.Left + leftmargin.Right),
                        Height = (int)TitleBar.ActualHeight
                    },
                    new Windows.Graphics.RectInt32
                    {
                        X = (int)Title2Location.X,
                        Y = 0,
                        Width = (int)TitleBarDragable.ActualWidth,
                        Height = (int)TitleBar.ActualHeight
                    }
                });
            }

            SizeChanged += delegate
            {
                UpdateDragRectangles();
            };
            UpdateDragRectangles();
            goto SetWindowChromeComplete;
        }
#endif
        SetValue(WindowChrome.WindowChromeProperty, WindowChrome);
        WindowChrome.SetIsHitTestVisibleInChrome(Back, true);
        WindowChrome.SetIsHitTestVisibleInChrome(Forward, true);
        WindowChrome.SetIsHitTestVisibleInChrome(Reload, true);
        WindowChrome.SetIsHitTestVisibleInChrome(Setting, true);
        goto SetWindowChromeComplete;
    SetWindowChromeComplete:
        void RefreshFrame()
        {
            HwndSource mainWindowSrc = HwndSource.FromHwnd(Handle);

#if WINDOWS10_0_17763_0_OR_GREATER
            if (!NotSupportedBuild || AppSetting.UseBackdropAnyway)
            {
                mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
                DwmApi.DwmExtendFrameIntoClientArea(Handle, new()
                {
                    cxLeftWidth = -1,
                    cxRightWidth = -1,
                    cyTopHeight = -1,
                    cyBottomHeight = -1,
                });
                goto End;
            }
#endif
            if (!IsNewTitleBarSupported)
            {
                WindowChrome.GlassFrameThickness = new Thickness(0);
                WindowChrome.UseAeroCaptionButtons = false;
                mainWindowSrc.CompositionTarget.BackgroundColor = Dark ? Color.FromArgb(255, 52, 52, 52) :
                       Color.FromArgb(255, 250, 250, 250);
            }

            WindowChrome.CaptionHeight = 32;
            goto End;
        End:
            ;
        }


        SizeChanged += (_, _) => RefreshFrame();
        IsVisibleChanged += (_, _) => RefreshFrame();
#if WINDOWS10_0_17763_0_OR_GREATER

        void RefreshDarkMode(bool dark) => CustomPInvoke.DwmApi.SetWindowAttribute(
            Handle,
            CustomPInvoke.DwmApi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
            dark ? 1 : 0
        );
        ThemeChanged += () => RefreshDarkMode(dark: Dark);
        static bool IsDarkBackground(WinUIColor color)
        {
            return color.R + color.G + color.B < (255 * 3 - color.R - color.G - color.B);
        }
        
        //RefreshDarkMode(dark: Dark);
        SetBackdrop((CustomPInvoke.BackdropType)Enum.Parse(typeof(CustomPInvoke.BackdropType), AppSetting.BackdropType, ignoreCase: true));
#else
        ThemeChanged += RefreshFrame;
#endif
        if (!IsNewTitleBarSupported)
        {
            TitleBarCaptionButtons.Visibility = Visibility.Visible;
            foreach (var child in TitleBarCaptionButtons.Children)
                WindowChrome.SetIsHitTestVisibleInChrome(child as IInputElement, true);
        }
        // Just to make everything updated again
        var d = Dark;
        Dark = d;

        Width += 1;
        WebView.CoreWebView2InitializationCompleted += (_, _) =>
        {
            var CoreWebView2 = WebView.CoreWebView2;
            CoreWebView2.NewWindowRequested += (_, e) =>
            {
                if (Control.ModifierKeys.HasFlag(Keys.Shift)) return;
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = e.Uri,
                    UseShellExecute = true
                });
                e.Handled = true;
            };

            var handle = WebView.Handle;
            _ = User32.SetWindowLong(handle, User32.WindowLongIndexFlags.GWL_EXSTYLE,
                (User32.SetWindowLongFlags)User32.GetWindowLong(handle, User32.WindowLongIndexFlags.GWL_EXSTYLE)
                | User32.SetWindowLongFlags.WS_CLIPCHILDREN
            );

            CoreWebView2.DocumentTitleChanged += (_, _) =>
            {
                var title = WebView.CoreWebView2.DocumentTitle;
                if (title == "Discord")
                {
                    title = "";
                    Title = "Mica Discord";
                }
                else Title = $"Mica Discord - {title}";
                WebsiteTitle.Text = title;
            };
            CoreWebView2.HistoryChanged += delegate
            {
                Back.IsEnabled = WebView.CanGoBack;
                Forward.IsEnabled = WebView.CanGoForward;
                RefreshFrame();
            };

            CoreWebView2.Settings.AreDevToolsEnabled = AppSetting.EnableDevTools;
            void DevToolsCheck(object _, System.Windows.Input.KeyEventArgs e)
            {
                if (e.Key == Key.I && Control.ModifierKeys == (Keys.Control | Keys.Shift))
                    goto OK;
                if (e.Key == Key.C && Control.ModifierKeys == (Keys.Control | Keys.Shift))
                    goto OK;
                if (e.Key == Key.F12)
                    goto OK;

                e.Handled = false;
                return;
            OK:
                if (AppSetting.EnableDevTools)
                {
                    if (AppSetting.ReplaceDiscordBackground)
                    {
#if WINDOWS10_0_17763_0_OR_GREATER
                        WinUIColor PrimaryColor = UISettings.GetColorValue(UIColorType.AccentLight3);
#else
                        SysDrawColor PrimaryColor = SysDrawColor.FromArgb(255, 88, 101, 242);
#endif
                        CoreWebView2.ExecuteScriptAsync($@"
(function() {{
let baseStyles = [
  'color: rgb({PrimaryColor.R}, {PrimaryColor.G}, {PrimaryColor.B})',
  'font-size: 100px',
] 
console.log('%cWARNING!',baseStyles.join(';'));
baseStyles = [
  'color: rgb({PrimaryColor.R}, {PrimaryColor.G}, {PrimaryColor.B})',
  'font-size: 50px',
] ;
console.log('%cDO NOT Paste ANYTHING that you do not understand how it works.', baseStyles.join(';'));
}})()");
                    }
                    e.Handled = false;
                }
                else
                    MessageBox.Show(caption: "Warning", text: "DevTools is currently disabled. You can enable it in Settings");
            }
            WebView.KeyDown += DevToolsCheck;

            CoreWebView2.WebMessageReceived += (_, e) =>
            {
                switch (e.TryGetWebMessageAsString())
                {
                    case "dark":
                        Dark = true;
                        break;
                    case "light":
                        Dark = false;
                        break;
                }
            };

            RefreshFrame();
        };
        WebView.NavigationCompleted += async delegate
        {

            if (!WebView.Source.OriginalString.Contains("discord.com")) return;
            DiscordEffectApplied = AppSetting.ReplaceDiscordBackground;
            if (DiscordEffectApplied)
            {
                var Dark = (await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('html')[0].classList.contains('theme-dark')")) == "true";
                var Light = (await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('html')[0].classList.contains('theme-light')")) == "true";
                if (Dark is false && Light is false)
                    Dark = this.Dark; // Don't change
#if WINDOWS10_0_17763_0_OR_GREATER
                WinUIColor
                PrimaryColor = UISettings.GetColorValue(UIColorType.AccentLight3),
                DisabledColor = UISettings.GetColorValue(UIColorType.AccentLight2),
                HoverColor = UISettings.GetColorValue(UIColorType.AccentLight2),
                Accent = UISettings.GetColorValue(UIColorType.Accent);
                DisabledColor.A /= 2;
#else
                SysDrawColor
                        PrimaryColor = SysDrawColor.FromArgb(255, 88, 101, 242),
                        DisabledColor = SysDrawColor.FromArgb(255 / 2, 50, 57, 140),
                        HoverColor = SysDrawColor.FromArgb(255, 69, 79, 191),
                        Accent = SysDrawColor.FromArgb(255, 88, 101, 242);
#endif


                (Resources["Color"] as SolidColorBrush ?? throw new NullReferenceException())
                        .Color = Color.FromArgb(PrimaryColor.A, PrimaryColor.R, PrimaryColor.G, PrimaryColor.B);

                /*
                static double GetHue(int red, int green, int blue)
                {

                    float min = Math.Min(Math.Min(red, green), blue);
                    float max = Math.Max(Math.Max(red, green), blue);

                    if (min == max)
                    {
                        return 0;
                    }

                    float hue;
                    if (max == red) hue = (green - blue) / (max - min);
                    else if (max == green) hue = 2f + (blue - red) / (max - min);
                    else hue = 4f + (red - green) / (max - min);

                    hue *= 60;
                    if (hue < 0) hue += 360;

                    return hue;
                }
                */
                this.Dark = Dark;
                var ErrorAccentColor = CustomPInvoke.UxTheme.GetAccentColor(AccentColorTypes.ImmersiveSaturatedInlineErrorText);
                await WebView.CoreWebView2.ExecuteScriptAsync(DefinedJavascript);

                await WebView.CoreWebView2.ExecuteScriptAsync($@"
(function () {{
    let s = document.createElement('style');
    s.innerHTML = `
:root {{ /* System Color */
    --sys-accent-prop: {Accent.R}, {Accent.G}, {Accent.B} !important;
    --sys-accent-alpha: {Accent.A} !important;
    --sys-text-color-on-accent: {((Accent.R * 0.299 + Accent.G * 0.587 + Accent.B * 0.114) > 186 ? "black" : "white")} !important;
    --sys-accent-light-3-prop: {PrimaryColor.R}, {PrimaryColor.G}, {PrimaryColor.B} !important;
    --sys-accent-light-3-alpha: {PrimaryColor.A} !important;
    --sys-text-color-on-accent-light-3: {((PrimaryColor.R * 0.299 + PrimaryColor.G * 0.587 + PrimaryColor.B * 0.114) > 186 ? "black" : "white")} !important;
    --sys-accent-disabled-prop: {DisabledColor.R}, {DisabledColor.G}, {DisabledColor.B} !important;
    --sys-accent-disabled-alpha: {DisabledColor.A} !important;
    --sys-error-accent-prop: {ErrorAccentColor.R}, {ErrorAccentColor.G}, {ErrorAccentColor.B} !important;
    --sys-error-accent-alpha: {ErrorAccentColor.A} !important;
    --sys-hover-accent-prop: {HoverColor.R}, {HoverColor.G}, {HoverColor.B} !important;
    --sys-hover-accent-alpha: {HoverColor.A} !important;
}}`.trim();
    document.head.appendChild(s);
}})()".Trim());
                await WebView.CoreWebView2.ExecuteScriptAsync($@"
(function () {{
    let s = document.createElement('style');
    s.innerHTML = `{DefinedCSS}`;
    document.head.appendChild(s);
}})()".Trim());
                RefreshFrame();
            }
        };

        Closing += (_, e) =>
        {
            if (!ForceClose && AppSetting.UseSystemTray)
            {
                e.Cancel = true;
                Hide();
            }
        };
    }
    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        SettingsDialog.ResetRequiresReload();
        DialogPlace.Visibility = Visibility.Visible;
        WebView.Visibility = Visibility.Hidden;
    }

    private void RefreshPage(object sender, RoutedEventArgs e)
    {
        try
        {
            WebView.Reload();
        }
        catch { }
    }
    public static SolidColorBrush GetColorFromHex(string hexaColor)
    {
        return new SolidColorBrush(
            Color.FromArgb(
            Convert.ToByte(hexaColor[1..2], 16),
            Convert.ToByte(hexaColor[3..2], 16),
            Convert.ToByte(hexaColor[5..2], 16),
            Convert.ToByte(hexaColor[7..2], 16)
        ));
    }
    public static Color GetColorFromUInt(uint value)
        => Color.FromArgb(
            (byte)(value >> 24),
            (byte)(value >> 16),
            (byte)(value >> 8),
            (byte)(value)
        );

    private void Minimize(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Maximize(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
public partial class MainWindow : Window
{
    readonly WV2::Microsoft.Web.WebView2.Wpf.WebView2 WebView;
    public bool ForceClose { get; set; } = false;
    public MainWindow()
    {
        AllowsTransparency = false;
        WindowInteropHelper = new WindowInteropHelper(this);
        InitializeComponent();
        
        if (!IsWin11) Resources["IconFont"] = new System.Windows.Media.FontFamily("Segoe MDL2 Assets");
        WebView2AddHere.Children.Add(WebView = new WV2::Microsoft.Web.WebView2.Wpf.WebView2
        {
            DefaultBackgroundColor = System.Drawing.Color.Transparent
        });
        async void InitializeAsync()
        {
            if (WebView == null) return;
            await WebView.EnsureCoreWebView2Async();
            WebView.Source = new Uri(@"https://discord.com/channels/@me");

        }
        InitializeAsync();
        TitleBar.Height = 32;
        Loaded += OnLoaded;
        SettingsDialog.OnClose += () =>
        {
            DialogPlace.Visibility = Visibility.Hidden;
            WebView.Visibility = Visibility.Visible;
            if (SettingsDialog.RequiresReload)
                WebView.Reload();
        };
        SettingsDialog.OnSettingsChanged += () =>
        {
#if WINDOWS10_0_17763_0_OR_GREATER
            SetBackdrop((CustomPInvoke.BackdropType)Enum.Parse(typeof(CustomPInvoke.BackdropType), AppSetting.BackdropType ?? "Mica", ignoreCase: true));
#endif
            var w = WebView.CoreWebView2;
            if (w != null)
            {
                w.Settings.AreDevToolsEnabled = AppSetting.EnableDevTools;
            }
        };
        //Icon = ImageSourceFromBitmap(ProgramResources.Logo);
        Back.Click += (_, _) =>
        {
            if (WebView.CanGoBack) WebView.GoBack();
        };
        Forward.Click += (_, _) =>
        {
            if (WebView.CanGoForward) WebView.GoForward();
        };
        if (!IsNewTitleBarSupported)
            SizeChanged += (_, _) =>
        {
            var a = WindowState == WindowState.Maximized ? 7.5 : 0;
            TitleBar.Margin = new Thickness(a, a, 0, 0);
            TitleBarCaptionButtons.Margin = new Thickness(0, 0, 7.5, 0);
            WebView.Margin = new Thickness(7.5, 0, 7.5, 7.5);
        };
    }
    WindowChrome WindowChrome { get; } = new WindowChrome
    {
        UseAeroCaptionButtons = true,
        ResizeBorderThickness = new Thickness(7.5)
    };
    public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
    {
        var handle = bmp.GetHbitmap();
        try
        {
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        finally { Gdi32.DeleteObject(handle); }
    }
#if WINDOWS10_0_17763_0_OR_GREATER
    void SetBackdrop(CustomPInvoke.BackdropType BackdropType) => SetBackdrop((int)BackdropType);
    void SetBackdrop(int BackdropType)
    {
        CustomPInvoke.DwmApi.SetWindowAttribute(
            Handle,
            CustomPInvoke.DwmApi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            BackdropType);
    }
#endif
    public WindowInteropHelper WindowInteropHelper { get; }
    public IntPtr Handle => WindowInteropHelper.Handle;
#if WINDOWS10_0_17763_0_OR_GREATER
    public WindowId WindowId => Win32Interop.GetWindowIdFromWindow(Handle);
    public AppWindow? AppWindow { get; private set; }
#endif
}
static class Extension
{
    public static T Edit<T>(this T In, Action<T> Act)
    {
        Act?.Invoke(In);
        return In;
    }
}