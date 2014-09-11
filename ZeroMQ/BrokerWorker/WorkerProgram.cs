namespace BrokerWorker
{
    static class WorkerProgram
    {
        static void Main(string[] args)
        {
            new MessageConsumer().Run();
        }
    }
}
