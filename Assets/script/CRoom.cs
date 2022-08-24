using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CRoom : MonoBehaviour
{
    public TextMeshProUGUI[] m_teamA;
    public TextMeshProUGUI[] m_teamB;
    public Button m_readyButton;
    public TextMeshProUGUI m_readyText;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerInfo(int _boss)
    {
        if (_boss == 0)
        {
            m_readyText.text = "START";
        }
        else
        {
            m_readyText.text = "READY";
        }
    }

    public void playerInfoReset()
    {
        for(int i = 0; i < 4; i++)
        {
            m_teamA[i].text = "";
            m_teamB[i].text = "";
        }
    }

    public void OnPlayerListInfo(ushort _team, ushort _ready, string _name, int _boss, int _index)
    {
        if(_team == 0)
        {
            m_teamA[_index].text = _name;
            if (_boss == 0)
            {
                m_teamA[_index].color = new Color(255, 0, 0);
            }
            else
            {
                m_teamA[_index].color = new Color(255, 255, 255);
            }
        }
        else
        {
            m_teamB[_index].text = _name;
            if (_boss == 0)
            {
                m_teamB[_index].color = new Color(255, 0, 0);
            }
            else
            {
                m_teamB[_index].color = new Color(255, 255, 255);
            }
        }
    }
}
