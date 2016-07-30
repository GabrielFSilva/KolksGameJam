using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EntitiesManager : MonoBehaviour 
{
	public event Action<Player>	OnPlayerLoaded;
	public event Action<List<Enemy>> OnEnemiesLoaded;

	public GameSceneManager 	gameManager;
	public PlayerManager		playerManager;
	public EnemiesManager		enemiesManager;
 
	public List<int> entitiesData;
	public List<int> iaData;

	public void LoadEntities(List<int> p_data)
	{
		entitiesData = new List<int> ();
		foreach (int __int in p_data)
			entitiesData.Add (__int - 129);

		for(int i = 0; i < entitiesData.Count; i++)
		{
			if (entitiesData[i] == 0)
				playerManager.LoadPlayer (i);
			else if (entitiesData[i] == 1) 
				enemiesManager.LoadEnemy(Enemy.EnemyType.STANDARD, i);
			else if (entitiesData[i] == 2) 
				enemiesManager.LoadEnemy(Enemy.EnemyType.COCKY, i);
			else if (entitiesData[i] == 3) 
				enemiesManager.LoadEnemy(Enemy.EnemyType.SHY, i);
		}
		OnPlayerLoaded (playerManager.player);
		OnEnemiesLoaded (enemiesManager.enemies);
	}
	public void LoadIA(List<int> p_data)
	{
		iaData = new List<int> ();
		foreach (int __int in p_data)
			iaData.Add (__int - 193);
		
		for(int i = 0; i < iaData.Count; i++)
		{
			if (iaData[i] >= 0 && iaData[i] < 4) 
			{
				playerManager.TryChangePlayerOrientation (i, (Tile.PlayerOrientation)iaData[i]);
				enemiesManager.TryChangeEnemiesOrientation (i, (Tile.PlayerOrientation)iaData[i]);
			}
		}
	}
}
