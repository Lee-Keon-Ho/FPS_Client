using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class CUdp
{
    byte[] buffer = new byte[65535];

    Socket m_socket;
    Thread thread;
    
    public void Init(String _ip, int _port)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);

            m_socket.Connect(iPEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        thread = new Thread(Run);
        thread.Start();
    }

    void Run()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
        EndPoint end = (EndPoint)endPoint;
        while (true)
        {
            int recvSize = 0;

            recvSize = m_socket.ReceiveFrom(buffer, ref end);

            if (recvSize <= 0)
            {
                m_socket.Close();
                continue;
            }

            Debug.Log("쿠와앜");
            // 처리를 하고 서버로 sendto 서버에서는 다른 peer로 전달만
        }
    }

    public void RunLoop()
    {

    }
}
