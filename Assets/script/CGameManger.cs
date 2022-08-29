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

}
