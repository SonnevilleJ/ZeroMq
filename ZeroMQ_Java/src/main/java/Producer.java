import org.zeromq.ZContext;
import org.zeromq.ZMQ;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class Producer {
    public static final String TCP_LOCALHOST_5559 = "tcp://localhost:5559";

    public void Run() throws IOException {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        ZContext context = new ZContext();
        ZMQ.Socket socket = context.createSocket(ZMQ.REQ);

        socket.connect(TCP_LOCALHOST_5559);
        System.out.println("Socket connected!");
        System.out.println("Press enter to start...");
        br.readLine();

        new MessageProducer(socket).run();
        new MessageProducer(socket).run();
        new MessageProducer(socket).run();

        socket.send("Done");
        socket.recv();

        br.readLine();
    }
}
