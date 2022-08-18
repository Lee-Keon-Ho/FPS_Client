using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LobbyButton : MonoBehaviour
{
    public GameObject inputCreate;
    public GameObject exitWindow;
    GameObject gameObject;
    private int test = 0;
    private void Awake()
    {
        gameObject = GameObject.Find("serverObject");
    }
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
        gameObject.GetComponent<App>().OnCreate(_text);
    }

    public void OnCreateExitButton()
    {
        inputCreate.SetActive(false);
    }

    public void OnExitButton()
    {
        exitWindow.SetActive(true);
    }

    public void OnExitNoButton()
    {
        exitWindow.SetActive(false);
    }

    public void OnExitOkButton()
    {
        gameObject.GetComponent<App>().OnLogOut();
    }

    public void ScrollDown()
    {
        test += 1;
        Debug.Log(test);
    }

    public void ScrollUp()
    {

    }
}
