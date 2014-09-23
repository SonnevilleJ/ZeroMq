package Utilities;

import java.util.ArrayList;
import java.util.List;

public class ThreadUtil {
    public ThreadUtil() {
    }

    public void mergeThreadsBackToMainThread(List<Thread> threads) {
        try {
            for (Thread thread : threads) {
                thread.join();
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }

    public void startThreads(List<Thread> threads) {
        for (Thread thread : threads) {
            thread.start();
        }
    }

    public <T extends Runnable> List<Thread> createThreadsList(int numberOfThreadsToCreate, T target) {
        List<Thread> threads = new ArrayList<Thread>();
        for (int i = 0; i < numberOfThreadsToCreate; i++) {
            threads.add(new Thread(target));
        }

        return threads;
    }

    public void threadSleep(int millis) {
        try {
            Thread.sleep(millis);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }
}