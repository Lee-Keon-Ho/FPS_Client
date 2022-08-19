using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PacketHandler : MonoBehaviour
{
    public static PacketHandler instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public int Handle(CSocket _socket)
    {
        if (_socket.GetRingBuffer().GetRemainSize() > 3)
        {
            byte[] tempBuffer = new byte[65530];
            int readPos = _socket.GetRingBuffer().GetReadPos();
            int bufferSize = _socket.GetRingBuffer().GetSize();
            MemoryStream tempStream = new MemoryStream(tempBuffer);
            BinaryReader binaryReader = new BinaryReader(tempStream);

            // 여기부터 read에 대한처리
            if (readPos > bufferSize)
            {
                Array.Copy(_socket.GetRingBuffer().GetBuffer(), readPos, tempBuffer, 0, bufferSize - readPos);
                Array.Copy(_socket.GetRingBuffer().GetBuffer(), 0, tempBuffer, bufferSize - readPos, _socket.GetRingBuffer().GetRemainSize() - (bufferSize - readPos));
            }
            else
            {
                Array.Copy(_socket.GetRingBuffer().GetBuffer(), readPos, tempBuffer, 0, _socket.GetRingBuffer().GetRemainSize());
            }

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            // Handle    
            if (type == 1) // login
            {
                ushort scene = binaryReader.ReadUInt16();
                if (scene == 1) SceneManager.LoadScene("Lobby");
            }
            if (type == 2)
            {
                ushort exit = binaryReader.ReadUInt16();
                if (exit == 1)
                {
                    //UnityEditor.EditorApplication.isPlaying = false; // 에디터
                    //Application.Quit(); // 게임
                    // 소켓에서 처리하는게 아니라 다른 곳에 보내서 처리하는게 맞다! 서버랑 똑같다
                    //_socket.Close();
                }
            }
            if (type == 3) // userList
            {
                ushort count = binaryReader.ReadUInt16();
                ushort state;
                string str;
                CUserList userList = Transform.FindObjectOfType<CUserList>();
                for (int i = 0; i < count; i++)
                {
                    state = binaryReader.ReadUInt16();
                    str = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(64));
                    userList.UserListUpdate(str, state, i);
                }
            }
            if (type == 4) // roomList
            {
                ushort count = binaryReader.ReadUInt16();
                int number = binaryReader.ReadInt32();
                string roomName = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(64));
                int playerCount = binaryReader.ReadInt32();
                int state = binaryReader.ReadInt32();
                RoomList roomList = Transform.FindObjectOfType<RoomList>();
                for (int i = 0; i < 10; i++)
                {
                    if (i < count)
                    {
                        roomList.RoomListUpdate(number, roomName, playerCount, state, i);
                    }
                    else
                    {
                        roomList.RoomListUpdate(0, "", 0, 0, i);
                    }
                }
            }
            if (type == 5) // chatting
            {
                LobbyChatting gameObject = Transform.FindObjectOfType<LobbyChatting>();
                gameObject.OnList(System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(100)));
            }
            if (type == 6) // CreateRoom
            {
                ushort scene = binaryReader.ReadUInt16();
                if (scene == 1) SceneManager.LoadScene("Room");
            }

            return size;
        }
        return 0;
    }
}
