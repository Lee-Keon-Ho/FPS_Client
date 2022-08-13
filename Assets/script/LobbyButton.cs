using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LobbyButton : MonoBehaviour
{
    public Button CreateButton;
    public GameObject inputCreate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCreateButton()
    {
        inputCreate.SetActive(true);
    }

    public void OnCreateOkButton(TextMeshProUGUI _text)
    {
        // send
    }

    public void OnCreateExitButton()
    {
        inputCreate.SetActive(false);
    }
}
