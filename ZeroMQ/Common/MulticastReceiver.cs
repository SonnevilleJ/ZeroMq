using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common 
{
    public class MulticastReceiver 
    {
        private const int BufferSize = 1024;
        private readonly Encoding _encoding;

        public MulticastReceiver(Encoding encoding)
        {
            _encoding = encoding;
        }

        public string Receive(string mcastGroup, int port)
        {
            var bytes = new byte[BufferSize];
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                var endPoint = new IPEndPoint(IPAddress.Any, port);
                var groupIp = IPAddress.Parse(mcastGroup);
                var multicastOption = new MulticastOption(groupIp, IPAddress.Any);
                
                socket.Bind(endPoint);
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);

                socket.Receive(bytes, BufferSize, SocketFlags.None);
                socket.Close();
            }
            var nonNullBytes = bytes.Where(b => b != 0).ToArray();
            var message = _encoding.GetString(nonNullBytes);
            return message;
        }
    }
}