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
    EndPoint end;

    float time = 0;

    MemoryStream writeStream;
    MemoryStream readStream;
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;

    public bool OnGame = false;

    public void Init(String _ip, int _port)
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

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
        end = (EndPoint)endPoint;
        while (true)
        {
            int recvSize = 0;

            recvSize = m_socket.ReceiveFrom(buffer, 0, ref end); // 여러명일 경우

            readStream = new MemoryStream(buffer);
            binaryReader = new BinaryReader(readStream);

            UdpPackHandler();

            OnGame = true;

            if (recvSize < 0)
            {
                //m_socket.Close();
                continue;
            }

            // 처리를 하고 서버로 sendto 서버에서는 다른 peer로 전달만
        }
    }

    public void RunLoop()
    {
        if(!OnGame)
        {
            time += Time.deltaTime;
            if (time > 1f)
            {
                byte[] buf = new byte[100];
                writeStream = new MemoryStream(buf);
                binaryWriter = new BinaryWriter(writeStream);

                App app = Transform.FindObjectOfType<App>();
                EndPoint end = (EndPoint)iPEndPoint;

                binaryWriter.Write((ushort)4);
                binaryWriter.Write((ushort)app.GetSocket());
                m_socket.SendTo(buf, 4, 0, end);
            }
        }
        else
        {
            byte[] buf = new byte[1000];
            writeStream = new MemoryStream(buf);
            binaryWriter = new BinaryWriter(writeStream);

            Vector3 vector = CGameManager.Instance.GetPosition();

            binaryWriter.Write((ushort)(sizeof(float) * 3 + 6));
            binaryWriter.Write(((ushort)2));
            binaryWriter.Write((ushort)CGameManager.Instance.number); // 자신의 번호

            binaryWriter.Write((float)vector.x);
            binaryWriter.Write((float)vector.y);
            binaryWriter.Write((float)vector.z);

            m_socket.SendTo(buf, (int)writeStream.Position, 0, end);
        }
    }

    private void UdpPackHandler()
    {
        int size = binaryReader.ReadUInt16();
        int type = binaryReader.ReadUInt16();

        if (type == 1)
        {
            int myNum = CGameManager.Instance.number;

            ushort number = binaryReader.ReadUInt16();
            int address = binaryReader.ReadInt32();
            ushort pont = binaryReader.ReadUInt16();

            if (myNum != number)
            {
                iPEndPoint = new IPEndPoint(address, pont);
                end = (EndPoint)iPEndPoint;
                m_socket.Connect(iPEndPoint);
            }

            ushort number2 = binaryReader.ReadUInt16();
            int address2 = binaryReader.ReadInt32();
            ushort pont2 = binaryReader.ReadUInt16();

            if (myNum != number2)
            {
                iPEndPoint = new IPEndPoint(address2, pont2);
                end = (EndPoint)iPEndPoint;
                m_socket.Connect(iPEndPoint);
            }
        }
        if (type == 2)
        {
            if (CGameManager.Instance.gameStart)
            {
                // ok를 받으면
                Vector3 vector;

                int num = binaryReader.ReadUInt16();

                vector.x = binaryReader.ReadSingle();
                vector.y = binaryReader.ReadSingle();
                vector.z = binaryReader.ReadSingle();

                CGameManager.Instance.test(num, vector);
            }
        }
    }
}
