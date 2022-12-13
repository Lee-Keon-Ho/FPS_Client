using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CRoom : MonoBehaviour
{
    public TextMeshProUGUI[] m_playerList;
    
    public Button m_readyButton;
    public TextMeshProUGUI m_readyText;
    public TextMeshProUGUI m_name;
    private App app;
    int boss;
    private void Awake()
    {
        app = Transform.FindObjectOfType<App>();
        if (app.GetBoss() == 0)
        {
            boss = 0;
            m_readyText.text = "START";
            m_readyButton.interactable = false;
            m_readyText.color = new Color(1, 1, 1);
        }
        else
        {
            boss = 1;
            m_readyText.text = "READY";
        }

        m_name.text = app.GetName();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        boss = app.GetBoss();
        int ready = app.GetReady();
        if (boss == 0)
        {
            m_readyText.text = "START";
            if (ready == 0)
            {
                m_readyButton.interactable = false;
                m_readyText.color = new Color(1, 1, 1);
            }
            else
            {
                m_readyButton.interactable = true;
                m_readyText.color = new Color(255, 0, 0);
            }
        }
        else
        {
            m_readyText.text = "READY";
            if (ready == 0)
            {
                m_readyText.color = new Color(255, 255, 255);
            }
            else
            {
                m_readyText.color = new Color(255, 0, 0);
            }
        }
    }

    public void playerInfoReset()
    {
        for(int i = 0; i < 8; i++)
        {
            m_playerList[i].text = "";
        }
    }

    public void OnPlayerListInfo(ushort _ready, string _name, int _boss, int _index)
    {
        m_playerList[_index].text = _name;
        if (_boss == 0)
        {
            m_playerList[_index].color = new Color(255, 0, 0);
        }
        else
        {
            m_playerList[_index].color = new Color(255, 255, 255);
        }
    }

    public int GetBoss() { return boss; }
}
