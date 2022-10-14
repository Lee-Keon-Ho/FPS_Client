using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    private float m_time = 1;

    public TextMeshProUGUI[] peer;
    public TextMeshProUGUI[] ok;
    public TextMeshProUGUI[] socket;
    public TextMeshProUGUI[] addr;
    public TextMeshProUGUI[] port;

    int count;

    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    void Start()
    {
        count = CGameManager.Instance.GetPlayerCount();

        for(int i = 0; i < count; i++)
        {
            peer[i].gameObject.SetActive(true);
        }

        StartCoroutine(LoadSceneProcess());
    }

    private void Update()
    {
        App app = Transform.FindObjectOfType<App>();

        if(CGameManager.Instance.gameSocket == 0) // gm에서 다른걸로 해야한다.
        {
            if (m_time >= 1f)
            {
                CUdp udp = app.GetUdp();

                udp.SendSocket(app.GetSocket());

                m_time = 0f;
            }
            m_time += Time.deltaTime;
        }
        else
        {
            if (m_time >= 1f)
            {
                CUdp udp = app.GetUdp();

                udp.PeerConnect();

                m_time = 0f;
            }
            m_time += Time.deltaTime;
        }

        int count = CGameManager.Instance.GetPlayerCount();

        for (int i = 0; i < count; i++)
        {
            CPlayer player = CGameManager.Instance.GetPlayer(i);
            socket[i].text = player.GetSocket().ToString();
            addr[i].text = player.GetAddrStr();
            port[i].text = player.GetPort().ToString();
        }

    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                if(CGameManager.Instance.gameStartTest == 1)
                {
                    timer += Time.unscaledDeltaTime;
                    progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                    if (progressBar.fillAmount >= 1f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}
