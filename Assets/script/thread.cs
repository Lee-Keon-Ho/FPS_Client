using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
public class thread : MonoBehaviour
{
    private Thread m_thread;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        m_thread = new Thread(Run);
        m_thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Run()
    {
        Socket socket = CSocket.Instance.m_socket;
        float time = 0.0f;
        while(true)
        {
            if (time >= 1.0) time = 0.0f;
            time += Time.deltaTime;
        }
    }
}
