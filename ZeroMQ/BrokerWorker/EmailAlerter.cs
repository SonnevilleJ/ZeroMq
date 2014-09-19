using System.Net.Mail;
using System.Text;
using System.Threading;
using ZMQ;

namespace BrokerWorker
{
    class EmailAlerter
    {
        public void Run(Socket socket, Encoding encoding, ref int messagesConsumed)
        {
            while (true)
            {
                Thread.Sleep(5000);
                socket.Send();
                var queueLength = int.Parse(socket.Recv(encoding));
                if (queueLength > 10)
                {
                    SendEmail("High load detected", string.Format("Current queue depth is {0}", queueLength));
                }

                if (queueLength == 0 && messagesConsumed > 0)
                {
                    SendEmail("Queue emptied", "All messages in queue have been processed.");
                    break;
                }
            }
        }

        private void SendEmail(string subject, string body)
        {
            var message = new MailMessage("QueueDemo@ZeroMQRocks.com", "SonnevilleJohn@JohnDeere.com", subject, body);
            new SmtpClient("mail.dx.deere.com").Send(message);
        }
    }
}