using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.SceneManagement;
public class CRecv : MonoBehaviour
{
    byte[] buffer;
    MemoryStream memoryStream;
    BinaryReader binaryReader;
    
    Socket socket;

    void Awake()
    {
        buffer = new byte[65535];

        memoryStream = new MemoryStream(buffer);
        binaryReader = new BinaryReader(memoryStream);
    }

    void Start()
    {
        socket = CSocket.Instance.m_socket;
        Debug.Log(socket);
    }

    // Update is called once per frame
    void Update()
    {
        int recvSize = socket.Receive(buffer);

        if (recvSize > 0)
        {
            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            if (type == 1)
            {
                ushort scene = binaryReader.ReadUInt16();
                if (scene == 1) SceneManager.LoadScene(1);
            }
        }
        Debug.Log("ok");
    }
}
