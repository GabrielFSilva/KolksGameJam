using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameSceneManager : MonoBehaviour 
{
	public enum ActionsAvailable
	{
		YAWN,
		YAWN_HELLO,
		YAWN_HELLO_EXCUSE,
	}
	//Testing stuff
	public bool				isOnTestMode;
	public string 			levelToTest;

	//Level Control
	public static int 		currentLevelIndex = 1;
	public int 				star3Value;
	public int 				star2Value;

	//Entities
	public int 				movesAvailable;
	public int 				playerMovimentCount;
	public int 				enemyYawnCount;
	public Player 			player;
	public List<Enemy> 		enemies;

	//Control Scripts
	public LevelLoader		levelLoader;
	public GridManager 		gridManager;
	public EntitiesManager	entitiesManager;
	public InputManager 	inputManager;
	public UIManager 		uiManager;

	public Text	yawnedCountLabel;
	public Text	currentLevelLabel;

	public SoundManager soundManager;
	public ActionsAvailable	actions;

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
			p_player.gameSceneManager = this;
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
		inputManager.onScreenClicked += InputManager_onScreenClicked;

		soundManager = SoundManager.GetInstance ();
		soundManager.PlayBGM ();
	}

	void InputManager_onScreenClicked (Tile.PlayerOrientation p_orientation)
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
		if (currentLevelIndex == 14)
			yield return new WaitForSeconds (6f);
		else
			yield return new WaitForSeconds (4.5f);
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
		if (__t < 0.5f)
			soundManager.PlayDefeatSFX ();
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
		PlayerYawnAction (player.gridPosition,player.playerOrientation,true);
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
		
		PlayerHelloAction (player.gridPosition, player.playerOrientation);
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
		
		PlayerExcuseMeAction (player.gridPosition,player.playerOrientation);
		player.StartAction ();
	}

	public Enemy TryToHitEnemy(Vector2 p_position, Tile.PlayerOrientation p_orientation, bool p_continueUntilEnd)
	{
		int __posX = Mathf.RoundToInt (p_position.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (p_position.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));

		while(__posX > -1)
		{
			if (!gridManager.TileIsWithinGrid(__posX,__posY))
				return null;
			if (!gridManager.TilePassYawn(__posX, __posY))
				return null;
			if (gridManager.TileHasPlayer(__posX, __posY))
				return null;
			
			foreach (Enemy __enemy in enemies)
				if (Mathf.RoundToInt (__enemy.gridPosition.x) == __posX && Mathf.RoundToInt (__enemy.gridPosition.y) == __posY) 
					return __enemy;

			__posX += Mathf.RoundToInt (Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
			__posY -= Mathf.RoundToInt (Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));
			if (!p_continueUntilEnd)
				return null;

		}
		return null;
	}
	public void PlayerYawnAction(Vector2 p_position,Tile.PlayerOrientation p_orientation, bool p_calledByPlayer)
	{
		if (p_calledByPlayer) 
			foreach (Enemy __enemy in enemies)
				__enemy.StopYawnChain ();
		Enemy __enemyHit;
		for (int i = 0; i < 4; i++) 
		{
			__enemyHit = TryToHitEnemy (p_position,(Tile.PlayerOrientation)i, true);
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
	public void PlayerHelloAction(Vector2 p_position, Tile.PlayerOrientation p_orientation)
	{
		if (playerMovimentCount >= movesAvailable)
			return;
		Enemy __enemyHit = TryToHitEnemy(p_position, p_orientation, true);
		if (__enemyHit == null) 
		{
			soundManager.PlayErrorSFX ();
			return;
		}
		__enemyHit.ChangeEnemyOrientation (p_orientation);
		playerMovimentCount++;
		soundManager.PlayHelloSFX ();
	}
	public void PlayerExcuseMeAction(Vector2 p_position, Tile.PlayerOrientation p_orientation)
	{
		if (playerMovimentCount >= movesAvailable)
			return;
		Enemy __enemyHit = TryToHitEnemy(p_position, p_orientation, false);
		if (__enemyHit == null) 
		{
			soundManager.PlayErrorSFX ();
			return;
		}
		int __posX = Mathf.RoundToInt (__enemyHit.gridPosition.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (__enemyHit.gridPosition.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));
		if (!gridManager.TileIsWithinGrid (__posX, __posY)) 
		{
			soundManager.PlayErrorSFX ();
			return;
		}
		if (gridManager.TileHasEnemy (__posX, __posY)) 
		{
			soundManager.PlayErrorSFX ();
			return;
		}

		if (gridManager.TileWalkable (__posX,__posY))
		{
			__enemyHit.SetEnemyDestination (p_orientation);
			playerMovimentCount++;
			soundManager.PlayExcuseMeSFX ();
		}
		
	}
	public bool GetPathCollision(Vector2 p_position, Tile.PlayerOrientation p_orientation)
	{
		int __posX = Mathf.RoundToInt (p_position.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (p_position.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));

		//Moviment Cap Reached
		if (playerMovimentCount >= movesAvailable)
			return false;
		//Map Limit Block
		if (!gridManager.TileIsWithinGrid(__posX,__posY))
			return false;
		//Tile Block
		if (!gridManager.TileWalkable(__posX, __posY))
			return false;
		//EnemyBlock
		foreach (Enemy __enemy in enemies)
			if (Mathf.RoundToInt (__enemy.gridPosition.x) == __posX && Mathf.RoundToInt (__enemy.gridPosition.y) == __posY)
				return false;
		return true;
	}
}
