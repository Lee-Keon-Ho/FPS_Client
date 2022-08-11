using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacter : MonoBehaviour
{
    private float speed = 3f;
    public GameObject gameObject;
    public Transform rightGunBone;

    // Start is called before the first frame update
    private void Awake()
    {
        //if (rightGunBone.childCount > 0)
        //    Destroy(rightGunBone.GetChild(0).gameObject);
        
        //if (hand.rightGun != null)
        //{
        //    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
        //    newRightGun.transform.parent = rightGunBone;
        //    newRightGun.transform.localPosition = Vector3.zero;
        //    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //}
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            gameObject.transform.Rotate(0f, -Input.GetAxis("Mouse X") * speed, 0f, Space.World);
        }
    }
}
