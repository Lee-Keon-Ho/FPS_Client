using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

public class CUdp
{
    byte[] buffer = new byte[65535];

    Socket m_socket;
    Thread thread;

    IPEndPoint iPEndPoint;

    float time = 0;

    MemoryStream writeStream;
    MemoryStream readStream;
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;

    public bool OnGame = false;

    public void Init(String _ip, int _port)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        writeStream = new MemoryStream(buffer);
        readStream = new MemoryStream(buffer);

        binaryWriter = new BinaryWriter(writeStream);
        binaryReader = new BinaryReader(readStream);

        try
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);

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

            OnGame = true;

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
        time += Time.deltaTime;
        if(time > 1f)
        {
            byte[] buf = new byte[1000];
            writeStream = new MemoryStream(buffer);
            binaryWriter = new BinaryWriter(writeStream);
            
            App app = Transform.FindObjectOfType<App>();            
            EndPoint end = (EndPoint)iPEndPoint;
            
            binaryWriter.Write((ushort)4);
            binaryWriter.Write((ushort)app.m_player.GetNumber());
            m_socket.SendTo(buf, 4, 0, end);
        }
    }
}
