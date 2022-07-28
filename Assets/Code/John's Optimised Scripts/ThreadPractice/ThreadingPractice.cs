using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Jobs;
using UnityEngine;

public struct CamJob : IJob
{
    public void Execute()
    {
        float answer = 0;

        for (int i = 0; i < 10000000; i++)
        {
            answer += Mathf.Sqrt(i) + Mathf.PerlinNoise(i * 1.24f, 0);
        }

        Debug.Log("I did something! : " + answer);
    }
}

public class ThreadingPractice : MonoBehaviour
{
    List<Thread> threads = new List<Thread>();
    public int delayTime = 100;
    // Start is called before the first frame update
    void Start()
    {
        //-----------------------------------------------------

        // Order of Thread Completion Test
        /*Thread newThread = new Thread(LongRunningFunction);
        threads.Add(newThread);
        Thread newThread2 = new Thread(LongRunningFunction2);
        threads.Add(newThread2);
        Thread newThread3 = new Thread(LongRunningFunction3);
        threads.Add(newThread3);
        Thread newThread4 = new Thread(LongRunningFunction4);
        threads.Add(newThread4);

        foreach(Thread thread in threads)
        {
            thread.Start();
        }*/

        //------------------------------------------------------


        for (int i = 0; i < 100; i++)
        {
            /*Thread newThread = new Thread(LongRunningFunction);
            newThread.Start();*/

            //Instantiate/New the Job
            CamJob camJob = new CamJob();

            //Schedule the job
            JobHandle handle = camJob.Schedule();
        }
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 100; i++)
            {
                Thread newThread = new Thread(LongRunningFunction);
                newThread.Start();
            }
        }
    }*/

    private void LongRunningFunction(object obj)
    {
        Thread.Sleep(delayTime * 10);

        Debug.Log("Hello from thread 1");
    }

    private void LongRunningFunction4(object obj)
    {
        Thread.Sleep(delayTime * 10);

        Debug.Log("Hello from thread 4");
    }

    private void LongRunningFunction3(object obj)
    {
        Thread.Sleep(delayTime * 10);

        Debug.Log("Hello from thread 3");
    }

    private void LongRunningFunction2(object obj)
    {
        Thread.Sleep(delayTime * 10);

        Debug.Log("Hello from thread 2");
    }
}
