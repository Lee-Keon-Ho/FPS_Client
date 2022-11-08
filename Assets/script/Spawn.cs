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

    CPlayer player;

    private float timer = 0f;
    void Awake()
    {
        
    }

    void Start()
    {
        gm = CGameManager.Instance;
        playerCount = gm.GetPlayerCount();
        App app = Transform.FindObjectOfType<App>();
        player = app.GetPlayer();

        for (int i = 0; i < playerCount; i++)
        {
            if (player.GetNumber() == i + 1)
            {
                //m_player.transform.position = spawn[i].transform.position;
                spawn[i].SetActive(false);
            }
            else
            {
                spawn[i].transform.position = m_player.transform.position;
                spawn[i].SetActive(true);
                pa[i].SetPlayer(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;

        //if (timer >= 1f)
        //{
        //    for (int i = 0; i < playerCount; i++)
        //    {
        //        if (player.GetSocket() != gm.GetPlayer(i).GetSocket())
        //        {
        //            spawn[i].transform.GetChild(0).position = gm.GetPosition(i); ;
        //            spawn[i].transform.GetChild(0).rotation = Quaternion.Euler(0f, gm.GetRotation(i), 0f);
        //        }
        //    }
        //    timer = 0f;
        //}
    }
}
