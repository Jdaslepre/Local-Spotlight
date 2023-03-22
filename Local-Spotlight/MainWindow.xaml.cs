using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.ApplicationModel;
using Windows.UI;
using WinRT;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Local_Spotlight
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();
            SetWindowSettings();
            ((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;
        }

        private void Copy()
        {
            var AppDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Pictures\\Local-Spotlight");
            string WhatToCopy = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Packages\\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\\LocalState\\Assets";
            string WhereToCopy = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Pictures\\Local-Spotlight";
            string[] Files = Directory.GetFiles(WhatToCopy, "*");
            if (AppDir.Exists)
            {
                foreach (string file in Files)
                {
                    string fileName = file.Substring(WhatToCopy.Length);
                    File.Copy(Path.Combine(WhatToCopy + fileName), Path.Combine(WhereToCopy + fileName + ".png"), true);
                }
            }
            else
            {
                Directory.CreateDirectory(AppDir.ToString());
                foreach (string file in Files)
                {
                    string fileName = file.Substring(WhatToCopy.Length);
                    File.Copy(Path.Combine(WhatToCopy + fileName), Path.Combine(WhereToCopy + fileName + ".png"), true);
                }
            }
        }

        private void Conv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Thread(new ThreadStart(Copy)).Start();
                info.IsOpen = true;
                info.Severity = InfoBarSeverity.Success;

                var AppDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Pictures\\Local-Spotlight");
                if (AppDir.Exists)
                {
                    info.Title = "Directory already exists, copying new images";
                }
                else
                {
                    info.Title = "Done!";
                }
       
                info.Message = "There will be images unrelated to wallpapers, this is caused by the windows content delivery manager.";
            }
            catch(Exception ex)
            {
                info.IsOpen = true;
                info.Severity = InfoBarSeverity.Error;
                info.Title = "Well...";
                info.Message = $"Something went wrong. {ex.Message}";
            }   
        }

        private void FileLocation_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Pictures\\Local-Spotlight");
        }

        private void SetWindowSettings()
        {
            var isDark = false;
            isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            Title = Package.Current.DisplayName;
            AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            AppWindow.TitleBar.ButtonHoverBackgroundColor = isDark ? Color.FromArgb(15, 255, 255, 255) : Color.FromArgb(10, 0, 0, 0);
            AppWindow.TitleBar.ButtonPressedBackgroundColor = isDark ? Color.FromArgb(10, 255, 255, 255) : Color.FromArgb(6, 0, 0, 0);
            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
      
        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            var isDark = false;
            isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            AppWindow.TitleBar.ButtonHoverBackgroundColor = isDark ? Color.FromArgb(15, 255, 255, 255) : Color.FromArgb(10, 0, 0, 0);
            AppWindow.TitleBar.ButtonPressedBackgroundColor = isDark ? Color.FromArgb(10, 255, 255, 255) : Color.FromArgb(6, 0, 0, 0);
            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}
