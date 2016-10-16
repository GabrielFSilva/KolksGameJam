using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public GameSceneManager	gameSceneManager;
	public bool 			yawned;
    public bool             yawning;
    //Graphics
    public SpriteRenderer 	    enemySprite;
	public Animator 		    animator;
    public EnemyMarksController marksController;

	//Grid Info
	public TupleInt 		gridPos;
	public Orientation 		enemyOrientation;

	//Enemy Timers and Speeds
	protected float 	_enemyTalkDelay = 0.35f;
	protected float 	_enemyMoveDelay = 0.4f;
	protected float 	_enemyYawnDelay = 1.7f;
	protected float 	_enemySpeed = 2.8f;

	//Movement
	protected Orientation 	_moveOrientation;
	protected Vector2 		_moveStartPos;
	protected Vector2 		_moveEndPos;
	protected bool 			_isMoving = false;
	protected bool 			_isGliding = false;
	protected float 		_moveTweenCount = 0f;

	public EnemyType enemyType;

	public ActionEffectType helloEffect;
	public ActionEffectType excuseMeEffect;

	public void EnemySetUp () 
	{
		animator.SetInteger ("Orientation",(int)enemyOrientation);
		yawned = false;
        yawning = false;
        _isMoving = false;
		_isGliding = false;
		UpdateSortingOrder ();
	}
	void Update()
	{
		if (_isMoving) 
		{
			UpdateEnemyPosition ();
			UpdateSortingOrder ();
			return;
		}
	}
	private void UpdateSortingOrder()
	{
		enemySprite.sortingOrder = Mathf.RoundToInt (transform.localPosition.y * -10f) - 2;
	}
	private void UpdateEnemyPosition()
	{
		_moveTweenCount += Time.deltaTime * _enemySpeed;
		transform.localPosition = Vector3.Lerp (_moveStartPos, _moveEndPos, _moveTweenCount);
		if (_moveTweenCount >= 1f) 
		{
			if (enemyType == EnemyType.POLITE) 
			{
				_isGliding = true;
				TupleInt __targetPos = TupleInt.AddTuples (gridPos, _moveOrientation);

				if (gameSceneManager.CanWalkToTile (__targetPos))
					SetEnemyDestination (_moveOrientation);
				else 
				{
					_isMoving = false;
					_isGliding = false;
				}
			}
			else
				_isMoving = false;
		}
	}
	public void SetEnemyDestination(Orientation p_orientation)
	{
		gridPos.AddOrientation(p_orientation);
		_isMoving = true;
		_moveOrientation = p_orientation;
		_moveTweenCount = _enemyMoveDelay * -1f;
		if (_isGliding)
			_moveTweenCount = 0f;
		_moveStartPos = transform.localPosition;
		_moveEndPos = transform.localPosition + new Vector3 (Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad),
			Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad)) * 2f;

		if (_isGliding)
			StartCoroutine (MovimentSFXDelay (0f));
		else
			StartCoroutine (MovimentSFXDelay (_enemyYawnDelay/_enemySpeed));
	}
	public void StartYawn()
	{
		if (yawned) return;
		StartCoroutine (Yawn ());
	}
	IEnumerator MovimentSFXDelay(float p_delay)
	{
		if (p_delay > 0f)
			yield return new WaitForSeconds (p_delay);
		SoundManager.GetInstance ().PlayMovimentSFX ();
	}
	IEnumerator Yawn()
	{
        yawning = true;
		yawned = true;
		yield return new WaitForSeconds (_enemyYawnDelay);

		animator.SetBool ("Yawning",true);
		EnableIcons (true);
		gameSceneManager.PlayerYawnAction (gridPos,enemyOrientation,gameObject);
		yield return new WaitForSeconds (0.3f);
		SoundManager.GetInstance ().PlayEnemyYawnSFX ();
        yawning = false;
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
	public void ChangeOrientationInstantly(Orientation p_orientation)
	{
		enemyOrientation = p_orientation;
		animator.SetInteger ("Orientation", (int)p_orientation);
	}
	protected void ChangeEnemyOrientation(Orientation p_playerOrientation)
	{
		StartCoroutine(ChangeOrientation (p_playerOrientation));
	}
	public void HitByHelloAction(Orientation p_playerOrientation)
	{
		if (helloEffect == ActionEffectType.STANDARD)
			ChangeEnemyOrientation (p_playerOrientation);
        else if (helloEffect == ActionEffectType.NONE && enemyType == EnemyType.COCKY)
            marksController.PlayActionDenied();
    }
	public void HitByExcuseMeAction(Orientation p_playerOrientation)
	{
        if (excuseMeEffect != ActionEffectType.NONE)
            SetEnemyDestination(p_playerOrientation);
        else if (excuseMeEffect == ActionEffectType.NONE && enemyType == EnemyType.COCKY)
            marksController.PlayActionDenied();
	}
	public void SawPlayer(Orientation p_playerOrientation)
	{
		if (enemyType == EnemyType.SHY)
			ChangeOrientationInstantly (p_playerOrientation);
	}
	IEnumerator ChangeOrientation(Orientation p_playerOrientation)
	{
		yield return new WaitForSeconds (_enemyTalkDelay);
		int __orientation = (int)p_playerOrientation + 2;
		if (__orientation >= 4)
			__orientation -= 4;

		enemyOrientation = (Orientation)__orientation;
		animator.SetInteger ("Orientation", __orientation);
		SoundManager.GetInstance ().PlayMovimentSFX ();
	}
	//Enables the Yawn icons within the enemy
	private void EnableIcons(bool p_yawned, float p_delay = 0.8f)
	{
        marksController.ShowYawnIcon(p_yawned, p_delay, enemySprite.sortingOrder);
	}
	//Called by EnemiesManager at EndOfLevel
	public void ShowFailedIcon()
	{
		if (!yawned)
			EnableIcons (false, 0.25f);
	}
}
