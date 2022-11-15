using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Camera camera;

    public float speed = 2f;
    public float runSpeed = 4f;
    public float finalSpeed;

    public bool toggleCameraRotation;

    public float smoothness = 10f;

    private float rotX;
    private float rotY;

    private int state;

    private CPlayer player;
    CUdp udp;

    bool run;
    bool walk;
    bool aiming;
    bool Key_Up_Down;

    //Action
    const int countOfDamageAnimations = 3;
    int lastDamageAnimation = -1;

    public GameObject bullet;
    public GameObject firePosition;
    public GameObject character;

    private GUIStyle style = new GUIStyle();

    private void Awake()
    {
        animator = GetComponent<Animator>();

        run = false;
        walk = false;
        aiming = false;
        Key_Up_Down = false;

        style.normal.textColor = Color.red;
        style.fontSize = 20;
    }

    void Start()
    {
        App app = Transform.FindObjectOfType<App>();
        player = app.GetPlayer();
        udp = app.GetUdp();

        animator = this.GetComponent<Animator>();
        camera = Camera.main;

        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 1: 
                // 함수화 해서 넣어주고
                break;
            case 2:
                // 함수화
                break;
            case 3:
                // 함수화
                break;
        }

        // 마우스는 별도로
        // 마우스는 초당 위치로 5번을 넘을 수 없다.

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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (walk) state = 2;
            run = true;
            Key_Up_Down = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (walk) state = 1;
            run = false;
            Key_Up_Down = true;
        }


        if (rotX != 0 || rotY != 0) // 마우스는 초당 위치로 5번을 넘을 수 없다.
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                walk = true;
                Key_Up_Down = true;

                if (run) state = 2;
                else state = 1;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                walk = false;
                Key_Up_Down = true;

                state = 0;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Key_Up_Down = true;
                state = 3;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                Key_Up_Down = true;
                state = 0;
            }
            else if(Input.GetMouseButtonDown(1))
            {
                if(aiming)
                {
                    //udp.KeyDown(player.GetSocket(), 0, this.transform.position, this.transform.eulerAngles.y);
                    character.SetActive(true);
                    Idle();
                    aiming = false;
                }
                else
                {
                    //udp.KeyDown(player.GetSocket(), 4, this.transform.position, this.transform.eulerAngles.y);
                    character.SetActive(false);
                    Aiming();
                    aiming = true;
                }
            }
            else
            {
                udp.MouseMove(player.GetSocket(), this.transform.eulerAngles.y);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                walk = true;
                Key_Up_Down = true;

                if (run) state = 2;
                else state = 1;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                walk = false;
                Key_Up_Down = true;
                state = 0;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Key_Up_Down = true;
                state = 3;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                Key_Up_Down = true;
                state = 0;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (aiming)
                {
                    //udp.KeyDown(player.GetSocket(), 0, this.transform.position, this.transform.eulerAngles.y);
                    character.SetActive(true);
                    Idle();
                    aiming = false;
                }
                else
                {
                    //udp.KeyDown(player.GetSocket(), 4, this.transform.position, this.transform.eulerAngles.y);
                    character.SetActive(false);
                    Aiming();
                    aiming = true;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, firePosition.transform.position, firePosition.transform.rotation);
        }

        if(Key_Up_Down)
        {
            udp.InputKey(player.GetSocket(), state, this.transform.position, this.transform.eulerAngles.y);
            Key_Up_Down = false;
        }

        PlayerState();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5f, 5f, Screen.width, 20f), "player : " + state.ToString(), style);
        GUI.Label(new Rect(5f, 5f + 20f, Screen.width, 20f), "Run : " + run.ToString(), style);
        GUI.Label(new Rect(5f, 5f + 40f, Screen.width, 20f), "Walk : " + walk.ToString(), style);
    }
    void LateUpdate()
    {
        if(toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    private void PlayerState()
    {
        switch (state)
        {
            case 0:
                Idle();
                break;
            case 1:
                Walk();
                this.transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                break;
            case 2:
                Run();
                this.transform.Translate(Vector3.forward * 5 * Time.deltaTime);
                break;
        }
    }

    public void Idle() // state를 변경하는 함수 // 한번만 사용하면 된다.
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
