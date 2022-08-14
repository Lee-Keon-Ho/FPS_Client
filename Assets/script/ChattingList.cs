using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChattingList : MonoBehaviour
{
    public TextMeshProUGUI chattingList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnList(byte[] b)
    {
        chattingList.text = "¤¾¤·";
    }
}
