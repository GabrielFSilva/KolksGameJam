using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	public GameSceneManager	gameSceneManager;
	public SpriteRenderer playerSprite;
	public Rigidbody2D	rigidBody2D;

	public Tile.PlayerOrientation playerOrientation = Tile.PlayerOrientation.RIGHT;
	public KeyCode playerDirection = KeyCode.Q;

	public float playerSpeed;
	public float moveTimerThreshold;
	public float moveKeyPressedTimer = 0f;
	public Vector2 gridPosition;
	public Vector2 moveStartPosition;
	public Vector2 moveEndPosition;
	public bool isMoving = false;
	public float moveTweenCount = 0f;

	void Start () 
	{
	
	}

	void Update () 
	{
		if (isMoving) 
		{
			UpdatePlayerPosition ();
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
			else if (Input.GetKey (KeyCode.A)) 
			{
				playerDirection = KeyCode.A;
				playerOrientation = Tile.PlayerOrientation.LEFT;
			}
			else if (Input.GetKey (KeyCode.S)) 
			{
				playerDirection = KeyCode.S;
				playerOrientation = Tile.PlayerOrientation.DOWN;
			}
			else if (Input.GetKey (KeyCode.D)) 
			{
				playerDirection = KeyCode.D;
				playerOrientation = Tile.PlayerOrientation.RIGHT;
			}
			UpdateSpriteOriantation ();
		} 
		else 
		{
			moveKeyPressedTimer += Time.deltaTime;
			if (moveKeyPressedTimer >= moveTimerThreshold) 
				SetPlayerDestination ();
		}
	}
	private void UpdatePlayerPosition()
	{
		moveTweenCount += Time.deltaTime * playerSpeed;
		transform.localPosition = Vector3.Lerp (moveStartPosition, moveEndPosition, moveTweenCount);
		if (moveTweenCount >= 1f) 
		{
			if (Input.GetKey (playerDirection))
				SetPlayerDestination ();
			else
				isMoving = false;
		}
	}
	private void SetPlayerDestination()
	{
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
		}
	}

	private void UpdateSpriteOriantation()
	{
		playerSprite.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, (int)playerOrientation * 90f));
	}
}
