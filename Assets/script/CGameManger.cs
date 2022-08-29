using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManger : MonoBehaviour
{
    private static CGameManger instance = null;
    private int teamACount;
    private int teamBCount;

    void Awake()
    {
        if(null == instance)
        {
            instance = this;
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
    

    public void SetTeamACount(int _num) { teamACount = _num; }
    public void SetTeamBCount(int _num) { teamBCount = _num; }
    public int GetTeamACount() { return teamACount; }
    public int GetTeamBCount() { return teamBCount; }
}
