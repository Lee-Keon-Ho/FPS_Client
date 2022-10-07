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

    public TextMeshProUGUI[] textMesh;

    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    private void Update()
    {
        App app = Transform.FindObjectOfType<App>();

        if(app.GetBoolUdp() != 1)
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
            int count = CGameManager.Instance.GetPlayerCount();

            for(int i = 0; i < count; i++)
            {
                Debug.Log(CGameManager.Instance.GetPlayer(i).GetSocket().ToString());
                //textMesh[i].text = CGameManager.Instance.GetPlayer(i).GetSocket().ToString();
            }
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
