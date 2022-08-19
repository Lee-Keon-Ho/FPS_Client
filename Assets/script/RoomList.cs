using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    public GameObject[] gameObject = new GameObject[10];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomListUpdate(int _num, string _name, int _playerCount, int _state, int _index)
    {
        if(_num != 0)
        {
            gameObject[_index].SetActive(true);
            gameObject[_index].transform.GetChild(0).GetComponent<Text>().text = _num.ToString();
            gameObject[_index].transform.GetChild(1).GetComponent<Text>().text = _name;
            gameObject[_index].transform.GetChild(2).GetComponent<Text>().text = _playerCount.ToString() + "/8";
            if (_state == 0)
            {
                gameObject[_index].transform.GetChild(3).GetComponent<Text>().text = "게임중";
            }
            else
            {
                gameObject[_index].transform.GetChild(3).GetComponent<Text>().text = "대기중";
            }
        }
        else
        {
            gameObject[_index].SetActive(false);
        }
    }
}
