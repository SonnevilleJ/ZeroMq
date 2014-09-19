namespace WebMonitor
{
    static class WebMonitorProgram
    {
        static void Main(string[] args)
        {
            new WebsiteContext().Run();
        }
    }
}
