using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAnimator : MonoBehaviour
{
	[SerializeField] Animator anim;
	public void SetMoveSpeed(float speed)
	{
		anim.SetFloat("Speed", speed);
	}

	public void Attack()
	{
		anim.Play("Attack");
	}

	public void Eat()
	{
		anim.Play("Idle Reaction");
	}
}