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
    int recvSize;

    Queue que = new Queue(); // 2022-10-28 concurrentqueue // C# que 락

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
            try
            {
                byte[] recvBuffer = new byte[1000];
                recvSize = m_socket.ReceiveFrom(recvBuffer, 0, 1000, 0, ref end); // 여러명일 경우
                que.Enqueue(recvBuffer);
            }
            catch (Exception e)
            {
                Debug.Log(e);
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
        while(que.Count > 0)
        {
            UdpPacketHandler.instance.Handle((byte[])que.Dequeue()); // 당장은 고민할 필요가 없다.
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

        bw.Write((ushort)28); // 2
        bw.Write((ushort)3); // 2
        bw.Write(_socket); // 4
        bw.Write(_positon.x); // 4
        bw.Write(_positon.y); // 4
        bw.Write(_positon.z); // 4
        bw.Write(_Rotation.y); // 4
        bw.Write(_action);

        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
            EndPoint end = (EndPoint)endPoint;

            int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
        }
    }

    public void InputKey(uint _socket, int _action, Vector3 _position, float _rotation)
    {
        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        CPlayer player;

        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)28);
        bw.Write((ushort)4);
        bw.Write(_socket);
        bw.Write(_action);
        bw.Write(_position.x);
        bw.Write(_position.y);
        bw.Write(_position.z);
        bw.Write(_rotation);

        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
            EndPoint end = (EndPoint)endPoint;

            int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
        }
    }

    public void MouseMove(uint _socket, Vector3 _position, float _rotation)
    {
        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        CPlayer player;

        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)24);
        bw.Write((ushort)5);
        bw.Write(_socket);
        bw.Write(_position.x);
        bw.Write(_position.y);
        bw.Write(_position.z);
        bw.Write(_rotation);

        for(int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
            EndPoint end = (EndPoint)endPoint;

            int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
        }
    }

    public void FireBullet(uint _socket, Vector3 _position, Quaternion _rotation)
    {
        App app = Transform.FindObjectOfType<App>();
        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        CPlayer player;

        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)36);
        bw.Write((ushort)6);
        bw.Write(_socket);
        bw.Write(_position.x);
        bw.Write(_position.y);
        bw.Write(_position.z);
        bw.Write(_rotation.x);
        bw.Write(_rotation.y);
        bw.Write(_rotation.z);
        bw.Write(_rotation.w);

        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            if(player.GetSocket() != app.GetSocket())
            {
                IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
                EndPoint end = (EndPoint)endPoint;

                int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
            }
        }
    }

    public void PeerHit(uint _socket, int _hp)
    {
        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        CPlayer player;

        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)8);
        bw.Write((ushort)7);
        bw.Write(_hp);

        for(int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            if (player.GetSocket() == _socket)
            {
                IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
                EndPoint end = (EndPoint)endPoint;

                int size = m_socket.SendTo(sendBuffer, (int)memoryStream.Position, SocketFlags.None, end);
                break;
            }
        }
    }

    public void Status()
    {
        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        CPlayer player;

        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)(4 + (sizeof(ushort) * count)));
        bw.Write((ushort)8);

        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            bw.Write((ushort)player.GetKill());
            bw.Write((ushort)player.GetDeath());
            Debug.Log(i + " : " + player.GetKill() + " : " + player.GetDeath());
        }

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