using System;
using System.Diagnostics;

namespace BrokerWorker
{
    static class WorkerProgram
    {
        static void Main(string[] args)
        {
            new MessageConsumer().Run(Process.GetCurrentProcess().Id);
            Console.ReadLine();
        }
    }
}
