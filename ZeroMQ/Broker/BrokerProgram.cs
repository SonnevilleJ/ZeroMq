using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ZMQ;

namespace Broker
{
    static class BrokerProgram
    {
        private static readonly IList<Process> Processes = new List<Process>();

        static void Main(string[] args)
        {
            var messageBroker = new MessageBroker();

            using (var context = new Context())
            {
                var frontend = context.Socket(SocketType.REP);
                var backend = context.Socket(SocketType.REQ);
                var monitor = context.Socket(SocketType.REP);
                frontend.Bind("tcp://*:5559");
                Console.WriteLine("Bound to frontend.");
                backend.Bind("tcp://*:5560");
                Console.WriteLine("Bound to backend.");
                monitor.Bind("tcp://*:9999");

                var t1 = Task.Run(() => Monitor(messageBroker));
                var t2 = Task.Run(() => messageBroker.Run(frontend, backend, monitor));
                Task.WaitAll(t1, t2);
            }
            Console.ReadLine();
            foreach (var process in Processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                }
            }
        }

        private static void Monitor(MessageBroker messageBroker)
        {
            do
            {
                Thread.Sleep(1000);
                if (messageBroker.QueueLength > 10)
                {
                    Console.WriteLine("High load detected - broker starting new worker process...");
                    Processes.Add(Process.Start(@"C:\Bench\ZeroMQ\ZeroMQ\BrokerWorker\bin\x64\Debug\BrokerWorker.exe"));
                }
            } while (messageBroker.IsRunning);
        }
    }
}
