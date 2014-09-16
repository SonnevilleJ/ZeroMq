using System;
using System.Threading.Tasks;
using ZMQ;

namespace Broker
{
    class BrokerContext
    {
        public void Run()
        {
            using (var context = new Context())
            {
                using (var frontend = context.Socket(SocketType.REP))
                using (var backend = context.Socket(SocketType.REQ))
                using (var monitor = context.Socket(SocketType.REP))
                {
                    frontend.Bind("tcp://*:5559");
                    Console.WriteLine("Bound to frontend.");
                    backend.Bind("tcp://*:5560");
                    Console.WriteLine("Bound to backend.");
                    monitor.Bind("tcp://*:9999");

                    var task = Task.Run(() => new MessageBroker().Run(frontend, backend, monitor));
                    task.Wait();
                }
            }
            Console.ReadLine();
        }
    }
}