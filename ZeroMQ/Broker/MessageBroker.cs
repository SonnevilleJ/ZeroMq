using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMQ;

namespace Broker
{
    public class MessageBroker
    {
        private readonly BlockingCollection<byte[]> _queue = new BlockingCollection<byte[]>();

        private bool _isRunning;

        public MessageBroker()
        {
            Console.WriteLine("Starting broker...");
        }

        public void Run(Socket frontend, Socket backend, Socket monitor)
        {
            _isRunning = true;
            var defaultEncoding = Encoding.UTF8;
            var tasks = new[]
            {
                Task.Run(() => RequestReceiver(frontend, _queue, defaultEncoding)),
                Task.Run(() => RequestPusher(backend, _queue.GetConsumingEnumerable(), defaultEncoding)),
                Task.Run(() => MonitorResponder(monitor, _queue, defaultEncoding)),
            };
            Task.WaitAll(tasks);
            Console.WriteLine("Broker done!");
        }

        private void MonitorResponder(Socket monitor, BlockingCollection<byte[]> queue, Encoding encoding)
        {
            while (_isRunning)
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
            _isRunning = false;
        }

        private void RequestReceiver(Socket source, BlockingCollection<byte[]> queue, Encoding encoding)
        {
            while (_isRunning)
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