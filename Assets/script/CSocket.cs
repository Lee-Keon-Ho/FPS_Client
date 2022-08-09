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

    byte[] sendBuffer;
    byte[] recvBuffer;
    MemoryStream sendMemoryStream;
    MemoryStream recvMemoryStream;
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;
    public Socket m_socket;
    Thread thread;

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
        sendBuffer = new byte[65535];
        sendMemoryStream = new MemoryStream(sendBuffer);
        binaryWriter = new BinaryWriter(sendMemoryStream);
        
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.123.14"), 30002);

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
        
    }

    public void LoginButton(TextMeshProUGUI _textMesh)
    {
        byte[] str = System.Text.Encoding.Unicode.GetBytes(_textMesh.text);
        binaryWriter.Write((ushort)(sizeof(int) + str.Length));
        binaryWriter.Write((ushort)1);
        binaryWriter.Write(str);

        int size = m_socket.Send(sendBuffer, (int)sendMemoryStream.Position, 0);

        //Debug.Log("str size" + str.Length);

        //for(int i = 0; i < str.Length; i ++)
        //{
        //    Debug.Log(str[i]);

        //}
        sendMemoryStream.Position = 0;

        //recv에서 성공이라고 하면

       //if (size > 0) SceneManager.LoadScene("Lobby");
    }

    private void OnDestroy()
    {
        // 종료 및 logout packet
    }

    void Run()
    {
        recvBuffer = new byte[65535];
        recvMemoryStream = new MemoryStream(recvBuffer);
        binaryReader = new BinaryReader(recvMemoryStream);

        while (true)
        {
            int recvSize = m_socket.Receive(recvBuffer);
            Debug.Log("abcd");
            if (recvSize <= 0) break;

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            if(type == 1)
            {
                CSceneManager.Instance.ChangeScene(1);
            }
            recvMemoryStream.Position = 0;
        }
    }

   
}
