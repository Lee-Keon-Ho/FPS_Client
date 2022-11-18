using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Camera camera;

    float rateTime;
    float nextTime;

    public float smoothness = 10f;

    private float rotX;
    private float rotY;

    private int m_state;

    private CPlayer player;
    CUdp udp;

    bool run;
    bool walk;
    bool aiming;

    //Action
    const int countOfDamageAnimations = 3;
    int lastDamageAnimation = -1;

    public GameObject bullet;
    public GameObject firePosition;
    public GameObject character;

    private GUIStyle style;

    private Vector3 dir;
    private Ray ray;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        style = new GUIStyle();

        rateTime = 0.1f;
        nextTime = 0.0f;

        run = false;
        walk = false;

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

        m_state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스는 별도로
        // 마우스는 초당 위치로 5번을 넘을 수 없다.
        //rotX = -(Input.GetAxis("Mouse Y"));
        //rotY = Input.GetAxis("Mouse X");

        switch (m_state)
        {
            case 0: // Idle
                Idle();
                break;
            case 1: // Walk
                Walk();
                break;
            case 2: // Run
                Run();
                break;
            case 3:
                Aiming();
                break;
        }

        MouseMove();
    }
    
    void LateUpdate()
    {
        Vector3 playerRotate = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
    }

    private void Idle()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            run = true; // 이걸 speed
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(run) ChangeStateRun();
            else ChangeStateWalk();
        }
        if(Input.GetMouseButtonDown(1))
        {
            ChangeStateAiming();
        }
    }

    private void Walk()
    {
        this.transform.Translate(Vector3.forward * 1 * Time.deltaTime);

        if (run)
        {
            ChangeStateRun();
        }
        else
        {
            if(Input.GetKeyUp(KeyCode.W))
            {
                ChangeStateIdle();
            }
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                run = true;
                ChangeStateRun();
            }
        }
    }

    private void Run()
    {
        this.transform.Translate(Vector3.forward * 3 * Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
            ChangeStateWalk();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            ChangeStateIdle();
        }
    }

    private void Aiming()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ChangeStateIdle();
        }
        if (Input.GetMouseButtonDown(0))
        {
            dir = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

            FireBullet(firePosition.transform.position, camera.transform.rotation);

            Instantiate(bullet, firePosition.transform.position, camera.transform.rotation);
        }
    }

    public void ChangeStateIdle() // state를 변경하는 함수 // 한번만 사용하면 된다.
    {
        m_state = 0;

        InputKey();

        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 0f);
    }

    public void ChangeStateWalk()
    {
        m_state = 1;

        InputKey();

        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 0.5f);
    }

    public void ChangeStateRun()
    {
        m_state = 2;

        InputKey();

        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 1f);
    }

    public void Attack()
    {
        ChangeStateAiming();
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

    public void ChangeStateAiming()
    {
        m_state = 3;

        InputKey();

        animator.SetBool("Squat", false);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Aiming", true);
    }

    public void Sitting()
    {
        animator.SetBool("Squat", !animator.GetBool("Squat"));
        animator.SetBool("Aiming", false);
    }

    private void MouseMove()
    {
        if(Time.time >= nextTime)
        {
            udp.MouseMove(player.GetSocket(), this.transform.eulerAngles.y);
            nextTime = Time.time + rateTime;
        }
    }

    private void InputKey()
    {
        udp.InputKey(player.GetSocket(), m_state, this.transform.position, this.transform.eulerAngles.y);
    }

    private void FireBullet(Vector3 _position, Quaternion _rotate)
    {
        udp.FireBullet(player.GetSocket(), _position, _rotate);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5f, 5f, 20f, 20f), this.transform.eulerAngles.y.ToString(), style);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
