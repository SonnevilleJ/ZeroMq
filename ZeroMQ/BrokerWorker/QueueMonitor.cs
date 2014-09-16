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
        public void Monitor(Socket monitor, Encoding encoding, Action action)
        {
            var tasks = new List<Task>();

            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Monitor loop running...");
                monitor.Send();
                var queueLength = int.Parse(monitor.Recv(encoding));
                if (queueLength > 10)
                {
                    Console.WriteLine("High load detected - broker starting new worker process...");
                    tasks.Add(Task.Run(action));
                }
                if (queueLength == 0)
                {
                    break;
                }
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Monitor Done!");
        }
    }
}