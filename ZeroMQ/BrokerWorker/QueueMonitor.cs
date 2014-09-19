using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZMQ;

namespace BrokerWorker
{
    class QueueMonitor
    {
        public void Monitor(Socket monitor, Encoding encoding, Action action, ref int messagesConsumed)
        {
            var tasks = new List<Task>();

            while (true)
            {
                Thread.Sleep(3000);
                monitor.Send();
                var queueLength = int.Parse(monitor.Recv(encoding));
                if (queueLength > 10)
                {
                    Console.WriteLine("High load detected - broker starting new worker process...");
                    for(var i = 0; i < queueLength % 10; i++)
                    {
                    tasks.Add(Task.Run(action));
                }
                }

                if (queueLength == 0 && messagesConsumed > 0) break;
            }
            Console.WriteLine("Queue empty... Waiting for {0} consumers to finish.", tasks.Count);
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Monitor Done!");
        }
    }
}