using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class App : MonoBehaviour
{
    public CSocket m_socket;
    public CPlayer m_player;
   
    private void Awake()
    {
        m_socket = new CSocket();
        m_player = new CPlayer();
        m_player.Init();
        m_socket.Init("211.218.197.86", 30002);
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_socket.RunLoop();
    }

    private void OnDestroy()
    {
        m_socket.Delete();
    }

    public void SetPlayerInfo(int _boss, int _ready)
    {
        m_player.SetBoss(_boss);
        m_player.SetReady(_ready);
    }

    public void OnLogin(TextMeshProUGUI _textMesh)
    {
        if(_textMesh.text.Length > 4)
        {
            m_socket.Login(_textMesh);
            m_player.SetName(_textMesh.text);
            List();
        }
    }

    public void OnReturn(TextMeshProUGUI _textMesh)
    {
        m_socket.ChatSend(_textMesh);
    }

    public void OnCreate(TextMeshProUGUI _textMesh)
    {
        m_socket.CreateRoom(_textMesh);
    }

    public void OnSelected(int _num)
    {
        m_socket.Selected(_num);
    }

    public void OnLogOut()
    {
        m_socket.LogOut();
    }

    public void List()
    {
        m_socket.UserList();
        m_socket.RoomList();
    }

    public void RoomOut()
    {
        m_socket.RoomOut();
    }

    public void TeamChange(int _num)
    {
        m_socket.TeamChange(_num);
    }

    public void ReadButton()
    {
        m_socket.Ready();
    }

    public void GameStart()
    {
        m_socket.GameStart();
    }

    public void Test(Vector3 _vector3)
    {

    }

    public string GetName() { return m_player.GetName(); }
    public int GetBoss() { return m_player.GetBoss(); }
    public void SetBoss(int _boss) { m_player.SetBoss(_boss); }
    public void SetReady(int _ready) { m_player.SetReady(_ready); }
    public int GetReady() { return m_player.GetReady(); }
    public void SetNumber(int _num) { m_player.SetNumber(_num); }

    public CPlayer GetPlayer() { return m_player; }
}