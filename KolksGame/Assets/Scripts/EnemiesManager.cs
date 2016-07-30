using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesManager : MonoBehaviour 
{

	public List<Enemy> enemies;
	public List<GameObject> 	enemiesPrefabs;
	public Transform			enemiesContainer;

	public void LoadEnemy(Enemy.EnemyType p_type, int p_posIndex)
	{
		if (enemies == null)
			enemies = new List<Enemy> ();
		
		int __width = GridManager.gridWidth;
		int __x = Mathf.RoundToInt(p_posIndex % __width);
		int __y = p_posIndex / __width;

		GameObject __enemy = (GameObject)Instantiate (enemiesPrefabs [(int)p_type]);
		__enemy.name = "Enemy";
		__enemy.transform.parent = enemiesContainer;
		__enemy.transform.localPosition = new Vector3 ((__x * 2f) - __width + 1f,
			(__y * -2f) + (GridManager.gridHeight - 1f));
		
		enemies.Add (__enemy.GetComponent<Enemy> ());
		enemies [enemies.Count - 1].gridPosition = new Vector2(__x, __y);
	}
	public void TryChangeEnemiesOrientation(int p_posIndex, Tile.PlayerOrientation p_orientation)
	{
		int __x = Mathf.RoundToInt(p_posIndex % GridManager.gridWidth);
		int __y = Mathf.RoundToInt(p_posIndex / GridManager.gridWidth);
		foreach (Enemy __en in enemies)
			if (__en.gridPosition.x == __x && __en.gridPosition.y == __y)
				__en.ChangeOrientationInstantly (p_orientation);
	}
}
