using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesManager : MonoBehaviour 
{
	public List<Enemy> 			enemies;
	public List<GameObject> 	enemiesPrefabs;
	public Transform			enemiesContainer;

	public void LoadEnemy(EnemyType p_type, int p_posIndex)
	{
		if (enemies == null)
			enemies = new List<Enemy> ();
		
		int __width = GridManager.gridSize.Item1;
		int __x = Mathf.RoundToInt(p_posIndex % __width);
		int __y = p_posIndex / __width;

		GameObject __enemy = (GameObject)Instantiate (enemiesPrefabs [(int)p_type]);
		__enemy.name = "Enemy";
		__enemy.transform.parent = enemiesContainer;
		__enemy.transform.localPosition = new Vector3 ((__x * 2f) - __width + 1f,
			(__y * -2f) + (GridManager.gridSize.Item2 - 1f));
		
		enemies.Add (__enemy.GetComponent<Enemy> ());
		enemies[enemies.Count - 1].gridPos = new TupleInt(__x, __y);
	}
	public void TryChangeEnemiesOrientation(int p_posIndex, Orientation p_orientation)
	{
		TupleInt __pos = TupleInt.IndexToTuple (p_posIndex, GridManager.gridSize);
		foreach (Enemy __en in enemies) 
		{
			if (__en.gridPos.Equals (__pos))
				__en.ChangeOrientationInstantly (p_orientation);
		}
	}
}
