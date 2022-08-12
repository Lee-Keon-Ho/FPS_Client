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

public class CSocket : MonoBehaviour
{
    private static CSocket instance;

    CRingBuffer ringBuffer;

    byte[] sendBuffer;
    Socket m_socket;
    Thread thread;

    public static CSocket Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<CSocket>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<CSocket>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }


    void Awake()
    {
        ringBuffer = new CRingBuffer(65535);
        
        sendBuffer = new byte[65535];

        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("222.113.24.195"), 30002);

            m_socket.Connect(iPEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        thread = new Thread(Run);
        thread.Start();

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ringBuffer.GetRemainSize() > 3)
        {
            byte[] tempBuffer = new byte[65530];
            int readPos = ringBuffer.GetReadPos();
            int bufferSize = ringBuffer.GetSize();
            MemoryStream tempStream = new MemoryStream(tempBuffer);
            BinaryReader binaryReader = new BinaryReader(tempStream);

            Debug.Log("remainSize : " + ringBuffer.GetRemainSize());
            Debug.Log("readPos : " + ringBuffer.GetReadPos());
            // 여기부터 read에 대한처리
            if (readPos > bufferSize)
            {
                Array.Copy(ringBuffer.GetBuffer(), readPos, tempBuffer, 0, bufferSize - readPos);
                Array.Copy(ringBuffer.GetBuffer(), 0, tempBuffer, bufferSize - readPos, ringBuffer.GetRemainSize() - (bufferSize - readPos));
            }
            else
            {
                Array.Copy(ringBuffer.GetBuffer(), tempBuffer, ringBuffer.GetRemainSize());
            }

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();
            Debug.Log("size : " + size);
            Debug.Log("type : " + type);

            if (type == 1) // Handle
            {
                ushort scene = binaryReader.ReadUInt16();
                Debug.Log("scene : " + scene);
                if (scene == 1) SceneManager.LoadScene("Lobby");
            }

            ringBuffer.Read(size);

        }
    }

    public void LoginButton(TextMeshProUGUI _textMesh)
    {
        byte[] str = System.Text.Encoding.Unicode.GetBytes(_textMesh.text);
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        bw.Write((ushort)(sizeof(int) + str.Length - 2));
        bw.Write((ushort)1);
        bw.Write(str);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position - 2, 0);

        memoryStream.Position = 0;
    }

    public void ChatSend(TextMeshProUGUI _textMesh)
    {
        byte[] str = System.Text.Encoding.Unicode.GetBytes(_textMesh.text);
        MemoryStream memoryStream = new MemoryStream(sendBuffer);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        bw.Write((ushort)(sizeof(int) + str.Length - 2));
        bw.Write((ushort)5);
        bw.Write(str);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position - 2, 0);

        memoryStream.Position = 0;
    }

    private void OnDestroy()
    {
        // 종료 및 logout packet
    }

    void Run()
    {   
        while (true)
        {
            int writePos = ringBuffer.GetWritePos();
            int recvSize = 0;

            recvSize = m_socket.Receive(ringBuffer.GetBuffer(), writePos, ringBuffer.GetWriteBufferSize(), SocketFlags.None);

            ringBuffer.Write(recvSize);

            for(int i = 0; i < ringBuffer.GetRemainSize(); i++)
            {
                Debug.Log(ringBuffer.GetBuffer()[i]);
            }

            if (recvSize <= 0) break;
        }
    }

   
}
