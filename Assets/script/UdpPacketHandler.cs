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
    }

    public int Handle(byte[] _recvBuffer, int _recvSize) // recvBuffer를 가져와서 써도 된다.
    {
        if (_recvSize > 3)
        {
            //readPos = _udp.GetRingBuffer().GetReadPos();
            //bufferSize = _udp.GetRingBuffer().GetSize();

            //if (readPos > bufferSize)
            //{
            //    Array.Copy(_udp.GetBuffer(), readPos, tempBuffer, 0, bufferSize - readPos);
            //    Array.Copy(_udp.GetBuffer(), 0, tempBuffer, bufferSize - readPos, _udp.GetRingBuffer().GetRemainSize() - (bufferSize - readPos));
            //}
            //else
            //{
            //    Array.Copy(_udp.GetBuffer(), readPos, tempBuffer, 0, _udp.GetRemianSize());
            //}

            readStream = new MemoryStream(_recvBuffer);
            binaryReader = new BinaryReader(readStream);

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            switch (type)
            {
                case 1:

                    break;
                case 2:
                    PeerConnect();
                    break;
                case 3:
                    PeerPosition();
                    break;
                default:
                    break;
            }

            return size;
        }
        return 0;
    }

    void PeerConnect()
    {
        App app = Transform.FindObjectOfType<App>();
        LoadingSceneController loading = Transform.FindObjectOfType<LoadingSceneController>();
        CGameManager gm = CGameManager.Instance;

        if (gm.gameSocket == 2) return;

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
            }
        }

        int connectCount = 0;
        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);

            if (player.GetUdpconnect()) connectCount++;
        }

        if (connectCount == count) gm.gameSocket = 2;
    }

    void PeerPosition()
    {
        CGameManager gm = CGameManager.Instance;
        int count = gm.GetPlayerCount();
        CPlayer player;

        uint socket = binaryReader.ReadUInt32();

        for (int i = 0; i < count; i++)
        {
            player = gm.GetPlayer(i);

            if(player.GetSocket() == socket)
            {
                Vector3 position;
                Quaternion rotation = Quaternion.identity;

                position.x = binaryReader.ReadSingle();
                position.y = binaryReader.ReadSingle();
                position.z = binaryReader.ReadSingle();

                rotation.x = binaryReader.ReadSingle();
                rotation.y = binaryReader.ReadSingle();
                rotation.z = binaryReader.ReadSingle();
                rotation.w = binaryReader.ReadSingle();

                int action = binaryReader.ReadInt32();

                player.SetPosition(position, rotation);
                player.SetAction(action);
            }
        }
    }
}