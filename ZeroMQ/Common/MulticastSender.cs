using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    public class MulticastSender
    {
        private readonly Encoding _encoding;

        public MulticastSender(Encoding encoding)
        {
            _encoding = encoding;
        }

        public void Send(string mcastGroup, int port, string message, int ttl = 1, int rep = 1)
        {
            try
            {
                Console.WriteLine("Multicast broadcast on Group: {0} Port: {1} TTL: {2} Message:{3}", mcastGroup, port, ttl, message);
                var ipAddress = IPAddress.Parse(mcastGroup);

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    var multicastOption = new MulticastOption(ipAddress);
                    socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
                    socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, ttl);

                    var bytes = _encoding.GetBytes(message);

                    var endPoint = new IPEndPoint(ipAddress, port);

                    socket.Connect(endPoint);

                    for (var x = 0; x < rep; x++)
                    {
                        socket.Send(bytes, bytes.Length, SocketFlags.None);
                    }

                    socket.Close();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
