using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class App : MonoBehaviour
{
    public CSocket m_socket;
    private string m_name;
    private void Awake()
    {
        m_socket = new CSocket();

        m_socket.Init("222.113.24.225", 30002);
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

    public void OnLogin(TextMeshProUGUI _textMesh)
    {
        if(_textMesh.text.Length > 4)
        {
            m_socket.Login(_textMesh);
            m_name = _textMesh.text;
            m_socket.UserList();
            m_socket.RoomList();
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

    public string GetName() { return m_name; }

    public void List()
    {
        m_socket.UserList();
        m_socket.RoomList();
    }

    public void RoomOut()
    {
        m_socket.RoomOut();
    }
}