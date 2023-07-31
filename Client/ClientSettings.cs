using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class ClientSettings
    {
        readonly Socket _s;
        public delegate void ReceivedEventHandler(ClientSettings cs, string received);
        public event ReceivedEventHandler Received = delegate { };
        public event EventHandler Connected = delegate { };
        public delegate void DisconnectedEventHandler(ClientSettings cs);
        public event DisconnectedEventHandler Disconnected = delegate {};
        bool _connected;

        public ClientSettings()
        {
            _s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ip, int port)
        {
            try
            {
                var ep = new IPEndPoint(IPAddress.Parse(ip), port);
                _s.BeginConnect(ep, ConnectCallback, _s);
            }
            catch { }
        }

        public void Close()
        {
            _s.Dispose();
            _s.Close();
        }

        void ConnectCallback(IAsyncResult ar)
        {
            _s.EndConnect(ar);
            _connected = true;
            Connected(this, EventArgs.Empty);
            var buffer = new byte[_s.ReceiveBufferSize];
            _s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReadCallback, buffer);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var buffer = (byte[]) ar.AsyncState;
            var rec = _s.EndReceive(ar);
            if (rec != 0)
            {
                var data = Encoding.ASCII.GetString(buffer, 0, rec);
                Received(this, data);
            }
            else
            {
                Disconnected(this);
                _connected = false;
                Close();
                return;
            }
            _s.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReadCallback, buffer);
        }

        public void Send(string data)
        {
            try
            {
                var buffer = Encoding.ASCII.GetBytes(data);
                _s.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, buffer);
            }
            catch { Disconnected(this); }
        }

        void SendCallback(IAsyncResult ar)
        {
            _s.EndSend(ar);
        }
    }
}