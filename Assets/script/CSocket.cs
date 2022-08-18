using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
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

    public void RunLoop()
    {
        if (ringBuffer.GetRemainSize() > 3)
        {
            byte[] tempBuffer = new byte[65530];
            int readPos = ringBuffer.GetReadPos();
            int bufferSize = ringBuffer.GetSize();
            MemoryStream tempStream = new MemoryStream(tempBuffer);
            BinaryReader binaryReader = new BinaryReader(tempStream);

            // 여기부터 read에 대한처리
            if (readPos > bufferSize)
            {
                Array.Copy(ringBuffer.GetBuffer(), readPos, tempBuffer, 0, bufferSize - readPos);
                Array.Copy(ringBuffer.GetBuffer(), 0, tempBuffer, bufferSize - readPos, ringBuffer.GetRemainSize() - (bufferSize - readPos));
            }
            else
            {
                Array.Copy(ringBuffer.GetBuffer(), readPos, tempBuffer, 0, ringBuffer.GetRemainSize());
            }

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            // Handle    
            if (type == 1) // login
            {
                ushort scene = binaryReader.ReadUInt16();
                if (scene == 1) SceneManager.LoadScene("Lobby");
            }
            if (type == 2)
            {
                ushort exit = binaryReader.ReadUInt16();
                if(exit == 1)
                {
                    //UnityEditor.EditorApplication.isPlaying = false; // 에디터
                    //Application.Quit(); // 게임
                    // 소켓에서 처리하는게 아니라 다른 곳에 보내서 처리하는게 맞다! 서버랑 똑같다
                    m_socket.Close();
                }
            }

            if (type == 3) // userList
            {
                ushort count = binaryReader.ReadUInt16();
                ushort state;
                String str;
                CUserList userList = Transform.FindObjectOfType<CUserList>();
                for (int i = 0; i < count; i++)
                {
                    state = binaryReader.ReadUInt16();
                    str = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(64));
                    userList.UserListUpdate(str, state, i);
                }
            }
            if (type == 4) // roomList
            {
                
            }
            if (type == 5) // chatting
            {
                ChattingList gameObject = Transform.FindObjectOfType<ChattingList>();
                gameObject.OnList(System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(100)));
            }
            if (type == 6) // CreateRoom
            {
                ushort scene = binaryReader.ReadUInt16();
                if (scene == 1) SceneManager.LoadScene("Room");
            }

            ringBuffer.Read(size);
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
}