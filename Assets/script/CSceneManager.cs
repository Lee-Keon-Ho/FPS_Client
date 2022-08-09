using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSceneManager : MonoBehaviour
{
    private static CSceneManager instance;
    public static CSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<CSceneManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<CSceneManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(int _index)
    {
        if (_index == 1)
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
