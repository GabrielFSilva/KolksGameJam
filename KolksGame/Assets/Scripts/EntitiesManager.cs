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
	public CoinsManager			coinsManager;
 
	public List<int> entitiesData;
	public List<int> iaData;

	public void LoadEntities(List<int> p_data)
	{
		entitiesData = new List<int> ();
		foreach (int __int in p_data)
			entitiesData.Add (__int - 129);

		for(int i = 0; i < entitiesData.Count; i++)
		{
			if (entitiesData [i] == 0)
				playerManager.LoadPlayer (i);
			else if (entitiesData [i] == 1)
				enemiesManager.LoadEnemy (EnemyType.STANDARD, i);
			else if (entitiesData [i] == 2)
				enemiesManager.LoadEnemy (EnemyType.COCKY, i);
			else if (entitiesData [i] == 3)
				enemiesManager.LoadEnemy (EnemyType.SHY, i);
			else if (entitiesData [i] == 4)
				enemiesManager.LoadEnemy (EnemyType.POLITE, i);
            else if (entitiesData[i] == 5)
                enemiesManager.LoadEnemy(EnemyType.NASTY, i);
            else if (entitiesData [i] == 8)
				coinsManager.LoadCoin (i);
			else if (entitiesData [i] == 9) 
			{
				enemiesManager.LoadEnemy (EnemyType.STANDARD, i);
				coinsManager.LoadCoin (i);
			}
			else if (entitiesData [i] == 10) 
			{
				enemiesManager.LoadEnemy (EnemyType.COCKY, i);
				coinsManager.LoadCoin (i);
			}
			else if (entitiesData [i] == 11) 
			{
				enemiesManager.LoadEnemy (EnemyType.SHY, i);
				coinsManager.LoadCoin (i);
			}
			else if (entitiesData [i] == 12) 
			{
				enemiesManager.LoadEnemy (EnemyType.POLITE, i);
				coinsManager.LoadCoin (i);
			}
            else if (entitiesData[i] == 13)
            {
                enemiesManager.LoadEnemy(EnemyType.NASTY, i);
                coinsManager.LoadCoin(i);
            }
        }
		OnPlayerLoaded (playerManager.player);
		OnEnemiesLoaded (enemiesManager.enemies);
		coinsManager.CallUpdateCoinsLabel ();
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
				playerManager.TryChangePlayerOrientation (i, (Orientation)iaData[i]);
				enemiesManager.TryChangeEnemiesOrientation (i, (Orientation)iaData[i]);
			}
		}
	}
}
