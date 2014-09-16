using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMQ;

namespace Broker
{
    class MessageBroker
    {
        private readonly BlockingCollection<byte[]> _queue = new BlockingCollection<byte[]>();

        public int QueueLength
        {
            get { return _queue.Count; }
        }

        public bool IsRunning { get; private set; }

        public void Run(Socket frontend, Socket backend, Socket monitor)
        {
            IsRunning = true;
            var defaultEncoding = Encoding.Unicode;
            var tasks = new[]
            {
                Task.Run(() => RequestReceiver(frontend, _queue, defaultEncoding)),
                Task.Run(() => RequestPusher(backend, _queue.GetConsumingEnumerable(), defaultEncoding)),
                Task.Run(() => MonitorResponder(monitor, _queue, defaultEncoding)),
            };
            Task.WaitAll(tasks);
            Console.WriteLine("Broker done!");
            IsRunning = false;
        }

        private void MonitorResponder(Socket monitor, BlockingCollection<byte[]> queue, Encoding encoding)
        {
            while (IsRunning)
            {
                monitor.Recv();
                var queueLength = queue.Count();
                Console.WriteLine("MONITOR - Received queue length monitor request - currently at {0}", queueLength);
                monitor.Send(string.Format("{0}", queueLength), encoding);
            }
        }

        private void RequestPusher(Socket destination, IEnumerable<byte[]> messages, Encoding encoding)
        {
            foreach (var message in messages)
            {
                var stringMessage = encoding.GetString(message);
                Console.WriteLine("PUSHER - Pulled message {0} from queue", stringMessage);
                destination.Send(message);
                destination.Recv();
            }
        }

        private void RequestReceiver(Socket source, BlockingCollection<byte[]> queue, Encoding encoding)
        {
            while (IsRunning)
            {
                var message = source.Recv();
                var stringMessage = encoding.GetString(message);
                Console.WriteLine("RECEIVER - Received message {0} from source", stringMessage);
                queue.Add(message);
                source.Send();

                if (stringMessage == "Done") break;
            }
            _queue.CompleteAdding();
        }
    }
}