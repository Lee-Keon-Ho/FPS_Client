using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomButton : MonoBehaviour
{
    GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        gameObject = GameObject.Find("serverObject");
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
}
