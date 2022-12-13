using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum eState
    {
        IDLE,
        WALK,
        RUN,
        AIMING,
        DAMAGE,
        DAETH
    }
    CGameManager gm;
    Animator animator;
    Camera camera;

    public AudioClip clip;
    AudioSource audio;

    float rateTime;
    float nextTime;
    float gameOverTime;

    public float smoothness = 10f;

    private int m_state;
    
    private CPlayer player;
    CUdp udp;

    bool run;

    //Action
    const int countOfDamageAnimations = 3;
    int lastDamageAnimation = -1;

    public GameObject bullet;
    public GameObject firePosition;
    public GameObject character;

    private GUIStyle style;

    private float DeathTime;

    public Transform[] respawnPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        style = new GUIStyle();

        rateTime = 0.1f;
        nextTime = 0.0f;

        run = false;

        DeathTime = 0.0f;

        style.normal.textColor = Color.red;
        style.fontSize = 20;
    }

    void Start()
    {
        App app = Transform.FindObjectOfType<App>();
        player = app.GetPlayer();
        udp = app.GetUdp();
        gm = CGameManager.Instance;

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
        if (player.GetHp() == 0)
        {
            ChangeStateDeath();
        }

        switch (m_state)
        {
            case (int)eState.IDLE:
                Idle();
                break;
            case (int)eState.WALK:
                Walk();
                break;
            case (int)eState.RUN:
                Run();
                break;
            case (int)eState.AIMING:
                Aiming();
                break;
            case (int)eState.DAMAGE:
                Damage();
                break;
            case (int)eState.DAETH:
                Death();
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
            run = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (run) ChangeStateRun();
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
            audio.clip = clip;
            audio.Play();

            FireBullet(firePosition.transform.position, camera.transform.rotation);
            bullet.name = player.GetSocket().ToString();
            Instantiate(bullet, firePosition.transform.position, camera.transform.rotation);
        }
    }

    private void Damage()
    {
        m_state = 0;

        InputKey();
    }

    public void ChangeStateIdle() 
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

    public void ChangeStateDeath()
    {
        m_state = 5;

        InputKey();
        
        animator.Play("Death");
    }

    public void Death()
    {
        if (DeathTime >= 4.0f)
        {
            int num = Random.Range(0, 7);
            this.transform.position = respawnPosition[num].position;
            player.SetHp(100);
            animator.Play("Idle");
            ChangeStateIdle();
            DeathTime = 0.0f;
        }
        else
        {
            DeathTime += Time.deltaTime;
        }
    }

    public void ChangeStateDamage()
    {
        m_state = 4;

        InputKey();

        animator.SetBool("Aiming", false);
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
            udp.MouseMove(player.GetSocket(), this.transform.position, this.transform.eulerAngles.y);
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

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(5f, 5f, 20f, 20f), m_state.ToString(), style);
    //    GUI.Label(new Rect(5f, 5f + 20, 20f, 20f), player.GetHp().ToString(), style);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if(CGameManager.Instance.GetGameOver()) return;
        if (collision.gameObject.tag == "bullet" && m_state != (int)eState.DAETH)
        {
            string socket = collision.gameObject.name;
            ChangeStateDamage();
            Destroy(collision.gameObject);

            if (player.GetBoss() == 0)
            {
                App app = FindObjectOfType<App>();
                int kill = 0;
                int hp = app.GetPlayer().GetHp();
                hp -= 34;
                if (hp > 0)
                {
                    app.GetPlayer().SetHp(hp);
                }
                else
                {
                    int count = gm.GetPlayerCount();
                    app.GetPlayer().SetHp(0);
                    for(int i = 0; i < count; i++)
                    {
                        if(socket == gm.GetPlayer(i).GetSocket().ToString()+"(Clone)")
                        {
                            gm.GetPlayer(i).AddKill();
                            kill = gm.GetPlayer(i).GetKill();
                            break;
                        }
                    }
                    gm.GetPlayer(0).AddDeath();
                    
                    udp.Status();
                    if(kill >= 5)
                    {
                        gm.gameSocket = 0;
                        gm.GetPlayer(0).SetHp(100);
                        udp.GameOver();
                    }
                }
            }
        }
    }

    public int GetState() { return m_state; }
}
