using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Linq;
using NetworkDebugger.Models;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using NetworkDebugger.Utils;

namespace NetworkDebugger.ViewModels
{
    public partial class TcpClientCtrlViewModel : ObservableObject, IDisposable
    {
        private byte[] _buffer;
        private bool _disposed = false;
        private MemoryStream _dataSent = new MemoryStream();
        private MemoryStream _dataReceived = new MemoryStream();
        private readonly ICollection<TcpClientCtrlViewModel> _container;

        [ObservableProperty]
        private bool _useEscapeString;
        [ObservableProperty]
        private string? _sendBuffer;
        [ObservableProperty]
        private Encoding _encoding;

        [ObservableProperty]
        private DataDisplayMode _displayMode;

        public static DataDisplayMode[] AllDisplayModes => Enum.GetValues<DataDisplayMode>();

        public TcpClient Client { get; }

        public string Name => Client.Client.RemoteEndPoint?.ToString() ?? "Not connected";

        public byte[]? ReceivedBytes { get; private set; }
        public byte[]? SentBytes { get; private set; }

        [DependsOn(nameof(ReceivedBytes), nameof(Encoding))]
        public string ReceivedText => ReceivedBytes != null ? Encoding.GetString(ReceivedBytes) : string.Empty;

        [DependsOn(nameof(SentBytes), nameof(Encoding))]
        public string SentText => SentBytes != null ? Encoding.GetString(SentBytes) : string.Empty;

        [DependsOn(nameof(ReceivedBytes), nameof(Encoding))]
        public string ReceivedHex => ReceivedBytes != null ? Convert.ToHexString(ReceivedBytes) : string.Empty;

        [DependsOn(nameof(SentBytes), nameof(Encoding))]
        public string SentHex => SentBytes != null ? Convert.ToHexString(SentBytes) : string.Empty;


        [DependsOn(nameof(DisplayMode), nameof(ReceivedBytes), nameof(Encoding))]
        public string ReceivedContent => DisplayMode switch
        {
            DataDisplayMode.Hex => ReceivedHex,
            _ => ReceivedText
        };

        [DependsOn(nameof(DisplayMode), nameof(SentBytes), nameof(Encoding))]
        public string SentContent => DisplayMode switch
        {
            DataDisplayMode.Hex => SentHex,
            _ => SentText
        };


        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Client.Dispose();
            }
        }

        public TcpClientCtrlViewModel(ICollection<TcpClientCtrlViewModel> container, TcpClient client, Encoding encoding, int bufferSize)
        {
            _container = container;
            _buffer = new byte[bufferSize];
            _encoding = encoding;
            Client = client;

            _ = MainLoop();
        }
        private async Task MainLoop()
        {
            NetworkStream netStream = Client.GetStream();
            while (!_disposed)
            {
                try
                {
                    int count = await netStream.ReadAsync(_buffer, 0, _buffer.Length);
                    await _dataReceived.WriteAsync(_buffer, 0, count);

                    ReceivedBytes = _dataReceived.ToArray();
                }
                catch (IOException)
                {
                    Close();
                }
            }
        }

        [RelayCommand]
        public async void Send()
        {
            if (SendBuffer is string str)
            {
                string tosend = UseEscapeString ? StrUtils.EscapeString(str) : str;
                byte[] binary = Encoding.GetBytes(tosend);
                await Client.GetStream().WriteAsync(binary);
                _dataSent.Write(binary);

                SendBuffer = string.Empty;
                SentBytes = _dataSent.ToArray();
            }
        }

        [RelayCommand]
        public async void SendFile()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select file to send",
                Filter = "Any file|*.*",
                Multiselect = false,
                CheckFileExists = true,
            };

            if (ofd.ShowDialog().GetValueOrDefault(false))
            {
                FileStream file = File.OpenRead(ofd.FileName);
                await file.CopyToAsync(_dataSent);
                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(Client.GetStream());
                
                SentBytes = _dataSent.ToArray();
            }
        }

        [RelayCommand]
        public void Close()
        {
            _container.Remove(this);
            Dispose();
        }
    }
}
