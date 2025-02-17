using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Interop;
using PInvoke;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using AccentColorTypes = MicaDiscord.CustomPInvoke.AccentColorTypes;
namespace MicaDiscord;

public partial class WinFormsIntegrationTest : Window
{
    const bool IsAccentColorEnabled = false;
    const string Radius = "0.5rem";
    public static readonly bool NotSupportedBuild = Environment.OSVersion.Version.Build < 22523;
    bool DiscordEffectApplied => false;
    bool _Dark = true;
    bool Dark
    {
        set
        {
            _Dark = value;
            if (!IsAccentColorEnabled) (Resources["Color"] as SolidColorBrush ?? throw new NullReferenceException()).Color = value ? Colors.White : Colors.Black;
        }
        get => _Dark;
    }

    void OnLoaded(object sender, RoutedEventArgs e)
    {
        void RefreshFrame()
        {
            HwndSource mainWindowSrc = HwndSource.FromHwnd(Handle);
            if (NotSupportedBuild && !Settings.Default.UseBackdropAnyway)
            {
                mainWindowSrc.CompositionTarget.BackgroundColor = Dark ? System.Windows.Media.Color.FromRgb(50, 50, 50) :
                       System.Windows.Media.Color.FromRgb(250, 250, 250);
            }
            else
            {
                mainWindowSrc.CompositionTarget.BackgroundColor = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
                DwmApi.DwmExtendFrameIntoClientArea(Handle, new()
                {
                    cxLeftWidth = -1,
                    cxRightWidth = -1,
                    cyTopHeight = -1,
                    cyBottomHeight = -1,
                });
            }
        }
        void RefreshDarkMode(bool dark) => CustomPInvoke.SetWindowAttribute(
            Handle,
            CustomPInvoke.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
            dark ? 1 : 0 // Always dark as of now, or you can change to: ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark ? 1 : 0
        );
        RefreshDarkMode(dark: true);
        SizeChanged += (_, _) => RefreshFrame();
        IsVisibleChanged += (_, _) => RefreshFrame();
        //ThemeManager.Current.ActualApplicationThemeChanged += (_, _) => RefreshDarkMode();
        Width += 1;

        SetBackdrop((BackdropType)Enum.Parse(typeof(BackdropType), Settings.Default.BackdropType, ignoreCase: true));
        Closing += (_, e) =>
        {
            if (!ForceClose && Settings.Default.UseSystemTray)
            {
                e.Cancel = true;
                Hide();
            }
        };

        //TitleBarUI.Margin = new Thickness(0, 0, (SystemParameters.WindowCaptionButtonWidth+10) * 3, 0);
        //this.TitleBar.Height = TitleBar.Height;
    }

    public static SolidColorBrush GetColorFromHex(string hexaColor)
    {
        return new SolidColorBrush(
            System.Windows.Media.Color.FromArgb(
            Convert.ToByte(hexaColor[1..2], 16),
            Convert.ToByte(hexaColor[3..2], 16),
            Convert.ToByte(hexaColor[5..2], 16),
            Convert.ToByte(hexaColor[7..2], 16)
        ));
    }
    public static System.Windows.Media.Color GetColorFromUInt(uint value)
        => System.Windows.Media.Color.FromArgb(
            (byte)(value >> 24),
            (byte)(value >> 16),
            (byte)(value >> 8),
            (byte)(value)
        );
}
public partial class WinFormsIntegrationTest : Window
{
    public bool ForceClose { get; set; } = false;
    public WinFormsIntegrationTest()
    {
        //this.Content = 
        AllowsTransparency = false;
        WindowInteropHelper = new WindowInteropHelper(this);
        Loaded += OnLoaded;
        
        SetValue(WindowChrome.WindowChromeProperty, WindowChrome);
        using var g = Graphics.FromImage(new Bitmap(1, 1));
        var dpix = g.DpiX;
    }
    WindowChrome WindowChrome { get; } = new WindowChrome
    {
        UseAeroCaptionButtons = true
    };
    public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
    {
        var handle = bmp.GetHbitmap();
        try
        {
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        finally { CustomPInvoke.DeleteObject(handle); }
    }

    void SetBackdrop(BackdropType BackdropType) => SetBackdrop((int)BackdropType);
    void SetBackdrop(int BackdropType)
    {
        CustomPInvoke.SetWindowAttribute(
            Handle,
            CustomPInvoke.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            BackdropType);
    }

    public WindowInteropHelper WindowInteropHelper { get; }
    public IntPtr Handle => WindowInteropHelper.Handle;

}
