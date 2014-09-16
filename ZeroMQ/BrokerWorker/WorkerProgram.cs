namespace BrokerWorker
{
    class WorkerProgram
    {
        static void Main(string[] args)
        {
            new ConsumerContext().Run();
        }
    }
}
