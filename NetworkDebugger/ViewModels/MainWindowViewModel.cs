using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace NetworkDebugger.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "NetworkDebugger";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Dashboard",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home12,
                    PageType = typeof(Views.Pages.DashboardPage),
                },
                new NavigationItem()
                {
                    Content = "HTTP Server",
                    PageTag = "http_server",
                    Icon = SymbolRegular.Server20,
                    PageType = typeof(Views.Pages.HttpServerPage),
                },
                new NavigationItem()
                {
                    Content = "TCP Server",
                    PageTag = "tcp_server",
                    Icon = SymbolRegular.Server20,
                    PageType = typeof(Views.Pages.TcpServerPage),
                },
                new NavigationItem()
                {
                    Content = "TCP Client",
                    PageTag = "tcp_client",
                    Icon = SymbolRegular.SerialPort20,
                    PageType = typeof(Views.Pages.TcpClientPage),
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }
    }
}
