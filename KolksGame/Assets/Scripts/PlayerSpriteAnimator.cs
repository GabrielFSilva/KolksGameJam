using UnityEngine;
using System.Collections;
using System;

public class PlayerSpriteAnimator : MonoBehaviour 
{
	public Animator	playerAnimator;
	public bool yawning = false;
	private float yawnCooldownTimer = 1.25f;

	void Update()
	{
		if (!yawning)
			yawnCooldownTimer += Time.deltaTime;
	}

	public void SetPlayerOrientation(int p_orientation)
	{
		if (playerAnimator.GetInteger ("Orientation") != p_orientation)
			playerAnimator.SetInteger ("Orientation", p_orientation);
	}
	public void StartYawn()
	{
		if (yawning || yawnCooldownTimer < 1.25f)
			return;
		yawning = true;
		playerAnimator.SetBool ("Yawning", true);
	}
	public void EndYawn()
	{
		yawning = false;yawnCooldownTimer = 0f;
		playerAnimator.SetBool ("Yawning", false);
	}
}
