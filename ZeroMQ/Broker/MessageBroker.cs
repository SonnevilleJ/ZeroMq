using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Socket = ZMQ.Socket;

namespace Broker
{
    public class MessageBroker
    {
        private readonly BlockingCollection<byte[]> _queue = new BlockingCollection<byte[]>();

        private readonly Encoding _encoding = Encoding.UTF8;
        private bool _isRunning;

        public MessageBroker()
        {
            Console.WriteLine("Starting broker...");
        }

        public void Run(Socket frontend, Socket backend, Socket monitor)
        {
            _isRunning = true;
            var tasks = new[]
            {
                Task.Run(()=> Heartbeat(_encoding)),
                Task.Run(() => RequestReceiver(frontend, _queue, _encoding)),
                Task.Run(() => RequestPusher(backend, _queue.GetConsumingEnumerable(), _encoding)),
                Task.Run(() => MonitorResponder(monitor, _queue, _encoding)),
            };
            Task.WaitAll(tasks);
            Console.WriteLine("Broker done!");
        }

        private void Heartbeat(Encoding encoding)
        {
            var ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            if (ipAddress == null) throw new InvalidOperationException("No IP address???");

            var sender = new MulticastSender(encoding);
            while (_isRunning)
            {
                Thread.Sleep(5000);
                sender.Send("239.20.20.20", 8888, ipAddress.ToString());
            }
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