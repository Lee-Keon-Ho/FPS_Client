using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;

public class UdpPacketHandler : MonoBehaviour
{
    public static UdpPacketHandler instance;
    byte[] tempBuffer;
    MemoryStream readStream;
    MemoryStream writeStream;
    BinaryReader binaryReader;
    BinaryWriter binaryWriter;
    int readPos;
    int bufferSize;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        tempBuffer = new byte[65530];
        readStream = new MemoryStream(tempBuffer);
        binaryReader = new BinaryReader(readStream);
    }

    public int Handle(CUdp _udp)
    {
        if (_udp.GetRemianSize() > 3)
        {
            readPos = _udp.GetRingBuffer().GetReadPos();
            bufferSize = _udp.GetRingBuffer().GetSize();

            // 여기부터 read에 대한처리
            if (readPos > bufferSize)
            {
                Array.Copy(_udp.GetBuffer(), readPos, tempBuffer, 0, bufferSize - readPos);
                Array.Copy(_udp.GetBuffer(), 0, tempBuffer, bufferSize - readPos, _udp.GetRingBuffer().GetRemainSize() - (bufferSize - readPos));
            }
            else
            {
                Array.Copy(_udp.GetBuffer(), 0, tempBuffer, 0, _udp.GetRemianSize());
            }

            readStream.Position = 0;

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            App app = Transform.FindObjectOfType<App>();

            switch (type)
            {
                case 1:

                    break;
                case 2:
                    Udp2();
                    break;
                default:
                    break;
            }

            return size;
        }
        return 0;
    }

    void Udp2()
    {
        App app = Transform.FindObjectOfType<App>();
        LoadingSceneController loading = Transform.FindObjectOfType<LoadingSceneController>();
        CGameManager gm = CGameManager.Instance;

        uint socket = binaryReader.ReadUInt32();

        int count = gm.GetPlayerCount();

        CPlayer player;

        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);
            if (player.GetSocket() == socket)
            {
                player.SetUdpConnect(true);
                loading.SetOk(i);

                IPEndPoint endPoint = new IPEndPoint(player.GetAddr(), player.GetPort());
                EndPoint end = (EndPoint)endPoint;

                byte[] buffer = new byte[100];
                writeStream = new MemoryStream(buffer);
                binaryWriter = new BinaryWriter(writeStream);

                binaryWriter.Write((ushort)8);
                binaryWriter.Write((ushort)2);
                binaryWriter.Write(player.GetSocket());

                int sendSize = app.GetUdp().GetSocket().SendTo(buffer, (int)writeStream.Position, System.Net.Sockets.SocketFlags.None, end);
                Debug.Log(sendSize);
            }
        }

        int connectCount = 0;
        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);

            if (player.GetUdpconnect()) connectCount++;
        }

        //if (connectCount == count) gm.gameSocket = 2;
    }
}
