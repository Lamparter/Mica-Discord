using CustomPInvoke;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
#if WINDOWS10_0_17763_0_OR_GREATER
using Windows.Storage;
#endif

namespace MicaDiscord;
#if WINDOWS10_0_17763_0_OR_GREATER
static class Setting {
    static ApplicationDataContainer ApplicationSetting = ApplicationData.Current.LocalSettings;
    public static void Save() { /* Do Nothing */ }
    public static bool ReplaceDiscordBackground
    {
        get => (bool)(ApplicationSetting.Values[nameof(ReplaceDiscordBackground)] ?? false);
        set => ApplicationSetting.Values[nameof(ReplaceDiscordBackground)] = value;
    }
    public static bool UseBackdropAnyway
    {
        get => (bool)(ApplicationSetting.Values[nameof(UseBackdropAnyway)] ?? false);
        set => ApplicationSetting.Values[nameof(UseBackdropAnyway)] = value;
    }
    public static bool UseSystemTray
    {
        get => (bool)(ApplicationSetting.Values[nameof(UseSystemTray)] ?? true);
        set => ApplicationSetting.Values[nameof(UseSystemTray)] = value;
    }
    public static bool EnableDevTools
    {
        get => (bool)(ApplicationSetting.Values[nameof(EnableDevTools)] ?? false);
        set => ApplicationSetting.Values[nameof(EnableDevTools)] = value;
    }
    public static bool ModeAwareCSS
    {
        get => (bool)(ApplicationSetting.Values[nameof(ModeAwareCSS)] ?? true);
        set => ApplicationSetting.Values[nameof(ModeAwareCSS)] = value;
    }
    public static string? BackdropType
    {
        get => (string?)(ApplicationSetting.Values[nameof(BackdropType)] ?? "Mica");
        set => ApplicationSetting.Values[nameof(BackdropType)] = value;
    }
}
#endif
/// <summary>
/// Interaction logic for SettingsDialog.xaml
/// </summary>
public partial class SettingsDialog : Grid
{
    public bool RequiresReload { get; private set; } = false;
    public void ResetRequiresReload() => RequiresReload = false;
#if !WINDOWS10_0_17763_0_OR_GREATER
    static Settings Setting = Settings.Default;
#endif
    public SettingsDialog()
    {
        InitializeComponent();
        if (!MainWindow.NotSupportedBuild) OSWarning.Visibility = Visibility.Collapsed;
        IsVisibleChanged += (_, _) =>
        {
            if (IsVisible)
            {
#if DEBUG
                ReloadCSSButton.Visibility = Visibility.Hidden;
#endif
                Backdrop.SelectedItem = Enum.Parse(typeof(BackdropType), Settings.Default.BackdropType);
                ReplaceBackgroundToggle.Content = Setting.ReplaceDiscordBackground ? "Disable" : "Enable";
                UseBackdropAnyway.Content = Setting.UseBackdropAnyway ? "Use Solid Color" : "Enable Anyway";
                Systray.IsChecked = Setting.UseSystemTray;
                DevTools.Content = Setting.EnableDevTools ? "Disable" : "Enable";
                ModeAwareCSS.IsChecked = Setting.ModeAwareCSS;
                RequiresReload = false;
            }
        };
        Backdrop.SelectionChanged += (_, _) =>
        {
            Setting.BackdropType = Backdrop.SelectedItem.ToString();
            Setting.Save();
            OnSettingsChanged?.Invoke();
        };
        ReplaceBackgroundToggle.Click += (_, _) =>
        {
            bool newValue = !Setting.ReplaceDiscordBackground;
            if (newValue)
            {
                if (MessageBox.Show(
                
                    caption: "Warning",
                    messageBoxText: "Replacing Discord Background might violate Discord TOS because this app runs JavaScript code to " +
                    "inject CSS when the page refresh! Please review Discord TOS first and use it with caution!\n" +
                    "Are you sure you still want to continue?\n\n" +
                    "NOTE: We do not take any responsibility of anything happening to your Discord account regardless of clicking Yes or No!",
                    button: MessageBoxButton.YesNo,
                    icon: MessageBoxImage.Warning
                    )
                != MessageBoxResult.Yes)
                    return;
            }
            Setting.ReplaceDiscordBackground = newValue;
            ReplaceBackgroundToggle.Content = newValue ? "Disable" : "Enable";
            Setting.Save();
            OnSettingsChanged?.Invoke();
            RequiresReload = true;
        };
        DevTools.Click += (_, _) =>
        {
            bool newValue = !Settings.Default.EnableDevTools;
            if (newValue)
            {
                if (MessageBox.Show(

                    caption: "Warning",
                    messageBoxText: "If somebody asked you to turn this on (or you did turn it on yourself), please remember that " +
                    "you can get your discord account hacked if you copy and paste malicious code in the DevTools Console.\n" +
                    "Are you sure you still want to continue?\n\n" +
                    "NOTE: We do not take any responsibility of anything happening to your Discord account regardless of clicking Yes or No!",
                    button: MessageBoxButton.YesNo,
                    icon: MessageBoxImage.Warning
                    )
                != MessageBoxResult.Yes)
                    return;
            }
            Setting.EnableDevTools = newValue;
            DevTools.Content = newValue ? "Disable" : "Enable";
            Setting.Save();
            OnSettingsChanged?.Invoke();
            RequiresReload = true;
        };
        UseBackdropAnyway.Click += (_, _) =>
        {
            bool newValue = !Settings.Default.UseBackdropAnyway;
            if (newValue)
            {
                if (MessageBox.Show(

                    caption: "Warning",
                    messageBoxText: "This might cause an unexpected behavior if your build does not support the feature!",
                    button: MessageBoxButton.YesNo,
                    icon: MessageBoxImage.Warning
                    )
                != MessageBoxResult.Yes)
                    return;
            }
            Setting.UseBackdropAnyway = newValue;
            UseBackdropAnyway.Content = newValue ? "Use Solid Color" : "Enable Anyway";
            Setting.Save();
            OnSettingsChanged?.Invoke();
            RequiresReload = true;
        };
    }
    public event Action? OnClose;
    private void CloseDialog(object sender, RoutedEventArgs e) => OnClose?.Invoke();
    public event Action? OnSettingsChanged;

    private void OpenAppFolder(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = AppDomain.CurrentDomain.BaseDirectory,
            UseShellExecute = true
        });
    }

    private void SystrayToggled(object sender, RoutedEventArgs e)
    {
        Setting.UseSystemTray = Systray.IsChecked ?? false;
        Setting.Save();
    }
    private void ModeAwareCSSToggled(object sender, RoutedEventArgs e)
    {
        Setting.ModeAwareCSS = ModeAwareCSS.IsChecked ?? false;
        Setting.Save();
        RequiresReload = true;
    }

    private void ReloadCSS(object sender, RoutedEventArgs e)
    {
#if !DEBUG
        MainWindow.DefinedCSS = System.IO.File.ReadAllText("./The CSS.css");
        RequiresReload = true;
#endif
    }
}
