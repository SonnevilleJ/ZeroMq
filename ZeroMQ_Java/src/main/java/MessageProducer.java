import Utilities.SocketUtil;
import Utilities.ThreadUtil;
import org.zeromq.ZMQ;

public class MessageProducer implements Runnable {
    private static final int[] MESSAGES_WAVE = {1, 2, 3, 4, 5, 4, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    private static int _totalInstances;
    private int _instance;
    private ZMQ.Socket socket;
    private final SocketUtil socketUtil = new SocketUtil();
    private final ThreadUtil threadUtil = new ThreadUtil();

    public MessageProducer(ZMQ.Socket socket) {
        this.socket = socket;
        _instance = _totalInstances;
        _totalInstances += 1;
        System.out.println("Starting producer " + _instance + "...");
    }

    @Override
    public void run() {
        int count = 0;
        while (true) {
            threadUtil.threadSleep(1000);
            socketUtil.sendMessagesForIteration(count, _instance, socket, MESSAGES_WAVE);
            count++;
        }
    }
}
