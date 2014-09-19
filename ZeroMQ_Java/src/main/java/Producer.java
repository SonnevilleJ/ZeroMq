import org.zeromq.ZContext;
import org.zeromq.ZMQ;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

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

        Thread t1 = new Thread(new MessageProducer(socket));
        Thread t2 = new Thread(new MessageProducer(socket));
        Thread t3 = new Thread(new MessageProducer(socket));

        t1.start();
        t2.start();
        t3.start();

        try {
            t1.join();
            t2.join();
            t3.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

//        socket.send("Done");
//        socket.recv();

        br.readLine();
    }
}
