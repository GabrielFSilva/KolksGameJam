using UnityEngine;
using System.Collections;
using System;

public class AnimalSpriteController : MonoBehaviour 
{
	public Animator	animator;
	public bool yawning = false;
	private float yawnCooldownTimer = 1.25f;

	void Update()
	{
		if (!yawning)
			yawnCooldownTimer += Time.deltaTime;
	}

	public void SetOrientation(int p_ori)
	{
		if (animator.GetInteger ("Orientation") != p_ori)
			animator.SetInteger ("Orientation", p_ori);
	}
    public void SetOrientation(Orientation p_ori)
    {
        if (animator.GetInteger("Orientation") != (int)p_ori)
            animator.SetInteger("Orientation", (int)p_ori);
    }
    public void PlayMove()
    {
        animator.SetTrigger("Move");
    }
    public void PlayRotate()
    {
        animator.SetTrigger("Rotate");
    }
    public void StartYawn()
	{
		if (yawning || yawnCooldownTimer < 1.25f)
			return;
		yawning = true;
		animator.SetBool ("Yawning", true);
	}
	public void EndYawn()
	{
		yawning = false;
        yawnCooldownTimer = 0f;
		animator.SetBool ("Yawning", false);
	}
}
