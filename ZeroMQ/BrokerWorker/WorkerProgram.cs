using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerWorker
{
    static class WorkerProgram
    {
        private static readonly Encoding DefaultEncoding = Encoding.Unicode;

        static void Main(string[] args)
        {
            using (var context = new Context())
            {
                using (var socket = context.Socket(SocketType.REP))
                {
                    Thread.Sleep(5000);
                    socket.Connect("tcp://localhost:5560");
                    Console.WriteLine("Socket connected!");

                    while (true)
                    {
                        var message = socket.Recv(DefaultEncoding);
                        Console.WriteLine("Received message: {0}", message);
                        socket.Send();
//                        Console.WriteLine("Sent response.");
                    }
                }
            }
        }
    }
}
