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
    private int iTeamB = 0;

    public bool gameStart = false;
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
    }

    public void SetTeamACount(int _num) { teamACount = _num; }
    public void SetTeamBCount(int _num) { teamBCount = _num; }
    public int GetTeamACount() { return teamACount; }
    public int GetTeamBCount() { return teamBCount; }

    public void TeamAPosition(Vector3 vector3) { teamA = vector3; }
    public void TeamBPosition(Vector3 vector3) { teamB = vector3; }

    public Vector3 GetPosition() { return teamA; }

    public void test(int num, Vector3 vector) 
    {
        iTeamB = num; teamB = vector;
    }

    public Vector3 GetB() { return teamB; }
}
