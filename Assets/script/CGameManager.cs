using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour
{
    private static CGameManager instance = null;
    private int teamACount;
    private int teamBCount;
    private int playerCount;
    private CPlayer[] m_player = new CPlayer[8];

    public int number;
    private Vector3 teamA;
    private Vector3 teamB;
    public ushort gameStartTest = 0; // 2022-10-07 test

    public bool gameStart = false;
    public ushort gameSocket = 0;
    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            instance.Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public static CGameManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void Init()
    {
        playerCount = 0;
        teamACount = 0;
        teamBCount = 0;

        for(int i = 0; i < 8; i++)
        {
            m_player[i] = new CPlayer();
        }
    }

    public void SetTeamACount(int _num) { teamACount = _num; }
    public void SetTeamBCount(int _num) { teamBCount = _num; }
    public int GetTeamACount() { return teamACount; }
    public int GetTeamBCount() { return teamBCount; }

    public void TeamAPosition(Vector3 vector3) { teamA = vector3; }
    public void TeamBPosition(Vector3 vector3) { teamB = vector3; }

    public Vector3 GetPosition() { return teamA; }


    public Vector3 GetB() { return teamB; }

    public void SetPlayers(int _num, uint _socket, uint _addr, ushort _port, string _addrStr)
    {
        m_player[_num].SetSocet(_socket);
        m_player[_num].SetAddr(_addr, _port);
        m_player[_num].SetAddrStr(_addrStr);
    }

    public CPlayer GetPlayer(int _num) { return m_player[_num]; }

    public void SetPlayerCount(int _count) { playerCount = _count; }

    public int GetPlayerCount() { return playerCount; }
}
