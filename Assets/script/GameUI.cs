using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI m_HP;
    public GameObject m_Status;
    public TextMeshProUGUI[] rankingName;
    public GameObject[] ranging;
    public TextMeshProUGUI[] Kill;
    public TextMeshProUGUI[] Death;

    private App app;
    private int count;
    void Start()
    {
        app = Transform.FindObjectOfType<App>();

        m_HP.text = app.GetPlayer().GetHp().ToString();

        count = CGameManager.Instance.GetPlayerCount();

        for(int i = 0; i < count; i++)
        {
            ranging[i].SetActive(true);
            rankingName[i].gameObject.SetActive(true);
            Kill[i].gameObject.SetActive(true);
            Death[i].gameObject.SetActive(true);

            rankingName[i].text = CGameManager.Instance.GetPlayer(i).GetName();
            Kill[i].text = CGameManager.Instance.GetPlayer(i).GetKill().ToString();
            Death[i].text = CGameManager.Instance.GetPlayer(i).GetDeath().ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_HP.text = app.GetPlayer().GetHp().ToString();

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            m_Status.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            m_Status.SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            rankingName[i].text = CGameManager.Instance.GetPlayer(i).GetName();
            Kill[i].text = CGameManager.Instance.GetPlayer(i).GetKill().ToString();
            Death[i].text = CGameManager.Instance.GetPlayer(i).GetDeath().ToString();
        }
    }
}
