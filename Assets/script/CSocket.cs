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
    byte[] recvBuffer;
    MemoryStream sendMemoryStream;
    MemoryStream recvMemoryStream;
    //BinaryWriter binaryWriter;
    //BinaryReader binaryReader;
    public Socket m_socket;
    Thread thread;

    // 2022-08-10
    byte[] buffer;
    int m_size = 65530;
    MemoryStream readBuffer;
    MemoryStream writeBuffer;
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;
    public static CSocket Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<CSocket>();
                if(obj != null) 
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
        sendMemoryStream = new MemoryStream(sendBuffer);
        binaryWriter = new BinaryWriter(sendMemoryStream);
        
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.123.4"), 30002);

            m_socket.Connect(iPEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        //thread = new Thread(Run);
        //thread.Start();


        //2022-08-10
        buffer = new byte[m_size];

        //readBuffer = new MemoryStream(buffer);
        //writeBuffer = new MemoryStream(buffer);
        //binaryReader = new BinaryReader(readBuffer);
        //binaryWriter = new BinaryWriter(writeBuffer);

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(readBuffer.Position == writeBuffer.Position)
        //{

        //}
    }

    public void LoginButton(TextMeshProUGUI _textMesh)
    {
        byte[] str = System.Text.Encoding.Unicode.GetBytes(_textMesh.text);
        MemoryStream memoryStream = new MemoryStream(str);
        BinaryWriter bw = new BinaryWriter(memoryStream);

        bw.Write((ushort)(sizeof(int) + str.Length - 2));
        bw.Write((ushort)1);
        bw.Write(str);

        Debug.Log("str size : " + memoryStream.Position);

        int size = m_socket.Send(sendBuffer, (int)memoryStream.Position, 0);
        
        Debug.Log("str size : " + str.Length);
        Debug.Log(_textMesh.text);
        for(int i = 0; i < _textMesh.text.Length; i ++)
        {
            Debug.Log(_textMesh.text[i]);

        }
        memoryStream.Position = 0;

        //recv에서 성공이라고 하면

       //if (size > 0) SceneManager.LoadScene("Lobby");
    }

    private void OnDestroy()
    {
        // 종료 및 logout packet
    }

    void Run()
    {
        //recvBuffer = new byte[65535];
        //recvMemoryStream = new MemoryStream(recvBuffer);
        //binaryReader = new BinaryReader(recvMemoryStream);
        
        while (true)
        {
            int writePos = ringBuffer.GetWritePos();
            int readPos = ringBuffer.GetReadPos();
            int bufferSize = ringBuffer.GetSize();
            int recvSize;


            if(readPos <= writePos)
            {
                recvSize = m_socket.Receive(ringBuffer.GetBuffer(), writePos, bufferSize - writePos, SocketFlags.None);
            }
            else
            {
                recvSize = m_socket.Receive(ringBuffer.GetBuffer(), readPos, readPos - writePos, SocketFlags.None);
            }
            
            
            if (recvSize <= 0) break;

            //ushort size = binaryReader.ReadUInt16();
            //ushort type = binaryReader.ReadUInt16();

            writeBuffer.Position += recvSize;
            if(writeBuffer.Position >= m_size)
            {
                writeBuffer.Position = 0;
            }
            

            //if(type == 1)
            //{
            //    CSceneManager.Instance.ChangeScene(1);
            //}
            //recvMemoryStream.Position = 0;
        }
    }

   
}
