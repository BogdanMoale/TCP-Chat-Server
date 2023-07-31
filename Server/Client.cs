using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Client
    {
        public delegate void ClientReceivedHandler(Client sender, byte[] data);
        public delegate void ClientDisconnectedHandler(Client sender);
        public event ClientReceivedHandler Received;
        public event ClientDisconnectedHandler Disconnected;

        public IPEndPoint Ip { get; private set; }

        public Socket _socket;

        public Client(Socket accepted)
        {
            _socket = accepted;
            Ip = (IPEndPoint) _socket.RemoteEndPoint;
            _socket.BeginReceive(new byte[] {0}, 0, 0, 0, Callback, null);
        }

        void Callback(IAsyncResult ar)
        {
            try
            {
                _socket.EndReceive(ar);
                var buffer = new byte[_socket.ReceiveBufferSize];
                var rec = _socket.Receive(buffer, buffer.Length, 0);
                if (rec < buffer.Length)
                {
                    Array.Resize(ref buffer, rec);
                }
                if (Received != null)
                {
                    Received(this, buffer);
                }
                _socket.BeginReceive(new byte[] { 0 }, 0, 0, 0, Callback, null);
               
            }
            catch (Exception)
            {
                Close();
                if (Disconnected != null)
                {
                    Disconnected(this);
                }
            }
        }

        public void Send(string data)
        {
            var buffer = Encoding.ASCII.GetBytes(data);
            _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, ar => _socket.EndSend(ar), buffer);
        }

        public void Close()
        {
            _socket.Dispose();
            _socket.Close();
        }
    }
}