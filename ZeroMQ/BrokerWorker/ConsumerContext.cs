using System;
using System.Text;
using System.Threading.Tasks;
using Common;
using ZMQ;

namespace BrokerWorker
{
    class ConsumerContext
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        public void Run()
        {
            var socketLock = new object();
            var messagesConsumed = 0;
            using (var context = new Context())
            using (var brokerConsumeSocket = context.Socket(SocketType.REP))
            using (var brokerAnnounceSocket = context.Socket(SocketType.REQ))
            using (var scalerAnnounceSocket = context.Socket(SocketType.REP))
            {
                var receiver = new MulticastReceiver(_encoding);
                var brokerIp = receiver.Receive("239.20.20.20", 8888);

                brokerAnnounceSocket.Connect(string.Format("tcp://{0}:9999", brokerIp));
                scalerAnnounceSocket.Bind(string.Format("tcp://*:7777"));
                Console.WriteLine("Scaler connected!");
                brokerConsumeSocket.Connect(string.Format("tcp://{0}:5560", brokerIp));
                Console.WriteLine("Consumer connected!");

                Action consumer = () => new MessageConsumer().Run(brokerConsumeSocket, socketLock, ref messagesConsumed);
                Action monitor = () => new QueueScaler().Run(brokerAnnounceSocket, _encoding, consumer, scalerAnnounceSocket, ref messagesConsumed);
                Action alerter = () => new EmailAlerter().Run(brokerAnnounceSocket, _encoding, ref messagesConsumed);

                var tasks = new[]
                {
                    Task.Run(monitor),
                    Task.Run(alerter)
                };
                Task.WaitAll(tasks);
            }
            Console.WriteLine("Consumer done!");
            Console.WriteLine("Consumed {0} messages!", messagesConsumed);
            Console.ReadLine();
        }
    }
}