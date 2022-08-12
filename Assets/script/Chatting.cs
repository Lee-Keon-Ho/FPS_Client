using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Chatting : MonoBehaviour
{
    public TextMeshProUGUI charText;
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
        if (charText.text.Length != 0)
        {
            CSocket.Instance.ChatSend(charText);
        }
        charText.text = null;
    }
}
