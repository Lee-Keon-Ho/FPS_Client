using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeerActions : MonoBehaviour
{
    private Animator animator;
	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;
	public int action;
	public GameObject peer;
	CharacterController controller;

	private int peerNum;
	private CPlayer player;

	void Awake()
    {
        animator = GetComponent<Animator>();
		controller = this.GetComponent<CharacterController>();
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

	void Update()
    {
		player = CGameManager.Instance.GetPlayer(peerNum);
		action = player.GetAction();
		switch (action)
        {
            case 0:
				this.transform.position = player.GetPosition();
				this.transform.rotation = Quaternion.Euler(0f, player.GetRotation(), 0f);
				Stay();
				break;
            case 1:
				this.transform.position = player.GetPosition();
				this.transform.rotation = Quaternion.Euler(0f, player.GetRotation(), 0f);
				Walk();
				break;
			case 2:
				Run();
				break;
		}
    }

    private void LateUpdate()
    {
        if(action == 0)
        {
			
		}
		if(action == 1)
        {
			transform.Translate(Vector3.right * 2 * Time.deltaTime);
		}
    }
	public void SetPlayer(int _peerNum) { peerNum = _peerNum; }
    public void SetAction(int _action) { action = _action; }
}
