using System;
using System.Threading.Tasks;

namespace BrokerWorker
{
    static class WorkerProgram
    {
        static void Main(string[] args)
        {
            var t1 = Task.Run(()=>new MessageConsumer().Run(1));
            var t2 = Task.Run(()=>new MessageConsumer().Run(2));
            var t3 = Task.Run(()=>new MessageConsumer().Run(3));
            Task.WaitAll(t1, t2, t3);
            Console.ReadLine();
        }
    }
}
