using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerWorker
{
    class MessageConsumer
    {
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;
        private static int _totalInstances;

        private readonly int _instance;

        public MessageConsumer()
        {
            _instance = _totalInstances;
            Interlocked.Increment(ref _totalInstances);
            Console.WriteLine("Starting consumer {0}...", _instance);
        }

        public void Run(Socket socket, object socketLock, ref int messagesConsumed)
        {
            while (true)
            {
                Thread.Sleep(1000);
                string message;
                lock (socketLock)
                {
                    message = socket.Recv(DefaultEncoding, 1000);
                }
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Received message {0} on consumer {1}", message, _instance);
                    Interlocked.Increment(ref messagesConsumed);
                    socket.Send();
                }
                else
                {
                    if (messagesConsumed > 0) break;
                }
            }
        }
    }
}