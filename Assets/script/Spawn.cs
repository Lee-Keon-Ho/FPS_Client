using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] peer;
    public GameObject m_player;
    private CUdp udp;
    private CGameManager gm;
    int playerCount;
    public PeerActions[] pa;
    public Transform[] spawn;
    private bool[] bSpawn;

    CPlayer player;
    void Awake()
    {
        bSpawn = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            bSpawn[i] = false;
        }
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
                peer[i].SetActive(true);
                m_player.transform.position = spawn[i].transform.position;
                gm.GetPlayer(i).SetPosition(m_player.transform.position);
                peer[i].SetActive(false);
            }
            else
            {
                peer[i].transform.position = spawn[i].transform.position;
                gm.GetPlayer(i).SetPosition(peer[i].transform.position);
                peer[i].SetActive(true);
                pa[i].SetPlayer(i);
            }
        }
    }
}
