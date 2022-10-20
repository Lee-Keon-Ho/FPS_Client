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
    byte[] recvBuffer = new byte[65535];
        
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

    int remainSize = 0;
    int writePos = 0;

    public void Init(String _ip, int _port)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        try
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
            m_end = (EndPoint)iPEndPoint;

            m_socket.SendTo(sendBuffer, 0, SocketFlags.None, m_end);
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
            //int writePos = ringBuffer.GetWritePos();
            int recvSize = 0;

            //recvSize = m_socket.ReceiveFrom(recvBuffer, writePos, ringBuffer.GetWriteBufferSize(),0 , ref end); // 여러명일 경우
            try
            {
                recvSize = m_socket.ReceiveFrom(recvBuffer, ref end);
                remainSize += recvSize;
                Debug.Log(end.ToString());
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            
            //ringBuffer.Write(recvSize);

            if (recvSize < 0)
            {
                //m_socket.Close();
                continue;
            }
        }
    }

    public void RunLoop()
    {
        if (remainSize > 3)
        {
            remainSize -= UdpPacketHandler.instance.Handle(this);
        }
    }

    public void SendSocket(uint _socket)
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)4);
        bw.Write((ushort)1);
        bw.Write((ushort)_socket);

        int size = m_socket.SendTo(sendBuffer, 6, SocketFlags.None, m_end);
    }

    public void PeerConnect()
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        App app = Transform.FindObjectOfType<App>();

        for(int i = 0; i < count; i++)
        {
            CPlayer player = gm.GetPlayer(i);

            if (player.GetUdpconnect()) continue;

            if (app.GetPlayer().GetSocket() != player.GetSocket())
            {
                IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
                EndPoint end = (EndPoint)endPoint;

                bw.Write((ushort)8);
                bw.Write((ushort)2);
                bw.Write(app.GetPlayer().GetSocket());

                int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
                Debug.Log(size);
            }
            else
            {
                player.SetUdpConnect(true);
            }
        }
    }

    public bool GetOnGame() { return GoGame; }
    public void SetOnGame(bool _onGame) { GoGame = _onGame; }
    public CRingBuffer GetRingBuffer() { return ringBuffer; }
    public int GetRemianSize() { return remainSize; }
    public byte[] GetBuffer() { return recvBuffer; }

    public Socket GetSocket() { return m_socket; }
}