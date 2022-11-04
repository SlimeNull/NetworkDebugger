using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PropertyChanged;
using NetworkDebugger.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Wpf.Ui.Controls;
using NetworkDebugger.Utils;

namespace NetworkDebugger.ViewModels
{

    public partial class TcpServerViewModel : ObservableObject
    {
        
        public TcpServerViewModel()
        {
            
        }

        [ObservableProperty]
        private bool _optionExpanded = true;

        [ObservableProperty]
        private TcpListener? _listener;

        [ObservableProperty]
        private string? _listeningAddr;

        [ObservableProperty]
        private int _listeningPort = 11451;

        [ObservableProperty]
        private int _bufferSize = 1024;

        [ObservableProperty]
        private Encoding _defaultEncoding = EncodingUtils.AllSupportedEncoding[0];

        [DependsOn(nameof(Listener))]
        [ObservableProperty]
        public ObservableCollection<TcpClientCtrlViewModel> _clients =
            new ObservableCollection<TcpClientCtrlViewModel>();

        [DependsOn(nameof(Listener))]
        public string ListenerSwitchText => Listener == null ? "Listen" : "Close";

        [DependsOn(nameof(Listener))]
        public string ListenerStatus => Listener == null ? "Not listening" : "Listening";

        [DependsOn(nameof(Listener))]
        public bool Listening => Listener != null;

        private async Task ListenerLoop()
        {
            TcpListener? listener = Listener;
            while (listener != null && Listener == listener)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Clients.Add(new TcpClientCtrlViewModel(Clients, client, DefaultEncoding, 1024));
            }
        }

        [RelayCommand]
        public void SwitchListener()
        {
            if (Listener == null)
            {
                IPAddress? addr;
                if (string.IsNullOrWhiteSpace(ListeningAddr))
                {
                    addr = IPAddress.Any;
                }
                else if (!IPAddress.TryParse(ListeningAddr, out addr))
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
                    TcpListener listener = new TcpListener(new IPEndPoint(addr, ListeningPort));
                    listener.Start();

                    Listener = listener;
                    _ = ListenerLoop();

                    OptionExpanded = false;
                }
                catch (Exception ex)
                {
                    new MessageBox()
                    {
                        Title = "Cannot listen",
                        Content = ex.Message
                    }.ShowDialog();
                }
            }
            else
            {
                Listener.Stop();
                Listener = null;

                foreach (TcpClientCtrlViewModel client in Clients)
                    client.Dispose();

                Clients.Clear();
            }
        }


    }
}
