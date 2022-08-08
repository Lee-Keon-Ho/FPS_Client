using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
public class thread : MonoBehaviour
{
    Thread m_thread;
    Socket socket;
    // Start is called before the first frame update
    private void Awake()
    {
        m_thread = new Thread(() => Run(socket));
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Run(Socket _socket)
    {

    }
}
