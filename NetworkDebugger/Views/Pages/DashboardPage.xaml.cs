using NetworkDebugger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace NetworkDebugger.Views.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : UiPage, INavigableView<DashboardViewModel>
    {
        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            
            InitializeComponent();
        }

        public DashboardViewModel ViewModel { get; }
    }
}
