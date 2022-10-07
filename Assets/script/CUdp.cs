using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

public class CUdp
{
    CRingBuffer ringBuffer = new CRingBuffer(65535);
    byte[] sendBuffer = new byte[65535];

    Socket m_socket;
    Thread thread;

    IPEndPoint iPEndPoint;
    EndPoint m_end;

    MemoryStream writeStream;
    MemoryStream readStream;
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;

    public bool OnGame = true;
    public bool GoGame = false;

    public void Init(String _ip, int _port)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        try
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
            m_end = (EndPoint)iPEndPoint;

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
        while (true)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            EndPoint end = (EndPoint)endPoint;

            int writePos = ringBuffer.GetWritePos();
            int recvSize = 0;

            recvSize = m_socket.ReceiveFrom(ringBuffer.GetBuffer(), writePos, ringBuffer.GetWriteBufferSize(),0 , ref end); // 여러명일 경우

            ringBuffer.Write(recvSize);

            if (recvSize < 0)
            {
                //m_socket.Close();
                continue;
            }
        }
    }

    public void RunLoop()
    {
        if (ringBuffer.GetRemainSize() > 3)
        {
            ringBuffer.Read(UdpPacketHandler.instance.Handle(this));
        }
    }

    public void SendSocket(uint _socket)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
        EndPoint end = (EndPoint)endPoint;

        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)4);
        bw.Write((ushort)1);
        bw.Write((ushort)_socket);

        int size = m_socket.SendTo(sendBuffer, 6, SocketFlags.None, m_end);
    }

    public bool GetOnGame() { return GoGame; }
    public void SetOnGame(bool _onGame) { GoGame = _onGame; }

    public CRingBuffer GetRingBuffer() { return ringBuffer; }
}
