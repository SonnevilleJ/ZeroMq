import org.zeromq.*;
import java.io.UnsupportedEncodingException;

public class MessageProducer implements Runnable
{
    public static final int requestsToSend = 10;
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
        for (int i = 0; i < requestsToSend; i++)
        {
            System.out.println("Sending message {0}..." + i);
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
