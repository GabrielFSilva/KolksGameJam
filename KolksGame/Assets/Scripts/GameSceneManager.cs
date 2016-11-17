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
    public TutorialManager  tutorialManager;
	public SoundManager 	soundManager;

	//Entities
	[Header("Entities")]
	public Player 			player;
	public List<Enemy> 		enemies;

    void Awake()
    {
        if (isOnTestMode)
            currentLevelIndex = int.Parse(levelToTest.Remove(0, 6)) - 1;
    }
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
		entitiesManager.coinsManager.OnCollectCoin += delegate(Coin p_coin) {
			uiManager.coinLabelManager.CreateUICoin(p_coin);
		};
	
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
            uiManager.CheckActionButtonsAnimations();
		};
		levelLoader.OnSetStarValues += delegate(int p_star3, int p_star2) {
			star3Value = p_star3;
			star2Value = p_star2;
		};
        
        levelLoader.LoadLevel (currentLevelIndex + 1, levelToTest, isOnTestMode);
        
		inputManager.player = player;
		inputManager.OnSwipe += InputManager_OnSwipe;
        inputManager.OnTap += InputManager_OnTap;
        inputManager.OnKeyPress += InputManager_OnKeyPress;

        tutorialManager.SceneLoaded(currentLevelIndex + 1, entitiesManager.coinsManager.GetActiveCoins().Count);

		soundManager = SoundManager.GetInstance ();
		soundManager.PlayBGM ();

        playerMovimentCount = 0;
    }

    void Player_OnPlayerMovementEnd ()
	{
		entitiesManager.coinsManager.CheckPlayerGotCoin (player.gridPos);
        if (player.gridPos.Equals(new TupleInt(2, 3)))
            tutorialManager.PlayerMoveToTargetPosition(currentLevelIndex + 1);
		Enemy __enemyHit;
		for (int i = 0; i < 4; i++) 
		{
			__enemyHit = TryToHitEnemy (player.gameObject ,player.gridPos,(Orientation)i, true, false);
			if (__enemyHit == null) 
				continue;
			
			if (Util.IsOpposeOrientation(__enemyHit.enemyOrientation, (Orientation)i))
				__enemyHit.SawPlayer (player.playerOrientation);

		}
	}
	private void InputManager_OnSwipe (Orientation p_ori)
	{
        if (p_ori == Orientation.LEFT)
            tutorialManager.OnSwipeLeft();
        player.ChangeOrientation(p_ori);
        player.SetPlayerDestination ();
    }
    private void InputManager_OnTap(Orientation p_ori)
    {
        if (currentLevelIndex == 0)
            return;
        
        tutorialManager.OnTap(p_ori);
        player.ChangeOrientation(p_ori);
    }
    private void InputManager_OnKeyPress(Orientation p_ori)
    {
        if (player.playerOrientation == p_ori)
        {
            if (p_ori == Orientation.LEFT)
                tutorialManager.OnSwipeLeft();
            player.SetPlayerDestination();
        }
        else
        {
            tutorialManager.OnTap(p_ori);
            player.ChangeOrientation(p_ori);
        }

    }
    void Update()
	{
		enemyYawnCount = 0;
		foreach (Enemy __enemy in enemies)
			if (__enemy.yawned)
				enemyYawnCount++;
		uiManager.energyBarManager.UpdateEnergyBar(movesAvailable,playerMovimentCount);
		if (playerMovimentCount - movesAvailable == 0 && !player.yawned) 
			uiManager.actionButtonsManager.SetYawnButtonGlow ();
	}

	IEnumerator EndLevel()
	{
		yield return new WaitForSeconds (0.5f);
        while(entitiesManager.enemiesManager.HasYawningEnemy())
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        soundManager.PlayEndOfLevelSFX ();
        uiManager.endLevelPanelManager.EnableStars(PlayerPrefsManager.GetLevelStart(currentLevelIndex));
		uiManager.endLevelPanelManager.EnableEndLevelPanel (true);
        tutorialManager.LevelEnded(currentLevelIndex + 1, entitiesManager.coinsManager.GetActiveCoins().Count);
		entitiesManager.enemiesManager.ShowFailedEnemies ();
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
		
		if (currentLevelIndex == 40)
			SceneManager.LoadScene("LevelSelectScene");
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
		PlayerYawnAction (player.gridPos,player.playerOrientation,player.gameObject);
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

	public Enemy TryToHitEnemy(GameObject p_caller, TupleInt p_position, Orientation p_orientation, bool p_continueUntilEnd, bool p_createYawnLine)
	{
		TupleInt __pos = TupleInt.AddTuples (p_position, p_orientation);
		while(__pos.Item1 > -1)
		{
			if (!gridManager.TileIsWithinGrid(__pos))
				return null;
			if (!gridManager.TilePassYawn (__pos)) 
			{
                if(p_createYawnLine)
				uiManager.yawnLineFeedbackManager.CreateYawnLine (p_caller.transform.position,
					gridManager.GetTileTransform(__pos).position, p_orientation, false);
				return null;
			}
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
	public void PlayerYawnAction(TupleInt p_position,Orientation p_orientation, GameObject p_caller)
	{
		//Stop all yawns (none should be any active)
		if (p_caller.tag == "Player") 
			foreach (Enemy __enemy in enemies)
				__enemy.StopYawnChain ();

		//Check all four directions
		Enemy __enemyHit;
		for (int i = 0; i < 4; i++) 
		{
			//Try to hit the enemy
			__enemyHit = TryToHitEnemy (p_caller ,p_position,(Orientation)i, true,true);
			if (__enemyHit == null)
				continue;

			//If the hit is looking at the back of the yawner
			int __orientation = i + 2;
			if (__orientation >= 4)
				__orientation -= 4;
			if (__orientation == (int)p_orientation) 
				continue;
				

			__orientation = (int)__enemyHit.enemyOrientation + 2;
			if (__orientation >= 4)
				__orientation -= 4;
			if (__orientation == i) 
			{
				__enemyHit.StartYawn ();
				uiManager.yawnLineFeedbackManager.CreateYawnLine (p_caller.transform.position,
					__enemyHit.transform.position, __enemyHit.enemyOrientation, true);
			}
		}
	}
	public void PlayerHelloAction(TupleInt p_position, Orientation p_orientation)
	{
        if (playerMovimentCount >= movesAvailable)
        {
            uiManager.PlayBarShakeAnimation();
            soundManager.PlayErrorSFX();
            return;
        }
		Enemy __enemyHit = TryToHitEnemy(player.gameObject ,p_position, p_orientation, true, false);
		if (__enemyHit == null) 
		{
			soundManager.PlayErrorSFX ();
			return;
		}
        int __ori = (int)p_orientation + 2;
        if (__ori >= 4)
            __ori -= 4;

        if ((int)__enemyHit.enemyOrientation == __ori)
            return;
		__enemyHit.HitByHelloAction (p_orientation);
        if (__enemyHit.GetActionCostEnergy(ActionType.HELLO))
		    playerMovimentCount++;
		soundManager.PlayHelloSFX ();
	}
	public void PlayerExcuseMeAction(TupleInt p_position, Orientation p_orientation)
	{
        if (playerMovimentCount >= movesAvailable)
        {
            uiManager.PlayBarShakeAnimation();
            soundManager.PlayErrorSFX();
            return;
        }
		Enemy __enemyHit = TryToHitEnemy(player.gameObject, p_position, p_orientation, false, false);
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
            if (__enemyHit.GetActionCostEnergy(ActionType.EXCUSE_ME))
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
        if (playerMovimentCount >= movesAvailable)
        {
            uiManager.PlayBarShakeAnimation();
            soundManager.PlayErrorSFX();
            return false;
        }
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
