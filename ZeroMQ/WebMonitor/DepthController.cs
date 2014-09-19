using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ZMQ;

namespace WebMonitor
{
    public class DepthController : ApiController
    {
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly Context _context;
        private readonly Socket _socket;

        public DepthController()
        {
            _context = new Context();
            _socket = _context.Socket(SocketType.REQ);
            _socket.Connect(string.Format("tcp://localhost:9999"));
            Console.WriteLine("DepthController connected!");
        }

        [HttpGet]
        public HttpResponseMessage depth()
        {
            _socket.Send();
            var queueLength = int.Parse(_socket.Recv(_encoding));

            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(queueLength.ToString()) };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_socket != null) _socket.Dispose();
                if (_context != null) _context.Dispose();
            }
        }
    }
}