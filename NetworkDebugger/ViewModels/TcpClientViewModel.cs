using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetworkDebugger.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace NetworkDebugger.ViewModels
{
    public partial class TcpClientViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _optionExpanded = true;
        [ObservableProperty]
        private string? _remoteAddr;
        [ObservableProperty]
        private int _remotePort = 11451;
        [ObservableProperty]
        private int _bufferSize = 1024;

        [ObservableProperty]
        private Encoding _defaultEncoding = EncodingUtils.AllSupportedEncoding[0];

        [ObservableProperty]
        private ObservableCollection<TcpClientCtrlViewModel> _clients =
            new ObservableCollection<TcpClientCtrlViewModel>();

        [RelayCommand]
        public void Connect()
        {
            IPAddress? addr;
            if (string.IsNullOrWhiteSpace(RemoteAddr))
            {
                addr = IPAddress.Loopback;
            }
            else if (!IPAddress.TryParse(RemoteAddr, out addr))
            {
                new MessageBox()
                {
                    Title = "Invalid IP address",
                    Content = "Specified IP address is not valid"
                }.ShowDialog();
                return;
            }
            
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(addr, _remotePort));

                Clients.Add(new TcpClientCtrlViewModel(Clients, client, _defaultEncoding, _bufferSize));
            }
            catch
            {
                
            }
        }
    }
}
