using System.Collections;
using System.Collections.Generic;
using System;   
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChattingList : MonoBehaviour
{
    public TextMeshProUGUI[] chattingList = new TextMeshProUGUI[20];
    public Scrollbar scrollbar;
    int chatCount;
    // Start is called before the first frame update
    private void Awake()
    {
        chatCount = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
