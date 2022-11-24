using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeerActions : MonoBehaviour
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

	private Animator animator;
	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;
	public int action;
	public int nextAction;
	public GameObject peer;
	CharacterController controller;

	private int peerNum;
	private CPlayer player;

	private int m_state;

	private float DeathTime;
	void Awake()
    {
        animator = GetComponent<Animator>();
		controller = this.GetComponent<CharacterController>();
		m_state = (int)eState.IDLE;
		action = (int)eState.IDLE;
		DeathTime = 0f;
	}

	

	public void Attack()
	{
		Aiming();
		animator.SetTrigger("Attack");
	}

	public void Death()
	{
		if(action == 0)
        {
			animator.Play("Idle");
			ChangeStateIdle();
        }
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

		ChangeStateIdle();
	}

	public void Aiming()
	{
		peer.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, player.GetRotation(), 0f), 20f * Time.deltaTime);

		if(action == 0)
        {
			ChangeStateIdle();
        }
		if(action == 4)
        {
			Damage();
		}
	}

	public void Sitting()
	{
		animator.SetBool("Squat", !animator.GetBool("Squat"));
		animator.SetBool("Aiming", false);
	}

	public void Idle()
	{
		peer.transform.position = Vector3.MoveTowards(peer.transform.position, player.GetPosition(), Time.deltaTime);
		peer.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, player.GetRotation(), 0f), 20f * Time.deltaTime);

		if(action == 1)
        {
			ChangeStateWalk();
		}
		if(action == 2)
        {
			ChangeStateRun();
        }
		if(action == 3)
        {
			ChangeStateAiming();
        }
		if (action == 4)
		{
			Damage();
		}
		if(action == 5)
        {
			ChangeStateDeath();
        }
	}
	public void Walk() 
	{
		peer.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, player.GetRotation(), 0f), 20f * Time.deltaTime);
		peer.transform.Translate(Vector3.forward * 1f * Time.deltaTime);

		if(action == 0)
        {
			ChangeStateIdle();
        }
		if(action == 2)
        {
			ChangeStateRun();
        }
		if (action == 4)
		{
			Damage();
		}
	}

	public void Run()
	{
		peer.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, player.GetRotation(), 0f), 20f * Time.deltaTime);
		peer.transform.Translate(Vector3.forward * 3f * Time.deltaTime);

		if(action == 0)
        {
			ChangeStateIdle();
        }
		if(action == 1)
        {
			ChangeStateWalk();
        }
		if (action == 4)
		{
			Damage();
		}
	}

	public void ChangeStateIdle() 
	{
		m_state = 0;

		animator.SetBool("Aiming", false);
		animator.SetFloat("Speed", 0f);
	}
	public void ChangeStateWalk()
	{
		m_state = 1;

		animator.SetBool("Aiming", false);
		animator.SetFloat("Speed", 0.5f);
	}
	public void ChangeStateRun()
	{
		m_state = 2;

		animator.SetBool("Aiming", false);
		animator.SetFloat("Speed", 1.0f);
	}

	public void ChangeStateAiming()
    {
		m_state = 3;

		animator.SetBool("Squat", false);
		animator.SetFloat("Speed", 0f);
		animator.SetBool("Aiming", true);
	}

	public void ChangeStateDeath()
    {
		m_state = 5;

		animator.Play("Death");
	}

	void Update() // 2022-11-16 여기도 수정
    {
		player = CGameManager.Instance.GetPlayer(peerNum); // 2022-11-22 state로 작업을 하자
		action = player.GetAction();

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
                break;
			case (int)eState.DAETH:
				Death();
				break;
        }
    }

	public void SetPlayer(int _peerNum) { peerNum = _peerNum; }
    public void SetAction(int _action) { action = _action; }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "bullet")
		{
			App app = Transform.FindObjectOfType<App>();
			CUdp udp = app.GetUdp();
			Damage();
			Destroy(collision.gameObject);
			if (app.GetPlayer().GetBoss() == 0)
            {
				int hp = player.GetHp();
				hp -= 34;
				if (hp > 0)
				{
					player.SetHp(hp);
				}
				else
				{
					player.SetHp(0);
					hp = 0;
				}
				udp.PeerHit(player.GetSocket(), hp);
			}
		}
	}
}
