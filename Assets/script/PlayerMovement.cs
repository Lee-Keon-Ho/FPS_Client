using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Camera camera;
    CharacterController controller;

    public float speed = 2f;
    public float runSpeed = 4f;
    public float finalSpeed;

    public bool toggleCameraRotation;
    public bool run;

    public float smoothness = 10f;

    private float rotX;
    private float rotY;

    private CPlayer player;
    CUdp udp;

    bool stop = false;

    //Action
    const int countOfDamageAnimations = 3;
    int lastDamageAnimation = -1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        App app = Transform.FindObjectOfType<App>();
        player = app.GetPlayer();
        udp = app.GetUdp();

        animator = this.GetComponent<Animator>();
        camera = Camera.main;
        //controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        rotX = -(Input.GetAxis("Mouse Y"));
        rotY = Input.GetAxis("Mouse X"); 
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }

        run = false;
        if (rotX != 0 || rotY != 0)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                run = true;
                udp.KeyDownW(player.GetSocket(), 2, this.transform.position, this.transform.eulerAngles.y); // 현재 위치와 방향을 같이 보낸다.
                this.transform.Translate(Vector3.forward * 2 * Time.deltaTime);
                Run();
            }
            else if (Input.GetKey(KeyCode.W))
            {
                udp.KeyDownW(player.GetSocket(), 1, this.transform.position, this.transform.eulerAngles.y);
                Walk();
                this.transform.Translate(Vector3.forward * 1 * Time.deltaTime);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                stop = true;
                udp.KeyUpW(player.GetSocket(), 0, this.transform.position, this.transform.eulerAngles.y);
                Stay();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                udp.KeyDownW(player.GetSocket(), 3, this.transform.position, this.transform.eulerAngles.y);
                Walk();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                udp.KeyUpW(player.GetSocket(), 0, this.transform.position, this.transform.eulerAngles.y);
                Stay();
            }
            else
            {
                udp.MouseMove(player.GetSocket(), this.transform.eulerAngles.y);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                run = true;
                udp.KeyDownW(player.GetSocket(), 2, this.transform.position, this.transform.eulerAngles.y); // 현재 위치와 방향을 같이 보낸다.
                this.transform.Translate(Vector3.forward * 2 * Time.deltaTime);
                Run();
            }
            else if (Input.GetKey(KeyCode.W))
            {
                udp.KeyDownW(player.GetSocket(), 1, this.transform.position, this.transform.eulerAngles.y);
                Walk();
                this.transform.Translate(Vector3.forward * 1 * Time.deltaTime);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                stop = true;
                udp.KeyUpW(player.GetSocket(), 0, this.transform.position, this.transform.eulerAngles.y);
                Stay();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                udp.KeyDownW(player.GetSocket(), 3, this.transform.position, this.transform.eulerAngles.y);
                Walk();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                udp.KeyUpW(player.GetSocket(), 0, this.transform.position, this.transform.eulerAngles.y);
                Stay();
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && stop)
        {
            udp.KeyUpW(player.GetSocket(), 0, this.transform.position, this.transform.eulerAngles.y);
            stop = false;
        }
        //InputMovement();
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

        //controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
    }

    public void Stay()
    {
        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 0f);
    }

    public void Walk()
    {
        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 0.5f);
    }

    public void Run()
    {
        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 1f);
    }

    public void Attack()
    {
        Aiming();
        animator.SetTrigger("Attack");
    }

    public void Death()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            animator.Play("Idle", 0);
        else
            animator.SetTrigger("Death");
    }

    public void Damage()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) return;
        int id = Random.Range(0, countOfDamageAnimations);
        if (countOfDamageAnimations > 1)
            while (id == lastDamageAnimation)
                id = Random.Range(0, countOfDamageAnimations);
        lastDamageAnimation = id;
        animator.SetInteger("DamageID", id);
        animator.SetTrigger("Damage");
    }

    public void Jump()
    {
        animator.SetBool("Squat", false);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Aiming", false);
        animator.SetTrigger("Jump");
    }

    public void Aiming()
    {
        animator.SetBool("Squat", false);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Aiming", true);
    }

    public void Sitting()
    {
        animator.SetBool("Squat", !animator.GetBool("Squat"));
        animator.SetBool("Aiming", false);
    }
}
