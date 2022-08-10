using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacter : MonoBehaviour
{
    Camera characterCamera;
    // Start is called before the first frame update
    private void Awake()
    {
        characterCamera = GameObject.Find("characterCamera").GetComponent<Camera>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
