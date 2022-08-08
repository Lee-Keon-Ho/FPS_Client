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

public class socket : MonoBehaviour // 이걸 쓰레드로 쓰자
{
    byte[] buffer;
    MemoryStream memoryStream;
    BinaryWriter binaryWriter;
    public static Socket m_socket;
    void Awake()
    {
        buffer = new byte[65535];
        memoryStream = new MemoryStream(buffer);
        binaryWriter = new BinaryWriter(memoryStream);

        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("59.30.46.242"), 30002);

            m_socket.Connect(iPEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

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
        binaryWriter.Write((ushort)2);
        binaryWriter.Write(str);

        int size = m_socket.Send(buffer, (int)memoryStream.Position, 0);

        Debug.Log("str size" + str.Length);

        for(int i = 0; i < str.Length; i ++)
        {
            Debug.Log(str[i]);

        }
        memoryStream.Position = 0;

        //recv에서 성공이라고 하면

        if (size > 0) SceneManager.LoadScene("Lobby");
    }

    private void OnDestroy()
    {
        // 종료 및 logout packet
    }
}
