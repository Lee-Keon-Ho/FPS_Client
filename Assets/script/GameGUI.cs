using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGUI : MonoBehaviour
{
    public Transform player;
    public Transform peer;
    public Transform peer1;

    private GUIStyle style = new GUIStyle();

    private void Awake()
    {
        style.normal.textColor = Color.red;
        style.fontSize = 20;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5f, 5f, Screen.width, 20f), "player : " + player.position.ToString(), style);
        GUI.Label(new Rect(5f, 5f + 20, Screen.width, 20f), "peer1 : " + peer.position.ToString(), style);
        GUI.Label(new Rect(5f, 5f + 40, Screen.width, 20f), "peer2 : " + peer1.position.ToString(), style);
    }
}
