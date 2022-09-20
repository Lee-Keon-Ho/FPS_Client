using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] A_spawn;
    public GameObject[] B_spawn;
    public GameObject m_player;

    void Awake()
    {
        CGameManager gameManager = CGameManager.Instance;
        int teamACount = gameManager.GetTeamACount();
        int teamBCount = gameManager.GetTeamBCount();

        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();

        int team = player.GetNumber();
        A_spawn[0].SetActive(true);
        B_spawn[0].SetActive(true);
        if (team == 1)
        {
            for (int i = 0; i < teamACount; i++)
            {
                A_spawn[i].SetActive(false);
            }
            for (int i = 0; i < teamBCount; i++)
            {
                B_spawn[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < teamACount; i++)
            {
                A_spawn[i].SetActive(true);
            }
            for (int i = 0; i < teamBCount; i++)
            {
                B_spawn[i].SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();

        if (player.GetNumber() == 1)
        {
            m_player.transform.position = A_spawn[0].transform.position;
        }
        if (player.GetNumber() == 2)
        {
            m_player.transform.position = B_spawn[0].transform.position;
        }
        CGameManager.Instance.gameStart = true;
        //app.Test(m_player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        App app = Transform.FindObjectOfType<App>();
        CPlayer player = app.GetPlayer();

        CGameManager.Instance.TeamAPosition(m_player.transform.position);
        if (player.GetNumber() != 1)
        {
            A_spawn[0].transform.position = CGameManager.Instance.GetB();
        }
        if (player.GetNumber() != 2)
        {
            B_spawn[0].transform.position = CGameManager.Instance.GetB();
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
                A_spawn[0].transform.position = _vector;
            }
            else
            {
                B_spawn[0].transform.position = _vector;
            }
        }
    }
}
