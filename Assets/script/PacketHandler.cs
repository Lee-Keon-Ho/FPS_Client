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
                case 20:
                    Test();
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
            str = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(64));
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
        ushort team;
        ushort ready;
        string name;
        ushort boss;
        CRoom room = Transform.FindObjectOfType<CRoom>();
        room.playerInfoReset();
        int teamA = 0;
        int teamB = 0;

        for (int i = 0; i < count; i++)
        {
            name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(64));
            team = binaryReader.ReadUInt16();
            ready = binaryReader.ReadUInt16();
            boss = binaryReader.ReadUInt16();

            if (team == 0)
            {
                room.OnPlayerListInfo(team, ready, name, boss, teamA);
                teamA++;
            }
            else
            {
                room.OnPlayerListInfo(team, ready, name, boss, teamB);
                teamB++;
            }
        }
    }

    private void GameStart()
    {
        int tempACount = binaryReader.ReadUInt16();
        int tempBCount = binaryReader.ReadUInt16();

        int playerNum = binaryReader.ReadUInt16();
        int playerTeam = binaryReader.ReadUInt16();

        int enemyNum = binaryReader.ReadUInt16();
        int enemyTeam = binaryReader.ReadUInt16();

        CGameManger.Instance.SetTeamACount(tempACount);
        CGameManger.Instance.SetTeamBCount(tempBCount);

        SceneManager.LoadScene("Game"); // 여기를 로딩씬으로 대처

        App app = Transform.FindObjectOfType<App>();
        app.UdpInit();
    }

    private void Test()
    {
        Vector3 vector;

        int num = binaryReader.ReadUInt16();

        vector.x = binaryReader.ReadSingle();
        vector.y = binaryReader.ReadSingle();
        vector.z = binaryReader.ReadSingle();

        Spawn spawn = Transform.FindObjectOfType<Spawn>();

        spawn.SetPosition(num, vector);
    }
}
