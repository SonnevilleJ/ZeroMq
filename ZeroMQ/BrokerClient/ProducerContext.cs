using System;
using System.Text;
using System.Threading.Tasks;
using ZMQ;

namespace BrokerClient
{
    public class ProducerContext
    {
        public void Run()
        {
            var defaultEncoding = Encoding.Unicode;
            const string produceEndpoint = "tcp://localhost:5559";
            using (var context = new Context())
            using (var socket = context.Socket(SocketType.REQ))
            {
                socket.Connect(produceEndpoint);
                Console.WriteLine("Socket connected!");
                Task.WaitAll(new[]
                {
                    Task.Run(() => new MessageProducer().Run(socket, defaultEncoding)),
                    Task.Run(() => new MessageProducer().Run(socket, defaultEncoding)),
                    Task.Run(() => new MessageProducer().Run(socket, defaultEncoding))
                });
                socket.Send("Done", defaultEncoding);
                socket.Recv();
            }
            Console.ReadLine();
        }
    }
}