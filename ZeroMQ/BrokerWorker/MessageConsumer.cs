using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerWorker
{
    class MessageConsumer
    {
        private static readonly object SyncRoot = new object();
        private static int _totalConsumers;
        private static readonly Encoding DefaultEncoding = Encoding.Unicode;
        private readonly int _id;

        public MessageConsumer()
        {
            lock (SyncRoot)
            {
                _id = _totalConsumers++;
            }
        }

        public void Run(Socket socket, object socketLock)
        {
            while (true)
            {
                Thread.Sleep(100);
                string message;
                lock (socketLock)
                {
                    message = socket.Recv(DefaultEncoding, 200);
                }
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Received message {0} on consumer {1}", message, _id);
                    socket.Send();
                }
                else
                {
                    break;
                }
            }
        }
    }
}