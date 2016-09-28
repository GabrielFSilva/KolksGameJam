using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour 
{
	public event Action OnPlayerMovementEnd;

	public GameSceneManager	gameSceneManager;
	public SpriteRenderer playerSprite;
	public PlayerSpriteAnimator animator;
	public Rigidbody2D	rigidBody2D;

	public Orientation playerOrientation = Orientation.RIGHT;
	public KeyCode playerDirection = KeyCode.Q;

	public bool yawned = false;
	private float _playerSpeed = 3.3f;
	private float _moveTimerThreshold = 0.25f;
	public float talkTimerCooldown;
	public float moveKeyPressedTimer = 0f;
	//public Vector2 gridPosition;
	public TupleInt gridPos;
	public Vector2 moveStartPosition;
	public Vector2 moveEndPosition;
	public bool isMoving = false;
	public float moveTweenCount = 0f;
	public bool isTalking = false;
	public float talkTweenCount = 0f;

	void Start () 
	{
		animator.SetPlayerOrientation ((int)playerOrientation);
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
			
		if (!Input.GetKey (playerDirection)) 
		{
			moveKeyPressedTimer = 0f;
			if (Input.GetKey (KeyCode.W)) 
			{
				playerDirection = KeyCode.W;
				playerOrientation = Orientation.UP;
			} 
			else if (Input.GetKey (KeyCode.UpArrow)) 
			{
				playerDirection = KeyCode.UpArrow;
				playerOrientation = Orientation.UP;
			} 
			else if (Input.GetKey (KeyCode.A)) 
			{
				playerDirection = KeyCode.A;
				playerOrientation = Orientation.LEFT;
			} 
			else if (Input.GetKey (KeyCode.LeftArrow)) 
			{
				playerDirection = KeyCode.LeftArrow;
				playerOrientation = Orientation.LEFT;
			}
			else if (Input.GetKey (KeyCode.S)) 
			{
				playerDirection = KeyCode.S;
				playerOrientation = Orientation.DOWN;
			} 
			else if (Input.GetKey (KeyCode.DownArrow)) 
			{
				playerDirection = KeyCode.DownArrow;
				playerOrientation = Orientation.DOWN;
			}
			else if (Input.GetKey (KeyCode.D)) 
			{
				playerDirection = KeyCode.D;
				playerOrientation = Orientation.RIGHT;
			} 
			else if (Input.GetKey (KeyCode.RightArrow)) 
			{
				playerDirection = KeyCode.RightArrow;
				playerOrientation = Orientation.RIGHT;
			} 
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				gameSceneManager.HelloButtonClicked (false);
				return;
			} 
			else if (Input.GetKeyDown(KeyCode.E))
			{
				gameSceneManager.ExcuseMeButtonClicked (false);
				return;
			} 
			else if (Input.GetKey (KeyCode.Space))
			{
				gameSceneManager.YawnButtonClicked (false);
				return;
			}
			UpdateSpriteOriantation ();
		} 
		else 
		{
			if (!animator.yawning)
				moveKeyPressedTimer += Time.deltaTime;
			if (moveKeyPressedTimer >= _moveTimerThreshold) 
				SetPlayerDestination ();
		}
	}
	public void StartAction()
	{
		isTalking = true;
		talkTweenCount = 0f;
	}
	public void ChangeOrientation(Orientation p_oritentation)
	{
		if (yawned || isMoving || isTalking) 
			return;
		playerOrientation = p_oritentation;
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
			SoundManager.GetInstance ().PlayMovimentSFX ();
		}
	}

	private void UpdateSpriteOriantation()
	{
		animator.SetPlayerOrientation ((int)playerOrientation);
	}
}
