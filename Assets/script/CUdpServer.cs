using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class CUdpServer
{
    Socket m_socket;
    IPEndPoint ipEndPoint;

    public void Init(String _ip, int _port)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        try
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);

            m_socket.Connect(ipEndPoint);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        Thread thread = new Thread(Run);
        thread.Start();
    }

    public void Run()
    {
        int recvSize = 0;
        byte[] buffer = new byte[65535];
        EndPoint endPoint = (EndPoint)ipEndPoint;

        while(true)
        {
            recvSize = m_socket.ReceiveFrom(buffer, ref endPoint);

            Debug.Log(recvSize);
            // Ã³¸®
        }
    }
}
