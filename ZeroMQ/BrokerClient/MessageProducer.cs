using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerClient
{
    class MessageProducer
    {
        private static int _totalInstances;
        private readonly int _instance;

        public MessageProducer()
        {
            _instance = _totalInstances;
            Interlocked.Increment(ref _totalInstances);
        }

        public void Run(Socket socket, Encoding encoding)
        {
            const int requestsToSend = 10;
            for (var i = 0; i < requestsToSend; i++)
            {
                Console.WriteLine("Sending message {0}...", i);
                lock (socket)
                {
                    socket.Send(string.Format("Hello message {0} from producer {1}.", i, _instance), encoding);
                    socket.Recv();
                }
            }
            Console.WriteLine("Producer done!");
        }
    }
}