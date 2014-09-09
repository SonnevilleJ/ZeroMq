using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZMQ;

namespace Broker
{
    static class BrokerProgram
    {
        private static readonly BlockingCollection<byte[]> RequestQueue = new BlockingCollection<byte[]>(); 
//        private static readonly BlockingCollection<byte[]> ResponseQueue = new BlockingCollection<byte[]>();
//        private static int countReceived = 0;
//        private static int countPushed = 0;
        private static readonly Encoding DefaultEncoding = Encoding.Unicode;

        static void Main(string[] args)
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
                        Task.Run(() => RequestReceiver(source, destination, RequestQueue)),
                        Task.Run(() => RequestPusher(source, destination, RequestQueue)),
//                        Task.Run(() => RequestReceiver(destination, ResponseQueue)),
//                        Task.Run(() => RequestPusher(source, ResponseQueue)),
                    };
                    Task.WaitAll(tasks);
                }
            }
        }

        private static void RequestPusher(Socket source, Socket destination, BlockingCollection<byte[]> queue)
        {
            foreach (var message in queue.GetConsumingEnumerable())
            {
//                Console.WriteLine("PUSHER - Pulled message {0} from queue", countPushed);
                destination.Send(message);
//                Console.WriteLine("PUSHER - Pushed message {0} to destination", countPushed);
                destination.Recv();
//                Console.WriteLine("PUSHER - Received response from destination");
//                countPushed++;
            }
        }

        private static void RequestReceiver(Socket source, Socket destination, BlockingCollection<byte[]> queue)
        {
            while (true)
            {
                var message = source.Recv();
//                Console.WriteLine("RECEIVER - Received message {0} from source", countReceived);
                queue.Add(message);
//                Console.WriteLine("RECEIVER - Pushed message {0} into queue", countReceived);
                source.Send();
//                Console.WriteLine("RECEIVER - Sent response to source");
//                countReceived++;
            }
//            queue.CompleteAdding();
        }
    }
}
