using System;
using System.Threading;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace WebMonitor
{
    class WebsiteContext
    {
        private static readonly Uri Endpoint = new Uri("http://localhost:80");

        public void Run()
        {
            var configuration = new HttpSelfHostConfiguration(Endpoint);
            object defaultControllerName = "Depth";
            configuration.Routes.MapHttpRoute("API Default", "{action}", new { controller = defaultControllerName });

            using (var server = new HttpSelfHostServer(configuration))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Server running...");

                Thread.Sleep(new TimeSpan(0, 10, 0));

                server.CloseAsync().Wait();
            }
        }
    }
}