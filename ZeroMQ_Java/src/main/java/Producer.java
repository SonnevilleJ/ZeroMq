import Utilities.ConsoleUtil;
import Utilities.SocketUtil;
import Utilities.ThreadUtil;
import org.zeromq.ZMQ;

import java.io.IOException;
import java.util.List;

public class Producer {
    public static final String TCP_LOCALHOST_5559 = "tcp://localhost:5559";
    private static final int NUMBER_OF_THREADS_TO_CREATE = 3;
    private final ThreadUtil threadUtil = new ThreadUtil();
    private final ConsoleUtil consoleUtil = new ConsoleUtil();
    private final SocketUtil socketUtil = new SocketUtil();

    public void Run() throws IOException {
        ZMQ.Socket socket = socketUtil.setupSocket(ZMQ.REQ, TCP_LOCALHOST_5559);

        consoleUtil.blockTillUserPressesEnter();

        List<Thread> threads = threadUtil.createThreadsList(NUMBER_OF_THREADS_TO_CREATE, new MessageProducer(socket));
        threadUtil.startThreads(threads);
        threadUtil.mergeThreadsBackToMainThread(threads);

//        socketUtil.sendMessage(socket, "Done");
    }
}
