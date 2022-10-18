using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class UdpPacketHandler : MonoBehaviour
{
    public static UdpPacketHandler instance;
    byte[] tempBuffer;
    MemoryStream memoryStream;
    BinaryReader binaryReader;
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
        memoryStream = new MemoryStream(tempBuffer);
        binaryReader = new BinaryReader(memoryStream);
    }

    public int Handle(CUdp _udp)
    {
        if (_udp.GetRemianSize() > 3)
        {
            readPos = _udp.GetRingBuffer().GetReadPos();
            bufferSize = _udp.GetRingBuffer().GetSize();

            // ������� read�� ����ó��
            if (readPos > bufferSize)
            {
                Array.Copy(_udp.GetBuffer(), readPos, tempBuffer, 0, bufferSize - readPos);
                Array.Copy(_udp.GetBuffer(), 0, tempBuffer, bufferSize - readPos, _udp.GetRingBuffer().GetRemainSize() - (bufferSize - readPos));
            }
            else
            {
                Array.Copy(_udp.GetBuffer(), 0, tempBuffer, 0, _udp.GetRemianSize());
            }

            memoryStream.Position = 0;

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            App app = Transform.FindObjectOfType<App>();

            switch (type)
            {
                case 1:
                    app.SetBoolUdp(1);

                    break;
                case 2:
                    CGameManager gm = CGameManager.Instance;

                    uint socket = binaryReader.ReadUInt32();

                    int count = gm.GetPlayerCount();

                    CPlayer player;

                    for(int i = 0; i < count; i++)
                    {
                        player = gm.GetPlayer(i);
                        if(player.GetSocket() == socket)
                        {
                            player.SetUdpConnect(true);
                        }
                    }

                    // ��� ������ udp ����� �Ϸ�Ǿ��ٰ� �ϸ� peer1���� �Ϸ�Ǿ��ٰ� ������
                    break;
                default:
                    break;
            }

            return size;
        }
        return 0;
    }
}
