import org.zeromq.ZMQ;

public class MessageProducer implements Runnable
{
    private static int _totalInstances;
    private int _instance;
    private ZMQ.Socket socket;

    public MessageProducer(ZMQ.Socket socket)
    {
        this.socket = socket;
        _instance = _totalInstances;
        _totalInstances += 1;
        System.out.println("Starting producer " + _instance + "...");
    }

    @Override
    public void run()
    {
        int[] messagestoSend = { 1, 2, 3, 4, 5, 4, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        int count = 0;
        while(true){
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            int requestsToSend = messagestoSend[(count++ % messagestoSend.length)];
            for (int i = 0; i < requestsToSend; i++)
            {
                System.out.println("Sending message " + i + " on producer " + _instance + "...");
                synchronized (socket)
                {
                    String data = "Hello message " + i + " from producer " + _instance;
                    socket.send(data);
                    socket.recv();
                }
            }
            System.out.println("Producer done!");
        }
    }
}
