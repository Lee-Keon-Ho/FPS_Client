using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManger : MonoBehaviour
{
    private static CGameManger instance = null;
    private int teamACount;
    private int teamBCount;
    private int playerCount;
    private CPlayer[] m_player = new CPlayer[8];

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
    
    public static CGameManger Instance
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
}
