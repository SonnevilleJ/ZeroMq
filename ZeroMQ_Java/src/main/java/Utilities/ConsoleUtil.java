package Utilities;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class ConsoleUtil {
    public ConsoleUtil() {
    }

    public void blockTillUserPressesEnter() throws IOException {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
        System.out.println("Socket connected!");
        System.out.println("Press enter to start...");
        br.readLine();
    }
}