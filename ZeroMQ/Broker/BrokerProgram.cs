using System;

namespace Broker
{
    static class BrokerProgram
    {
        static void Main(string[] args)
        {
            new MessageBroker().Run();
            Console.ReadLine();
        }
    }
}
