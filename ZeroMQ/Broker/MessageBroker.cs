using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using ZMQ;

namespace Broker
{
    class MessageBroker
    {
        private readonly BlockingCollection<byte[]> _queue = new BlockingCollection<byte[]>();
        private readonly Encoding _defaultEncoding = Encoding.Unicode;

        public void Run()
        {
            using (var context = new Context())
            {
                using (var frontend = context.Socket(SocketType.REP))
                using (var backend = context.Socket(SocketType.REQ))
                {
                    var source = frontend;
                    var destination = backend;

                    source.Bind("tcp://*:5559");
                    Console.WriteLine("Bound to frontend.");
                    destination.Bind("tcp://*:5560");
                    Console.WriteLine("Bound to backend.");

                    var tasks = new[]
                    {
                        Task.Run((Action) (() => RequestReceiver(source, _queue))),
                        Task.Run((Action) (() => RequestPusher(destination, _queue))),
                    };
                    Task.WaitAll(tasks);
                }
            }
        }

        private void RequestPusher(Socket destination, BlockingCollection<byte[]> queue)
        {
            foreach (var message in queue.GetConsumingEnumerable())
            {
                Console.WriteLine("PUSHER - Pulled message {0} from queue", _defaultEncoding.GetString(message));
                destination.Send(message);
                destination.Recv();
            }
        }

        private void RequestReceiver(Socket source, BlockingCollection<byte[]> queue)
        {
            while (true)
            {
                var message = source.Recv();
                Console.WriteLine("RECEIVER - Received message {0} from source", _defaultEncoding.GetString(message));
                queue.Add(message);
                source.Send();
            }
        }
    }
}