using ZMQ;

namespace Broker
{
    static class BrokerProgram
    {
        static void Main(string[] args)
        {
            using (var context = new Context())
            {
                using (Socket frontend = context.Socket(SocketType.ROUTER), backend = context.Socket(SocketType.DEALER))
                {
                    frontend.Bind("tcp://*:5559");
                    backend.Bind("tcp://*:5560");

                    while(true)
                    {
                        frontend.Forward(backend);
                        backend.Forward(frontend);
                    }
                }
            }
        }
    }
}
