using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] spawn;
    public GameObject m_player;
    private CUdp udp;
    private CGameManager gm;
    int playerCount;
    void Awake()
    {
        gm = CGameManager.Instance;
        int teamACount = gm.GetTeamACount();
        int teamBCount = gm.GetTeamBCount();
        playerCount = gm.GetPlayerCount();
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();

        for(int i = 0; i < playerCount; i++)
        {
            if(player.GetNumber() == i + 1)
            {
                spawn[i].SetActive(false);
            }
            else
            {
                spawn[i].SetActive(true);
            }
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();
        udp = app.GetUdp();

        for(int i = 0; i < playerCount; i++)
        {
            if(player.GetNumber() == i + 1)
            {
                m_player.transform.position = spawn[i].transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();

        udp.PeerPosition(m_player.transform.position, m_player.transform.rotation, player.GetSocket());
        Debug.Log(m_player.transform.rotation);
        for(int i = 0; i < playerCount; i++)
        {
            if(player.GetSocket() != gm.GetPlayer(i).GetSocket())
            {
                spawn[i].transform.position = gm.GetPosition(i);
                spawn[i].transform.rotation = gm.GetRotation(i);
            }
        }
    }

    public void SetPosition(int _num, Vector3 _vector)
    {
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();
        if (player.GetNumber() != _num)
        {
            if(_num == 1)
            {
                spawn[0].transform.position = _vector;
            }
            else
            {
                spawn[0].transform.position = _vector;
            }
        }
    }
}
