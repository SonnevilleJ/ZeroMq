import java.io.IOException;

public class App {

    public static void main(String[] args){
        try {
            new Producer().Run();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
