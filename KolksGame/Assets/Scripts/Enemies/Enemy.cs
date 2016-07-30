using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public GameSceneManager	gameSceneManager;
	public SpriteRenderer 	enemySprite;
	public Animator 		animator;
	public bool 			yawned;

	//Grid Info
	public Vector2 					gridPosition;
	public Tile.PlayerOrientation 	enemyOrientation;

	//Enemy Timers and Speeds
	protected float 	_enemyTalkDelay = 0.35f;
	protected float 	_enemyMoveDelay = 0.4f;
	protected float 	_enemyYawnDelay = 0.6f;
	protected float 	_enemySpeed = 1.5f;

	//Movement
	protected Vector2 	_moveStartPos;
	protected Vector2 	_moveEndPos;
	protected bool 		_isMoving = false;
	protected float 	_moveTweenCount = 0f;

	//EnemyType
	public enum EnemyType
	{
		STANDARD,
		COCKY,
		SHY
	}
	public EnemyType enemyType;
	public enum ActionEffectType
	{
		NONE,
		STANDARD,
		GLIDE
	}
	public ActionEffectType helloEffect;
	public ActionEffectType excuseMeEffect;

	void Start () 
	{
		animator.SetInteger ("Orientation",(int)enemyOrientation);
		yawned = false;
	}
	void Update()
	{
		if (_isMoving) 
		{
			UpdateEnemyPosition ();
			return;
		}
	}
	private void UpdateEnemyPosition()
	{
		_moveTweenCount += Time.deltaTime * _enemySpeed;
		transform.localPosition = Vector3.Lerp (_moveStartPos, _moveEndPos, _moveTweenCount);
		if (_moveTweenCount >= 1f) 
			_isMoving = false;
	}
	public void SetEnemyDestination(Tile.PlayerOrientation p_playerOrientation)
	{
		gridPosition.x += Mathf.RoundToInt (Mathf.Cos ((int)p_playerOrientation * 90f * Mathf.Deg2Rad));
		gridPosition.y -= Mathf.RoundToInt (Mathf.Sin ((int)p_playerOrientation * 90f * Mathf.Deg2Rad));

		_isMoving = true;
		_moveTweenCount = _enemyMoveDelay * -1f;
		_moveStartPos = transform.localPosition;
		_moveEndPos = transform.localPosition + new Vector3 (Mathf.Cos ((int)p_playerOrientation * 90f * Mathf.Deg2Rad),
			Mathf.Sin ((int)p_playerOrientation * 90f * Mathf.Deg2Rad)) * 2f;
		
		StartCoroutine (MovimentSFXDelay ());
	}
	public void StartYawn()
	{
		if (yawned)
			return;
		StartCoroutine (Yawn ());
	}
	IEnumerator MovimentSFXDelay()
	{
		yield return new WaitForSeconds (_enemyYawnDelay/_enemySpeed);
		SoundManager.GetInstance ().PlayMovimentSFX ();
	}
	IEnumerator Yawn()
	{
		yawned = true;
		yield return new WaitForSeconds (_enemyYawnDelay);

		animator.SetBool ("Yawning",true);
		gameSceneManager.PlayerYawnAction (gridPosition,enemyOrientation,false);
		yield return new WaitForSeconds (0.3f);
		SoundManager.GetInstance ().PlayEnemyYawnSFX ();
	}
	public void StopYawnChain()
	{
		yawned = false;
		StopCoroutine (Yawn ());
	}
	public void EndYawn()
	{
		animator.SetBool ("Yawning", false);
	}
	public void ChangeOrientationInstantly(Tile.PlayerOrientation p_orientation)
	{
		enemyOrientation = p_orientation;
		animator.SetInteger ("Orientation", (int)p_orientation);
	}
	protected void ChangeEnemyOrientation(Tile.PlayerOrientation p_playerOrientation)
	{
		StartCoroutine(ChangeOrientation (p_playerOrientation));
	}
	public void HitByHelloAction(Tile.PlayerOrientation p_playerOrientation)
	{
		if (helloEffect == ActionEffectType.STANDARD)
			ChangeEnemyOrientation (p_playerOrientation);
	}
	public void HitByExcuseMeAction(Tile.PlayerOrientation p_playerOrientation)
	{
		if (excuseMeEffect == ActionEffectType.STANDARD)
			SetEnemyDestination (p_playerOrientation);
	}
	public void SawPlayer(Tile.PlayerOrientation p_playerOrientation)
	{
		if (enemyType == EnemyType.SHY)
			ChangeOrientationInstantly (p_playerOrientation);
	}
	IEnumerator ChangeOrientation(Tile.PlayerOrientation p_playerOrientation)
	{
		yield return new WaitForSeconds (_enemyTalkDelay);
		int __orientation = (int)p_playerOrientation + 2;
		if (__orientation >= 4)
			__orientation -= 4;

		enemyOrientation = (Tile.PlayerOrientation)__orientation;
		animator.SetInteger ("Orientation", __orientation);
		SoundManager.GetInstance ().PlayMovimentSFX ();
	}
}
