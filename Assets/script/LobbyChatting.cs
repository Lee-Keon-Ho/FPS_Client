using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LobbyChatting : MonoBehaviour
{
    public TextMeshProUGUI chatText;
    public TMP_InputField chatInputField;
    public TextMeshProUGUI[] chattingList = new TextMeshProUGUI[20];
    public Scrollbar scrollbar;
    GameObject serverObject;
    int chatCount;
    private void Awake()
    {
        chatCount = 0;
        serverObject = GameObject.Find("serverObject");
        chatInputField.onSubmit.AddListener(delegate { OnReturn(); });
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnReturn()
    {
        chatInputField.Select();
        if (chatText.text.Length > 1)
        {
            serverObject.GetComponent<App>().OnReturn(chatText);
        }
        chatInputField.text = "";
    }

    public void OnList(String _str)
    {
        if (chatCount > 19)
        {
            for (int i = 0; i < 19; i++)
            {
                chattingList[i].text = chattingList[i + 1].text;
            }
            chatCount = 19;
        }

        if (chatCount >= 9) scrollbar.value -= 0.09f;
        if (chatCount >= 19) scrollbar.value = 0.0f;

        chattingList[chatCount].text = _str;
        chatCount++;
    }
}
