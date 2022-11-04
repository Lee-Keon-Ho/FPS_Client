using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Camera camera;
    CharacterController controller;

    public float speed = 2f;
    public float runSpeed = 4;
    public float finalSpeed;

    public bool toggleCameraRotation;
    public bool run;

    public float smoothness = 10f;

    private CPlayer player;
    CUdp udp;

    void Start()
    {
        App app = Transform.FindObjectOfType<App>();
        player = app.GetPlayer();
        udp = app.GetUdp();

        animator = this.GetComponent<Animator>();
        camera = Camera.main;
        controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                udp.KeyDownW(player.GetSocket(), 1, this.transform.position, this.transform.rotation.y); // 현재 위치와 방향을 같이 보낸다.
            }
            if(Input.GetKeyUp(KeyCode.W))
            {
                udp.KeyUpW(player.GetSocket(), 0, this.transform.position, this.transform.rotation.y); // 현재 위치와 방향을 같이 보낸다.
            }
            run = false;
        }

        InputMovement();
    }

    void LateUpdate()
    {
        if(toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    void InputMovement()
    {
        finalSpeed = (run) ? runSpeed : speed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
    }
}
