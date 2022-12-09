using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI m_HP;
    public GameObject m_Status;
    public TextMeshProUGUI[] rankingName;
    public GameObject[] ranging;
    public TextMeshProUGUI[] Kill;
    public TextMeshProUGUI[] Death;
    public GameObject GameOver;
    public TextMeshProUGUI winPlayer;

    private App app;
    private int count;

    float gameOverTime;

    int MaxKill;

    void Start()
    {
        app = Transform.FindObjectOfType<App>();

        m_HP.text = app.GetPlayer().GetHp().ToString();

        count = CGameManager.Instance.GetPlayerCount();

        gameOverTime = 0f;

        for (int i = 0; i < count; i++)
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
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            m_Status.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            m_Status.SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            rankingName[i].text = CGameManager.Instance.GetPlayer(i).GetName();
            MaxKill = CGameManager.Instance.GetPlayer(i).GetKill();
            if (MaxKill >= 1)
            {
                Kill[i].text = MaxKill.ToString();
                winPlayer.text = rankingName[i].text;
            }
            else
            {
                Kill[i].text = MaxKill.ToString();
            }
            Death[i].text = CGameManager.Instance.GetPlayer(i).GetDeath().ToString();

        }

        if (CGameManager.Instance.GetGameOver())
        {
            GameOver.SetActive(true);
            gameOverTime += Time.deltaTime;

            if (gameOverTime >= 5.0f)
            {
                GameOver.SetActive(false);
                for (int i = 0; i < CGameManager.Instance.GetPlayerCount(); i++)
                {
                    CGameManager.Instance.GetPlayer(i).Init();
                }
                app.m_socket.GameOver();
                app.m_socket.RoomState();
                app.m_udp.CloseSocket();
                CGameManager.Instance.SetGameOver(false);

                SceneManager.LoadScene("Room");
            }
        }
    }
}
