using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PropertyChanged;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkDebugger.ViewModels
{
    public partial class HttpServerViewModel : ObservableObject
    {
        [ObservableProperty]
        HttpListener _listener = new HttpListener();

        [ObservableProperty]
        private bool _optionExpanded = true;

        [ObservableProperty]
        private string? _listeningAddr;

        [ObservableProperty]
        private int _listeningPort = 11451;

        [ObservableProperty]
        private bool _isListening;

        public HttpServerViewModel()
        {
            
        }

        [DependsOn(nameof(IsListening))]
        public string ListenerStatus => Listener.IsListening ? "Listening" : "Not listening";

        [DependsOn(nameof(IsListening))]
        public string ListenerSwitchText => Listener.IsListening ? "Stop" : "Listen";

        public async Task MainLoop()
        {
            while (Listener.IsListening)
            {
                HttpListenerContext context = await Listener.GetContextAsync();
            }
        }

        [RelayCommand]
        public void SwitchListener()
        {
            if (Listener.IsListening)
            {
                Listener.Stop();
            }
            else
            {
                Listener.Start();
                _ = MainLoop();
            }

            IsListening = Listener.IsListening;
        }
    }
}
