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
    public PeerActions[] pa;

    private float timer = 0f;
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
            else
            {
                spawn[i].transform.position = gm.GetPosition(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();

        timer += Time.deltaTime;

        if(timer >= 0.5f) // 누른다는 생각을 가져라 키보드 값을 보내야 한다.
        {
            udp.PeerPosition(m_player.transform.position, m_player.transform.rotation, player.GetSocket(), player.GetAction());
            timer = 0f;
        }
        
        
        for(int i = 0; i < playerCount; i++)
        {
            if(player.GetSocket() != gm.GetPlayer(i).GetSocket())
            {
                Vector3 speed = Vector3.zero;
                spawn[i].transform.position = Vector3.SmoothDamp(spawn[i].transform.position, gm.GetPosition(i), ref speed, 0.1f);
                //spawn[i].transform.position = gm.GetPosition(i);
                spawn[i].transform.rotation = gm.GetRotation(i);
                pa[i].SetAction(gm.GetPlayer(i).GetAction());
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
