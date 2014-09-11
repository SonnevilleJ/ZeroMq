using System;
using System.Threading.Tasks;

namespace BrokerClient
{
    static class ClientProgram
    {
        static void Main(string[] args)
        {
            var t1 = Task.Run(()=>new MessageProducer().Run(1));
            var t2 = Task.Run(()=>new MessageProducer().Run(2));
            var t3 = Task.Run(()=>new MessageProducer().Run(3));
            Task.WaitAll(t1, t2, t3);
            Console.ReadLine();
        }
    }
}
