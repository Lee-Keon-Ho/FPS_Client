using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyCharacter : MonoBehaviour
{
    private float speed = 3f;
    public Camera getCamera;
    public GameObject gameObject;
    private RaycastHit hit;
    public Transform rightGunBone;
    public TextMeshProUGUI name;
    // Start is called before the first frame update
    private void Awake()
    {
        name.text = GameObject.Find("serverObject").GetComponent<App>().GetName();
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
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                gameObject.transform.Rotate(0f, -Input.GetAxis("Mouse X") * speed, 0f, Space.World);
            }
        }
    }
}
