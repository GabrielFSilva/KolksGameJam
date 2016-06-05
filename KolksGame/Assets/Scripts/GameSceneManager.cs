using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameSceneManager : MonoBehaviour 
{
	//temp UI
	public Text	movimentCountLabel;
	public Text	yawnedCountLabel;

	public List<GameObject> levels;
	public LevelInfo testLevel;
	public LevelInfo currentLevel;
	public static int currentLevelIndex = 0;

	public PlayerManager player;
	public int playerMovimentCount;
	public List<Enemy> enemies;

	//Grid Stuff
	public Transform tilesContainer;
	public GameObject tilePrefab;
	public List<GridRow> grid;
	public List<Sprite> tileFloorSprites;
	public List<Sprite> tileContentSprites;
	public int gridWidth;
	public int gridHeight;



	void Start () 
	{
		LoadLevel ();

		gridHeight = grid.Count;
		gridWidth = grid [0].columns.Count;
		playerMovimentCount = 0;
		player.gameSceneManager = this;

		foreach (Enemy __enemy in enemies) 
		{
			__enemy.transform.localPosition = new Vector3 ((__enemy.gridPosition.x * 2f) - gridWidth + 1f,
				(__enemy.gridPosition.y * -2f) + grid.Count - 1f);
			__enemy.gameSceneManager = this;
		}
		player.transform.localPosition = new Vector3 ((player.gridPosition.x * 2f) - gridWidth + 1f,
			(player.gridPosition.y * -2f) + grid.Count - 1f);
	}
	void Update()
	{
		movimentCountLabel.text = "Mov: " + playerMovimentCount.ToString () + "/" + currentLevel.movesAvailable.ToString();
		int __count = 0;
		foreach (Enemy __enemy in enemies)
			if (__enemy.yawned)
				__count++;
		yawnedCountLabel.text = __count.ToString () + "/" + enemies.Count.ToString();
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	private void LoadLevel()
	{
		if (testLevel != null)
			currentLevel = testLevel;
		else 
		{
			currentLevel = ((GameObject)Instantiate (levels [currentLevelIndex])).GetComponent<LevelInfo> ();
			currentLevel.transform.parent = transform;
			currentLevel.transform.localPosition = Vector3.zero;
		}
		currentLevel.name = "Level";
		player = currentLevel.player;
		grid = new List<GridRow> ();
		enemies = new List<Enemy> ();
		foreach (Transform __child in currentLevel.grid.transform)
			grid.Add (__child.GetComponent<GridRow>());
		foreach (Transform __child in currentLevel.enemiesContainer.transform)
			enemies.Add (__child.GetComponent<Enemy>());
		LoadTiles ();
	}
	private void LoadTiles()
	{
		GameObject __tileFloor;
		GameObject __tileContent;
		foreach (GridRow __row in grid) 
		{
			for (int i = 0; i < __row.columns.Count; i++) 
			{
				//Load Floor
				__tileFloor = (GameObject)Instantiate (tilePrefab);
				__tileFloor.name = "TileFloor";
				__tileFloor.transform.parent = tilesContainer;
				__tileFloor.transform.localPosition = new Vector3 ((i * 2f) - __row.columns.Count + 1f,
					(grid.IndexOf (__row) * -2f) + grid.Count - 1f);
				SpriteRenderer __sr = __tileFloor.GetComponent<SpriteRenderer> ();
				if ((i + grid.IndexOf (__row)) % 2 == 0)
					__sr.sprite = tileFloorSprites [0];
				else
					__sr.sprite = tileFloorSprites [1];

				//Load File Content
				if (__row.columns [i].content != Tile.TileContent.NOTHING) 
				{
					__tileContent = (GameObject)Instantiate (tilePrefab);
					__tileContent.name = __row.columns [i].content.ToString();
					__tileContent.transform.parent = __tileFloor.transform;
					__tileContent.transform.localPosition = Vector3.zero;
					__tileContent.GetComponent<SpriteRenderer> ().sprite = tileContentSprites [(int)__row.columns [i].content - 1];
					__tileContent.GetComponent<SpriteRenderer> ().sortingOrder = 1;
					__tileContent.transform.localRotation = Quaternion.Euler (
						new Vector3 (0f, 0f, (int)__row.columns [i].orientation * 90f));
				}
			}
		}
	}
	public Enemy TryToHitEnemy(Vector2 p_position, Tile.PlayerOrientation p_orientation, bool p_continueUntilEnd)
	{
		int __posX = Mathf.RoundToInt (p_position.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (p_position.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));

		while(__posX > -1)
		{
			if (!TileIsWithinGrid(__posX,__posY))
				return null;
			if (!TilePassYawn (__posX, __posY))
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
	public void PlayerYawnAction(Vector2 p_position)
	{
		Enemy __enemyHit;
		for (int i = 0; i < 4; i++) 
		{
			__enemyHit = TryToHitEnemy (p_position,(Tile.PlayerOrientation)i, true);
			if (__enemyHit == null) 
				continue;
			int __orientation = (int)__enemyHit.enemyOrientation + 2;
			if (__orientation >= 4)
				__orientation -= 4;
			if (__orientation == i)
				__enemyHit.StartYawn ();
		}
	}
	public void PlayerHelloAction(Vector2 p_position, Tile.PlayerOrientation p_orientation)
	{
		if (playerMovimentCount >= currentLevel.movesAvailable)
			return;
		Enemy __enemyHit = TryToHitEnemy(p_position, p_orientation, true);
		if (__enemyHit == null)
			return;
		__enemyHit.ChangeEnemyOrientation (p_orientation);
		playerMovimentCount++;
	}
	public void PlayerExcuseMeAction(Vector2 p_position, Tile.PlayerOrientation p_orientation)
	{
		if (playerMovimentCount >= currentLevel.movesAvailable)
			return;
		Enemy __enemyHit = TryToHitEnemy(p_position, p_orientation, false);
		if (__enemyHit == null)
			return;

		int __posX = Mathf.RoundToInt (__enemyHit.gridPosition.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (__enemyHit.gridPosition.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));
		if (!TileIsWithinGrid(__posX,__posY))
			return;
		if (TileWalkable (__posX,__posY))
		{
			__enemyHit.SetEnemyDestination (p_orientation);
			playerMovimentCount++;
		}
		
	}
	public bool GetPathCollision(Vector2 p_position, Tile.PlayerOrientation p_orientation)
	{
		int __posX = Mathf.RoundToInt (p_position.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (p_position.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));

		//Moviment Cap Reached
		if (playerMovimentCount >= currentLevel.movesAvailable)
			return false;
		//Map Limit Block
		if (!TileIsWithinGrid(__posX,__posY))
			return false;
		//Tile Block
		if (!TileWalkable(__posX, __posY))
			return false;
		//EnemyBlock
		foreach (Enemy __enemy in enemies)
			if (Mathf.RoundToInt (__enemy.gridPosition.x) == __posX && Mathf.RoundToInt (__enemy.gridPosition.y) == __posY)
				return false;
		return true;
			
	}
	private bool TileIsWithinGrid(int p_posX, int p_posY)
	{
		if (p_posX < 0 || p_posX > gridWidth - 1 || p_posY < 0 || p_posY > gridHeight - 1)
			return false;
		return true;
	}
	private bool TilePassYawn(int p_posX, int p_posY)
	{
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.NOT_WALKABLE_BLOCK_YAWN)
			return false;
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.WALKABLE_BLOCK_YAWN)
			return false;
		return true;
	}
	private bool TileWalkable(int p_posX, int p_posY)
	{
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.NOT_WALKABLE_BLOCK_YAWN)
			return false;
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.NOT_WALKABLE_PASS_YAWN)
			return false;
		return true;
	}
}
