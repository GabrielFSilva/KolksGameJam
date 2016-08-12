using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameSceneManager : MonoBehaviour 
{
	public static int 		currentLevelIndex = 1;
	public enum ActionsAvailable
	{
		YAWN,
		YAWN_HELLO,
		YAWN_HELLO_EXCUSE,
	}

	//Testing stuff
	[Header("Testing")]
	public bool				isOnTestMode;
	public string 			levelToTest;

	//Level Control
	[Header("LevelInfo")]
	public int 				star3Value;
	public int 				star2Value;
	public int 				movesAvailable;
	public int 				playerMovimentCount;
	public int 				enemyYawnCount;
	public ActionsAvailable	actions;

	//Control Scripts
	[Header("Managers")]
	public LevelLoader		levelLoader;
	public GridManager 		gridManager;
	public EntitiesManager	entitiesManager;
	public InputManager 	inputManager;
	public UIManager 		uiManager;
	public SoundManager 	soundManager;

	//Entities
	[Header("Entities")]
	public Player 			player;
	public List<Enemy> 		enemies;

	[Header("Extra")]
	public Text	yawnedCountLabel;
	public Text	currentLevelLabel;

	void Start () 
	{
		gridManager.gameManager = this;
		entitiesManager.gameManager = this;
		entitiesManager.OnEnemiesLoaded += delegate(List<Enemy> p_enemies) {
			enemies = p_enemies;
			enemies.ForEach(__enemy => __enemy.gameSceneManager = this);
		};
		entitiesManager.OnPlayerLoaded += delegate(Player p_player) {
			player = p_player;
			player.gameSceneManager = this;
			player.OnPlayerMovementEnd += Player_OnPlayerMovementEnd;
		};
		entitiesManager.coinsManager.OnUpdateCoinsCollected += delegate(int p_collectedCoins, int p_coinsOnStage) {
			uiManager.coinLabelManager.UpdateCoinLabel(p_collectedCoins, p_coinsOnStage);
		};
		if (currentLevelIndex == 14) 
		{
			Camera.main.transform.localPosition = new Vector3(7f,0f,-10f);
			Camera.main.orthographicSize = 9;
		}

	
		levelLoader.OnSetGridDimensions += gridManager.SetGridDimensions;
		levelLoader.OnSendLayerData += delegate(int p_layerID, List<int> p_data) {
			if (p_layerID == 0)
				gridManager.LoadTiles(p_data);
			else if (p_layerID == 1)
				gridManager.LoadScenery(p_data);
			else if (p_layerID == 2)
				entitiesManager.LoadEntities(p_data);
			else if (p_layerID == 3)
				entitiesManager.LoadIA(p_data);
		};
		levelLoader.OnSetEnergy += delegate(int p_energy) {
			movesAvailable = p_energy;
		};
		levelLoader.OnSetLevelActions += delegate(bool p_hello, bool p_excuseMe) {
			if (!p_hello)
				actions = ActionsAvailable.YAWN;
			else if (!p_excuseMe)
				actions = ActionsAvailable.YAWN_HELLO;
			else
				actions = ActionsAvailable.YAWN_HELLO_EXCUSE;
			uiManager.actionButtonsManager.EnableActionButtons (actions);
		};
		levelLoader.OnSetStarValues += delegate(int p_star3, int p_star2) {
			star3Value = p_star3;
			star2Value = p_star2;
		};
		levelLoader.LoadLevel (currentLevelIndex + 1, levelToTest, isOnTestMode);
		playerMovimentCount = 0;

		inputManager.player = player;
		inputManager.OnScreenClicked += InputManager_OnScreenClicked;

		soundManager = SoundManager.GetInstance ();
		soundManager.PlayBGM ();
	}
	void Player_OnPlayerMovementEnd ()
	{
		entitiesManager.coinsManager.CheckPlayerGotCoin (player.gridPos);
		Enemy __enemyHit;
		for (int i = 0; i < 4; i++) 
		{
			__enemyHit = TryToHitEnemy (player.gridPos,(Orientation)i, true);
			if (__enemyHit == null) 
				continue;
			
			if (Util.IsOpposeOrientation(__enemyHit.enemyOrientation, (Orientation)i))
				__enemyHit.SawPlayer (player.playerOrientation);

		}
	}
	void InputManager_OnScreenClicked (Orientation p_orientation)
	{
		if (p_orientation == player.playerOrientation)
			player.SetPlayerDestination ();
		else
			player.ChangeOrientation (p_orientation);
	}
	void Update()
	{
		enemyYawnCount = 0;
		foreach (Enemy __enemy in enemies)
			if (__enemy.yawned)
				enemyYawnCount++;
		yawnedCountLabel.text = enemyYawnCount.ToString () + "/" + enemies.Count.ToString();
		currentLevelLabel.text = "Level " + (currentLevelIndex+1).ToString() + " / 50";

		uiManager.energyBarManager.UpdateEnergyBar(movesAvailable,playerMovimentCount);
		if (playerMovimentCount - movesAvailable == 0) 
			uiManager.actionButtonsManager.SetYawnButtonGlow ();
	}

	IEnumerator EndLevel()
	{
		yield return new WaitForSeconds (3.5f);
		soundManager.PlayEndOfLevelSFX ();
		uiManager.endLevelPanelManager.EnableEndLevelPanel (true);

		float __t = -0.25f;
		float __limit = 0f;
		if (enemyYawnCount < enemies.Count)
			__limit = 0.25f;
		else 
		{
			PlayerPrefsManager.SetLevelStars (currentLevelIndex, 0);

			if (movesAvailable - playerMovimentCount >= star3Value) 
			{
				PlayerPrefsManager.SetLevelStars (currentLevelIndex, 3);
				__limit = 1f;
			}
			else if (movesAvailable - playerMovimentCount >= star2Value) 
			{
				PlayerPrefsManager.SetLevelStars (currentLevelIndex, 2);
				__limit = 0.75f;
			}
			else
			{
				PlayerPrefsManager.SetLevelStars (currentLevelIndex, 1);
				__limit = 0.5f;
			}
			if (currentLevelIndex == 0) 
			{
				PlayerPrefsManager.SetLevelStars (currentLevelIndex, 3);
				__limit = 1f;
			}
		}
		if (__limit >= 0.5f)
			PlayerPrefsManager.SetUnlockedLevel(currentLevelIndex + 2);
		while (__t < __limit) 
		{
			__t += Time.deltaTime * 0.6f;
			uiManager.endLevelPanelManager.UpdateStarBarPosition (__t, __limit);
			uiManager.endLevelPanelManager.UpdateStarSprites (__t);
			yield return null;
		}
		//Failed Stage
		if (__t < 0.5f)
			soundManager.PlayDefeatSFX ();
		//Completed Stage
		else
			entitiesManager.coinsManager.SaveCollectedCoins ();
		
		yield return new WaitForSeconds (0.25f);
		uiManager.endLevelPanelManager.EnableEndLevelButtons (__t);
	}

	public void NextButtonClicked()
	{
		soundManager.PlayClickSFX ();

		if (!isOnTestMode)
			currentLevelIndex++; 
		
		if (currentLevelIndex == 9)
			SceneManager.LoadScene("VictoryScreen");
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void YawnButtonClicked(bool p_isFromButton)
	{
		if (p_isFromButton && !player.yawned)
			soundManager.PlayClickSFX ();
		if (player.isTalking || player.isMoving || player.yawned)
			return;
		
		player.yawned = true;
		player.animator.StartYawn ();
		PlayerYawnAction (player.gridPos,player.playerOrientation,true);
		player.StartAction ();
		uiManager.actionButtonsManager.DisableYawnButtonTransition ();
		soundManager.PlayPlayerYawnSFX ();
		StartCoroutine (EndLevel());
	}
	public void HelloButtonClicked(bool p_isFromButton)
	{
		if (p_isFromButton)
			soundManager.PlayClickSFX ();
		if (player.isTalking || player.isMoving || player.yawned)
			return;
		if (actions < ActionsAvailable.YAWN_HELLO)
			return;
		
		PlayerHelloAction (player.gridPos, player.playerOrientation);
		player.StartAction ();
	}
	public void ExcuseMeButtonClicked(bool p_isFromButton)
	{
		if (p_isFromButton)
			soundManager.PlayClickSFX ();
		if (player.isTalking || player.isMoving || player.yawned)
			return;
		if (actions < ActionsAvailable.YAWN_HELLO_EXCUSE)
			return;
		
		PlayerExcuseMeAction (player.gridPos,player.playerOrientation);
		player.StartAction ();
	}

	public Enemy TryToHitEnemy(TupleInt p_position, Orientation p_orientation, bool p_continueUntilEnd)
	{
		TupleInt __pos = TupleInt.AddTuples (p_position, p_orientation);
		while(__pos.Item1 > -1)
		{
			if (!gridManager.TileIsWithinGrid(__pos))
				return null;
			if (!gridManager.TilePassYawn(__pos))
				return null;
			if (gridManager.TileHasPlayer(__pos))
				return null;
			
			foreach (Enemy __enemy in enemies)
				if (Mathf.RoundToInt (__enemy.gridPos.Item1) == __pos.Item1 && Mathf.RoundToInt (__enemy.gridPos.Item2) == __pos.Item2) 
					return __enemy;

			__pos.AddOrientation(p_orientation);
			if (!p_continueUntilEnd)
				return null;

		}
		return null;
	}
	public void PlayerYawnAction(TupleInt p_position,Orientation p_orientation, bool p_calledByPlayer)
	{
		if (p_calledByPlayer) 
			foreach (Enemy __enemy in enemies)
				__enemy.StopYawnChain ();
		Enemy __enemyHit;
		for (int i = 0; i < 4; i++) 
		{
			__enemyHit = TryToHitEnemy (p_position,(Orientation)i, true);
			if (__enemyHit == null)
				continue;

			int __orientation = i + 2;
			if (__orientation >= 4)
				__orientation -= 4;
			if (__orientation == (int)p_orientation) 
				continue;
				
			__orientation = (int)__enemyHit.enemyOrientation + 2;
			if (__orientation >= 4)
				__orientation -= 4;
			if (__orientation == i)
				__enemyHit.StartYawn ();
		}
	}
	public void PlayerHelloAction(TupleInt p_position, Orientation p_orientation)
	{
		if (playerMovimentCount >= movesAvailable)
			return;
		Enemy __enemyHit = TryToHitEnemy(p_position, p_orientation, true);
		if (__enemyHit == null) 
		{
			soundManager.PlayErrorSFX ();
			return;
		}
		__enemyHit.HitByHelloAction (p_orientation);
		playerMovimentCount++;
		soundManager.PlayHelloSFX ();
	}
	public void PlayerExcuseMeAction(TupleInt p_position, Orientation p_orientation)
	{
		if (playerMovimentCount >= movesAvailable)
			return;
		Enemy __enemyHit = TryToHitEnemy(p_position, p_orientation, false);
		if (__enemyHit == null) 
		{
			soundManager.PlayErrorSFX ();
			return;
		}
		TupleInt __pos = TupleInt.AddTuples(__enemyHit.gridPos, p_orientation);
		if (!CanWalkToTile(__pos))
		{
			soundManager.PlayErrorSFX ();
			return;
		}

		if (gridManager.TileWalkable (__pos))
		{
			__enemyHit.HitByExcuseMeAction (p_orientation);
			playerMovimentCount++;
			soundManager.PlayExcuseMeSFX ();
		}
		
	}
	public bool CanWalkToTile(TupleInt p_pos)
	{
		if (!gridManager.TileIsWithinGrid (p_pos))
			return false;
		if (gridManager.TileHasEnemy (p_pos)) 
			return false;
		if (!gridManager.TileWalkable (p_pos))
			return false;
		return true;
	}
	public bool GetPathCollision(TupleInt p_position, Orientation p_orientation)
	{
		TupleInt __pos = TupleInt.AddTuples (p_position, p_orientation);
		//Moviment Cap Reached
		if (playerMovimentCount >= movesAvailable)
			return false;
		//Map Limit Block
		if (!gridManager.TileIsWithinGrid(__pos))
			return false;
		//Tile Block
		if (!gridManager.TileWalkable(__pos))
			return false;
		//EnemyBlock
		foreach (Enemy __enemy in enemies)
			if (__enemy.gridPos.Equals(__pos))
				return false;
		return true;
	}
}
