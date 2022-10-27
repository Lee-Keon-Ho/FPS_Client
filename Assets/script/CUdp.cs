using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Collections;

public class CUdp
{
    byte[] sendBuffer = new byte[1000];
    byte[] recvBuffer = new byte[1000];
    int recvSize;

    Queue que = new Queue();

    Socket m_socket;
    Thread thread;

    IPEndPoint iPEndPoint;
    EndPoint m_end;

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
            //int recvSize = 0;

            try
            {
                recvSize = m_socket.ReceiveFrom(recvBuffer, 0, 1000, 0, ref end); // 여러명일 경우
                que.Enqueue(recvBuffer);
                Debug.Log(que.Count);
                //ringBuffer.Write(recvSize); 

                // 받아서 바로 처리
            }
            catch (Exception e)
            {
                Debug.Log(e);
                //ringBuffer.Write(recvSize); // 끊겼을때 멈춰라
                // 버퍼가 최과 되면 0으로 바꾸는 부분을 새로 만들어서 사용
            }

            if (recvSize < 0)
            {
                //m_socket.Close();
                continue;
            }
        }
    }

    public void RunLoop()
    {
        // 패킷이 있다.
        UdpPacketHandler.instance.Handle(recvBuffer, recvSize);
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

            if (app.GetPlayer().GetSocket() != player.GetSocket())
            {
                IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
                EndPoint end = (EndPoint)endPoint;

                bw.Write((ushort)8);
                bw.Write((ushort)2);
                bw.Write(app.GetPlayer().GetSocket());

                Debug.Log("SendTo : " + end);
                int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
            }
            else
            {
                player.SetUdpConnect(true);
            }
        }
    }

    public void PeerPosition(Vector3 _positon, Quaternion _Rotation, uint _socket, int _action)
    {
        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        CPlayer player;

        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        bw.Write((ushort)40); // 2
        bw.Write((ushort)3); // 2
        bw.Write(_socket); // 4
        bw.Write(_positon.x); // 4
        bw.Write(_positon.y); // 4
        bw.Write(_positon.z); // 4
        bw.Write(_Rotation.x); // 4
        bw.Write(_Rotation.y); // 4
        bw.Write(_Rotation.z); // 4
        bw.Write(_Rotation.w);
        bw.Write(_action);

        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
            EndPoint end = (EndPoint)endPoint;

            int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
        }
    }
    public Socket GetSocket() { return m_socket; }
}