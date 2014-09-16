using System;

namespace BrokerWorker
{
    static class WorkerProgram
    {
        static void Main(string[] args)
        {
            new MessageConsumer().Run("tcp://localhost:5560");
            Console.ReadLine();
        }
    }
}
