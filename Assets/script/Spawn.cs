using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] A_spawn;
    public GameObject[] B_spawn;
    public GameObject player;

    void Awake()
    {
        CGameManger gameManager = CGameManger.Instance;
        int teamACount = gameManager.GetTeamACount();
        int teamBCount = gameManager.GetTeamBCount();

        for(int i = 0; i < teamACount; i++)
        {
            A_spawn[i].SetActive(false);
        }

        for (int i = 0; i < teamBCount; i++)
        {
            B_spawn[i].SetActive(false);
        }

        App app = Transform.FindObjectOfType<App>();
        CPlayer Cplayer = app.GetPlayer();

        if(Cplayer.GetNumber() == 1)
        {
            player.transform.position = A_spawn[0].transform.position;
        }
        if (Cplayer.GetNumber() == 2)
        {
            player.transform.position = B_spawn[0].transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
