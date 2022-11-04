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
        playerCount = gm.GetPlayerCount();
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();

        for (int i = 0; i < playerCount; i++)
        {
            if (player.GetNumber() == i + 1)
            {
                spawn[i].SetActive(false);
                m_player.transform.position = spawn[i].transform.position;
                Debug.Log(m_player.transform.position);
            }
            else
            {
                spawn[i].SetActive(true);
                pa[i].SetPlayer(i);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    //    App app = Transform.FindObjectOfType<App>();
    //    CPlayer player = app.GetPlayer();

    //    timer += Time.deltaTime;

    //    if (timer >= 1f) // 누른다는 생각을 가져라 키보드 값을 보내야 한다.
    //    {
    //        //udp.PeerPosition(m_player.transform.position, m_player.transform.rotation, player.GetSocket(), player.GetAction());
    //        for (int i = 0; i < playerCount; i++)
    //        {
    //            if (player.GetSocket() != gm.GetPlayer(i).GetSocket())
    //            {
    //                spawn[i].transform.position = gm.GetPosition(i);
    //                spawn[i].transform.rotation = gm.GetRotation(i);
    //                pa[i].SetAction(gm.GetPlayer(i).GetAction());
    //            }
    //        }
    //        timer = 0f;
    //    }

    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        udp.KeyDownW(player.GetSocket(), player.GetAction());
    //    }
    //    if (Input.GetKeyUp(KeyCode.W))
    //    {
    //        udp.KeyDownW(player.GetSocket(), player.GetAction());
    //    }
    }
}
