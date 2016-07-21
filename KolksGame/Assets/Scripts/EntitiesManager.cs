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

		foreach (int __int in entitiesData) 
		{
			if (__int == 0)
				playerManager.LoadPlayer (entitiesData.IndexOf (__int));
			else if (__int == 1) 
				enemiesManager.LoadEnemy(EnemiesManager.EnemyTypes.STANDARD, entitiesData.IndexOf(__int));
		}
		OnPlayerLoaded (playerManager.player);
		OnEnemiesLoaded (enemiesManager.enemies);
	}
	public void LoadIA(List<int> p_data)
	{
		iaData = new List<int> ();
		foreach (int __int in p_data)
			iaData.Add (__int - 193);
		
		foreach (int __int in iaData) 
		{
			if (__int >= 0 && __int < 4) 
			{
				playerManager.TryChangePlayerOrientation (iaData.IndexOf (__int), (Tile.PlayerOrientation)__int);
				enemiesManager.TryChangeEnemiesOrientation (iaData.IndexOf (__int), (Tile.PlayerOrientation)__int);
			}
		}
	}
}
