using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI m_HP;
    private App app;
    void Start()
    {
        app = Transform.FindObjectOfType<App>();

        m_HP.text = app.GetPlayer().GetHp().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        m_HP.text = app.GetPlayer().GetHp().ToString();
    }
}
