using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomButton : MonoBehaviour
{
    GameObject gameObject;
    private void Awake()
    {
        gameObject = GameObject.Find("serverObject");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackButton()
    {
        gameObject.GetComponent<App>().RoomOut();
        gameObject.GetComponent<App>().List();
    }

    public void OnTemaA()
    {
        gameObject.GetComponent<App>().TeamChange(0);
    }

    public void OnTeamB()
    {
        gameObject.GetComponent<App>().TeamChange(1);
    }

    public void OnReadyButton()
    {
        int boss = gameObject.GetComponent<App>().GetBoss();
        if(boss == 0)
        {
            gameObject.GetComponent<App>().GameStart();
        }
        else
        {
            gameObject.GetComponent<App>().ReadButton();
        }
        
    }

    public void OnStart()
    {
        gameObject.GetComponent<App>().GameStart();
    }
}
