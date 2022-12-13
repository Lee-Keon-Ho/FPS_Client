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

    CPlayer[] players;
    CPlayer tempPlayer;
    void Start()
    {
        CGameManager gm = CGameManager.Instance;

        app = Transform.FindObjectOfType<App>();

        m_HP.text = app.GetPlayer().GetHp().ToString();

        count = CGameManager.Instance.GetPlayerCount();

        gameOverTime = 0f;
        players = new CPlayer[count];
        for (int i = 0; i < count; i++)
        {
            ranging[i].SetActive(true);
            rankingName[i].gameObject.SetActive(true);
            Kill[i].gameObject.SetActive(true);
            Death[i].gameObject.SetActive(true);

            rankingName[i].text = gm.GetPlayer(i).GetName();
            Kill[i].text = gm.GetPlayer(i).GetKill().ToString();
            Death[i].text = gm.GetPlayer(i).GetDeath().ToString();

            players[i] = gm.GetPlayer(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_HP.text = app.GetPlayer().GetHp().ToString();

        for(int i = 0; i < count; i++)
        {
            for(int j = 0; j < count - 1 - i; j++)
            {
                if(players[i].GetKill() < players[j + 1].GetKill())
                {
                    tempPlayer = players[i];
                    players[j] = players[j + 1];
                    players[j + 1] = tempPlayer;
                }
            }
        }

        for (int i = 0; i < count; i++)
        {
            rankingName[i].text = players[i].GetName();
            MaxKill = players[i].GetKill();
            Kill[i].text = MaxKill.ToString();

            if (MaxKill >= 5) winPlayer.text = rankingName[i].text;
            
            Death[i].text = players[i].GetDeath().ToString();

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            m_Status.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            m_Status.SetActive(false);
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
