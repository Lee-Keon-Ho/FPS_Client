using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Chatting : MonoBehaviour
{
    public TextMeshProUGUI chatText;
    public TMP_InputField chatInputField;
    GameObject serverObject;

    private void Awake()
    {
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
}
