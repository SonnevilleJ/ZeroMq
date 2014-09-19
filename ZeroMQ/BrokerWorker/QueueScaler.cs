using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZMQ;

namespace BrokerWorker
{
    class QueueScaler
    {
        private readonly List<Task> _tasks = new List<Task>();
        private static object syncroot = new object();
        private static int ConsumerCount = 0;
        private readonly List<Task> _tasks;
        private bool _isRunning;

        public void Run(Socket brokerAnnounceSocket, Encoding encoding, Action action, Socket scalerAnnounceSocket, ref int messagesConsumed)
        {
            _isRunning = true;
            var announcer = Task.Run(() => ScalerAnnouncer(scalerAnnounceSocket, encoding));
            while (_isRunning)
            {
                Thread.Sleep(3000);
                brokerAnnounceSocket.Send();
                var queueLength = int.Parse(brokerAnnounceSocket.Recv(encoding));
                if (queueLength > 10)
                {
                    Console.WriteLine("High load detected - broker starting new worker process...");
                    for(var i = 0; i < queueLength % 10; i++)
                    {
                        _tasks.Add(Task.Run(action));
                    }
                }

//                if (queueLength == 0 && messagesConsumed > 0)
//                {
//                    _isRunning = false;
//                    break;
//                }
            }
            announcer.Wait();
            Console.WriteLine("Queue empty... Waiting for {0} consumers to finish.", _tasks.Count);
            Task.WaitAll(_tasks.ToArray());
            Console.WriteLine("Scaler Done!");
        }

        private void ScalerAnnouncer(Socket scalerAnnounceSocket, Encoding encoding)
        {
            while (_isRunning)
            {
                scalerAnnounceSocket.Recv();
                Console.WriteLine("SCALER - Received consumer count request - currently at {0}", ConsumerCount);
                scalerAnnounceSocket.Send(string.Format("{0}", ConsumerCount), encoding);
            }
        }

        public static void RegisterConsumerDeath()
        {
            lock (syncroot)
            {
                ConsumerCount--;
            }
        }
    }
}