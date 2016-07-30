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

	public Tile.PlayerOrientation playerOrientation = Tile.PlayerOrientation.RIGHT;
	public KeyCode playerDirection = KeyCode.Q;

	public bool yawned = false;
	private float _playerSpeed = 1.8f;
	private float _moveTimerThreshold = 0.25f;
	public float talkTimerCooldown;
	public float moveKeyPressedTimer = 0f;
	public Vector2 gridPosition;
	public Vector2 moveStartPosition;
	public Vector2 moveEndPosition;
	public bool isMoving = false;
	public float moveTweenCount = 0f;
	public bool isTalking = false;
	public float talkTweenCount = 0f;

	void Start () 
	{
		animator.SetPlayerOrientation ((int)playerOrientation);
		if (playerOrientation == Tile.PlayerOrientation.UP)
			playerDirection = KeyCode.W;
		else if (playerOrientation == Tile.PlayerOrientation.RIGHT)
			playerDirection = KeyCode.D;
		else if (playerOrientation == Tile.PlayerOrientation.DOWN)
			playerDirection = KeyCode.S;
		else
			playerDirection = KeyCode.A;
	}

	void Update () 
	{
		if (yawned)
			return;
		if (isMoving) 
		{
			UpdatePlayerPosition ();
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
				playerOrientation = Tile.PlayerOrientation.UP;
			} 
			else if (Input.GetKey (KeyCode.UpArrow)) 
			{
				playerDirection = KeyCode.UpArrow;
				playerOrientation = Tile.PlayerOrientation.UP;
			} 
			 
			else if (Input.GetKey (KeyCode.A)) 
			{
				playerDirection = KeyCode.A;
				playerOrientation = Tile.PlayerOrientation.LEFT;
			} 
			else if (Input.GetKey (KeyCode.LeftArrow)) 
			{
				playerDirection = KeyCode.LeftArrow;
				playerOrientation = Tile.PlayerOrientation.LEFT;
			}
			else if (Input.GetKey (KeyCode.S)) 
			{
				playerDirection = KeyCode.S;
				playerOrientation = Tile.PlayerOrientation.DOWN;
			} 
			else if (Input.GetKey (KeyCode.DownArrow)) 
			{
				playerDirection = KeyCode.DownArrow;
				playerOrientation = Tile.PlayerOrientation.DOWN;
			}
			else if (Input.GetKey (KeyCode.D)) 
			{
				playerDirection = KeyCode.D;
				playerOrientation = Tile.PlayerOrientation.RIGHT;
			} 
			else if (Input.GetKey (KeyCode.RightArrow)) 
			{
				playerDirection = KeyCode.RightArrow;
				playerOrientation = Tile.PlayerOrientation.RIGHT;
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
	public void ChangeOrientation(Tile.PlayerOrientation p_oritentation)
	{
		if (yawned || isMoving || isTalking) 
			return;
		playerOrientation = p_oritentation;
		UpdateSpriteOriantation ();
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
		if (gameSceneManager.GetPathCollision(gridPosition, playerOrientation))
		{
			isMoving = true;
			moveTweenCount = 0f;
			gridPosition = gridPosition + new Vector2 (Mathf.Cos ((int)playerOrientation * 90f * Mathf.Deg2Rad),
				-1f * Mathf.Sin ((int)playerOrientation * 90f * Mathf.Deg2Rad));
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
