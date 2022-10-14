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

            switch (type)
            {
                case 1:
                    App app = Transform.FindObjectOfType<App>();

                    app.SetBoolUdp(1);

                    break;
                case 2:

                    CGameManager.Instance.gameStartTest = 1;

                    break;
                default:
                    break;
            }

            return size;
        }
        return 0;
    }
}
