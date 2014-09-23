package Utilities;

import org.zeromq.ZContext;
import org.zeromq.ZMQ;

public class SocketUtil {
    public SocketUtil() {
    }

    public ZMQ.Socket setupSocket(int socketType, String urlIncludingPort) {
        ZContext context = new ZContext();
        ZMQ.Socket socket = context.createSocket(socketType);
        socket.connect(urlIncludingPort);
        return socket;
    }

    public void sendMessagesForIteration(int count, int instance, ZMQ.Socket socket, int[] messagesWave) {
        int requestsToSend = getNumberOfMessagesToSend(count, messagesWave);
        for (int i = 0; i < requestsToSend; i++) {
            System.out.println("Sending message " + i + " on producer " + instance + "...");
            synchronized (socket) {
                String data = "Hello message " + i + " from producer " + instance;
                sendMessage(socket, data);
            }
        }
        System.out.println("Producer finished sending messages for iteration " + count);
    }

    public int getNumberOfMessagesToSend(int currentCount, int[] messagesWave) {
        return messagesWave[(currentCount % messagesWave.length)];
    }

    public void sendMessage(ZMQ.Socket socket, String message) {
        socket.send(message);
        socket.recv();
    }
}