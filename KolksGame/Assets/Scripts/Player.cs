using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour 
{
	public event Action OnPlayerMovementEnd;

	public GameSceneManager	gameSceneManager;
	public SpriteRenderer playerSprite;
	public AnimalSpriteController animator;
	public Rigidbody2D	rigidBody2D;

	public Orientation playerOrientation = Orientation.RIGHT;
	public KeyCode playerDirection = KeyCode.Q;

	public bool yawned = false;
	private float _playerSpeed = 4.0f;
	public float talkTimerCooldown;

	public TupleInt gridPos;
	public Vector2 moveStartPosition;
	public Vector2 moveEndPosition;
	public bool isMoving = false;
	public float moveTweenCount = 0f;
	public bool isTalking = false;
	public float talkTweenCount = 0f;

	void Start () 
	{
		animator.SetOrientation ((int)playerOrientation);
		if (playerOrientation == Orientation.UP)
			playerDirection = KeyCode.W;
		else if (playerOrientation == Orientation.RIGHT)
			playerDirection = KeyCode.D;
		else if (playerOrientation == Orientation.DOWN)
			playerDirection = KeyCode.S;
		else
			playerDirection = KeyCode.A;

		UpdateSortingOrder ();
	}

	void Update () 
	{
        if (yawned)
			return;
		if (isMoving) 
		{
			UpdatePlayerPosition ();
			UpdateSortingOrder ();
			return;
		}
		if (isTalking) 
		{
			talkTweenCount += Time.deltaTime;
			if (talkTweenCount >= talkTimerCooldown)
				isTalking = false;
			return;
		}
	}
	public void StartAction()
	{
		isTalking = true;
		talkTweenCount = 0f;
	}
	public void ChangeOrientation(Orientation p_oritentation)
	{
		if (yawned || isMoving || isTalking || p_oritentation == playerOrientation) 
			return;
		playerOrientation = p_oritentation;
        animator.PlayRotate();
		UpdateSpriteOriantation ();
	}
	private void UpdateSortingOrder()
	{
		playerSprite.sortingOrder = Mathf.RoundToInt (transform.localPosition.y * -10f) - 1;
	}
	private void UpdatePlayerPosition()
	{
		moveTweenCount += Time.deltaTime * _playerSpeed;
		transform.localPosition = Vector3.Lerp (moveStartPosition, moveEndPosition, moveTweenCount);
		if (moveTweenCount >= 1f) 
		{
			if (OnPlayerMovementEnd != null)
				OnPlayerMovementEnd ();
			
			if (Input.GetKey (playerDirection))
				SetPlayerDestination ();
			else
				isMoving = false;
		}
	}
	public void SetPlayerDestination()
	{
		if (yawned || isMoving || isTalking) 
			return;
		if (gameSceneManager.GetPathCollision(gridPos, playerOrientation))
		{
			isMoving = true;
			moveTweenCount = 0f;
			gridPos.AddOrientation(playerOrientation);
			moveStartPosition = transform.localPosition;
			moveEndPosition = transform.localPosition + new Vector3 (Mathf.Cos ((int)playerOrientation * 90f * Mathf.Deg2Rad),
				Mathf.Sin ((int)playerOrientation * 90f * Mathf.Deg2Rad)) * 2f;
			
			gameSceneManager.playerMovimentCount++;
            animator.PlayRotate();
            SoundManager.GetInstance ().PlayMovimentSFX ();
		}
	}

	private void UpdateSpriteOriantation()
	{
		animator.SetOrientation ((int)playerOrientation);
	}
}
