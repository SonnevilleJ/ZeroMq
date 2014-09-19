using System;
using System.Text;
using System.Threading.Tasks;
using ZMQ;

namespace BrokerClient
{
    public class ProducerContext
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        public void Run()
        {
            const string produceEndpoint = "tcp://localhost:5559";
            using (var context = new Context())
            using (var socket = context.Socket(SocketType.REQ))
            {
                socket.Connect(produceEndpoint);
                Console.WriteLine("Socket connected!");

                Console.WriteLine("Press enter to start...");
                Console.ReadLine();

                Task.WaitAll(new[]
                {
                    Task.Run(() => new MessageProducer().Run(socket, _encoding)),
                    Task.Run(() => new MessageProducer().Run(socket, _encoding)),
                    Task.Run(() => new MessageProducer().Run(socket, _encoding))
                });
                socket.Send("Done", _encoding);
                socket.Recv();
            }
            Console.ReadLine();
        }
    }
}