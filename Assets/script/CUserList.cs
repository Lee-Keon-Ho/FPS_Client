using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CUserList : MonoBehaviour
{
    public GameObject[] list = new GameObject[41];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UserListUpdate(string _name, ushort _state, int _num)
    {
        list[_num].SetActive(true);
        switch (_state)
        {
            case 0:
                list[_num].transform.GetChild(0).GetComponent<Image>().color = Color.green;
                break;
            case 1:
                list[_num].transform.GetChild(0).GetComponent<Image>().color = Color.red;
                break;
            default:
                list[_num].transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
                break;
        }
        list[_num].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _name;
    }
}
