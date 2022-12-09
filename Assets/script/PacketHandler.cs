using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PacketHandler : MonoBehaviour
{
    public static PacketHandler instance;
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
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        tempBuffer = new byte[65530];
        memoryStream = new MemoryStream(tempBuffer);
        binaryReader = new BinaryReader(memoryStream);
    }

    public int Handle(CSocket _socket)
    {
        if (_socket.GetRingBuffer().GetRemainSize() > 3)
        {
            readPos = _socket.GetRingBuffer().GetReadPos();
            bufferSize = _socket.GetRingBuffer().GetSize();

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

            memoryStream.Position = 0;

            ushort size = binaryReader.ReadUInt16();
            ushort type = binaryReader.ReadUInt16();

            switch(type)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Logout();
                    break;
                case 3:
                    UserList();
                    break;
                case 4:
                    RoomList();
                    break;
                case 5:
                    Chatting();
                    break;
                case 6:
                    CreateRoom();
                    break;
                case 7:
                    RoomIn();
                    break;
                case 8:
                    RoomOut();
                    break;
                case 9:
                    RoomState();
                    break;
                case 12:
                    GameStart();
                    break;
                case 13:
                    PlayerInfo();
                    break;
                case 14:
                    SockAddr();
                    break;
                case 15:
                    AddressAll();
                    break;
                default:
                    break;
            }

            return size;
        }
        return 0;
    }

    private void Login()
    {
        ushort scene = binaryReader.ReadUInt16();
        ushort socket = binaryReader.ReadUInt16();

        App app = Transform.FindObjectOfType<App>();
        app.SetSocket(socket);

        if (scene == 1) SceneManager.LoadScene("Lobby");
    }

    private void Logout()
    {
        ushort exit = binaryReader.ReadUInt16();
        if (exit == 1)
        {
            //UnityEditor.EditorApplication.isPlaying = false; // 에디터
            Application.Quit(); // 게임
            // 소켓에서 처리하는게 아니라 다른 곳에 보내서 처리하는게 맞다! 서버랑 똑같다
            //_socket.Close();
            
        }
    }

    private void UserList()
    {
        ushort count = binaryReader.ReadUInt16();
        ushort state;
        string str;
        CUserList userList = Transform.FindObjectOfType<CUserList>();

        if (userList == null) return;

        for (int i = 0; i < count; i++)
        {
            state = binaryReader.ReadUInt16();
            str = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(32));
            userList.UserListUpdate(str, state, i);
        }
    }

    private void RoomList()
    {
        ushort count = binaryReader.ReadUInt16();
        int number;
        string roomName;
        int playerCount;
        int state;
        RoomList roomList = Transform.FindObjectOfType<RoomList>();

        if (roomList == null) return;

        for (int i = 0; i < 10; i++)
        {
            number = binaryReader.ReadInt32();
            roomName = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(64));
            playerCount = binaryReader.ReadInt32();
            state = binaryReader.ReadInt32();

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

    private void Chatting()
    {
        LobbyChatting gameObject = Transform.FindObjectOfType<LobbyChatting>();
        gameObject.OnList(System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(100)));
    }

    private void CreateRoom()
    {
        ushort scene = binaryReader.ReadUInt16();
        if (scene == 1)
        {
            App gameObject = Transform.FindObjectOfType<App>();
            gameObject.SetPlayerInfo(0, 0);
            SceneManager.LoadScene("Room");
        }
    }

    private void RoomIn()
    {
        ushort roomIn = binaryReader.ReadUInt16();

        if (roomIn == 1)
        {
            App gameObject = Transform.FindObjectOfType<App>();
            gameObject.SetPlayerInfo(1, 0);
            SceneManager.LoadScene("Room");
        }
    }

    private void RoomOut()
    {
        App gameObject = Transform.FindObjectOfType<App>();
        gameObject.SetPlayerInfo(1, 0);
        SceneManager.LoadScene("Lobby");
    }

    private void PlayerInfo()
    {
        int boss = binaryReader.ReadUInt16();
        int ready = binaryReader.ReadUInt16();
        int number = binaryReader.ReadUInt16();

        App player = Transform.FindObjectOfType<App>();

        player.SetBoss(boss);
        player.SetReady(ready);
        player.SetNumber(number);
    }

    private void RoomState()
    {
        ushort count = binaryReader.ReadUInt16();
        ushort ready;
        string name;
        ushort boss;
        CRoom room = Transform.FindObjectOfType<CRoom>();
        room.playerInfoReset();

        for (int i = 0; i < count; i++)
        {
            name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(32));
            ready = binaryReader.ReadUInt16();
            boss = binaryReader.ReadUInt16();

            room.OnPlayerListInfo(ready, name, boss, i);
        }
    }

    private void GameStart()
    {
        CGameManager gm = CGameManager.Instance;

        int count = binaryReader.ReadInt16();
        gm.SetPlayerCount(count);

        App app = Transform.FindObjectOfType<App>();

        app.UdpInit("183.108.148.83", 30001);
        
        LoadingSceneController.LoadScene("Game"); // 로딩에 들어가서 나의 유디피 포트값을 tcp로 보내주자
    }

    private void SockAddr()
    {
        uint address = binaryReader.ReadUInt32();
        ushort port = binaryReader.ReadUInt16();
        long sourceAddr = binaryReader.ReadInt32();

        App app = Transform.FindObjectOfType<App>();
        app.SetAddr(address, port);
        app.GetPlayer().SetSourceAddr(sourceAddr);
    }

    private void AddressAll()
    {
        App app = Transform.FindObjectOfType<App>();
        CGameManager gm = CGameManager.Instance;
        int count = binaryReader.ReadUInt16();
        uint addr;
        uint socket;
        ushort port;
        long sourceAddr;

        gm.SetPlayerCount(count);

        for (int i = 0; i < count; i++)
        {
            socket = binaryReader.ReadUInt32(); 
            addr = binaryReader.ReadUInt32();
            port = binaryReader.ReadUInt16();
            sourceAddr = binaryReader.ReadInt32();

            string name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(32));

            CGameManager.Instance.SetPlayers(i, socket, addr, port, sourceAddr, name);
        }

        CGameManager.Instance.gameSocket = 1;
    }
}