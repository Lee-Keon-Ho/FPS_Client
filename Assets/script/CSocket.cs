using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TMPro;
using System.Threading;

public class CSocket
{
    CRingBuffer ringBuffer;

    byte[] sendBuffer;
    Socket m_socket;
    Thread thread;

    public void Init(String _ip, int _port)
    {
        ringBuffer = new CRingBuffer(65535);

        sendBuffer = new byte[65535];

        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

    public void Delete()
    {
        m_socket.Close();
    }

    public void RunLoop()
    {
        if (ringBuffer.GetRemainSize() > 3)
        {
            ringBuffer.Read(PacketHandler.instance.Handle(this));
        }
    }

    public void Login(TextMeshProUGUI _textMesh)
    {
        byte[] str = System.Text.Encoding.Unicode.GetBytes(_textMesh.text);
        MemoryStream memoryStream = new MemoryStream(sendBuffer); // 포인터 처럼 
        BinaryWriter bw = new BinaryWriter(memoryStream); // 가지고 있어도 된다.

        memoryStream.Position = 0;

        bw.Write((ushort)(sizeof(int) + str.Length - 2));
        bw.Write((ushort)1);
        bw.Write(str);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position - 2, 0);
    }

    public void LogOut()
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)(sizeof(int)));
        bw.Write((ushort)2); // logout

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position, 0);
    }

    public void UserList()
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)(sizeof(int)));
        bw.Write((ushort)3);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position, 0);
    }

    public void RoomList()
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)(sizeof(int)));
        bw.Write((ushort)4);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ChatSend(TextMeshProUGUI _textMesh)
    {
        byte[] str = System.Text.Encoding.Unicode.GetBytes(_textMesh.text); // 11 32가 나오는건 인코딩 해서 나오는 것이다.
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)(sizeof(int) + str.Length - 2));
        bw.Write((ushort)5);
        bw.Write(str);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position - 2, 0);
    }

    public void CreateRoom(TextMeshProUGUI _textMesh)
    {
        byte[] str = System.Text.Encoding.Unicode.GetBytes(_textMesh.text); // 11 32가 나오는건 인코딩 해서 나오는 것이다.
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)(sizeof(int) + str.Length - 2));
        bw.Write((ushort)6);
        bw.Write(str);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position - 2, 0);
    }

    public void Selected(int _num)
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)6);
        bw.Write((ushort)7);
        bw.Write((ushort)_num);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position, 0);
    }

    public void RoomOut()
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)sizeof(int));
        bw.Write((ushort)8);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position, 0);
    }
    public void TeamChange(int _num)
    {
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        memoryStream.Position = 0;

        bw.Write((ushort)6);
        bw.Write((ushort)10);
        bw.Write((ushort)_num);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position, 0);
    }

    void Run()
    {   
        while (true)
        {
            int writePos = ringBuffer.GetWritePos();
            int recvSize = 0;

            recvSize = m_socket.Receive(ringBuffer.GetBuffer(), writePos, ringBuffer.GetWriteBufferSize(), SocketFlags.None);

            ringBuffer.Write(recvSize);

            if (recvSize <= 0)
            {
                m_socket.Close();
                break;
            }
        }
    }

   

    public CRingBuffer GetRingBuffer() { return ringBuffer; }
}