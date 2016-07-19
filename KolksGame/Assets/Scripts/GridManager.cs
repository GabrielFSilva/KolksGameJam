using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour 
{
	public GameSceneManager gameManager;
	public TileLoader		tileLoader;

	public List<GridRow> 	grid;
	public List<int> 		gridInfo;
	public List<int> 		sceneryInfo;

	public int gridWidth;
	public int gridHeight;

	public void LoadTiles(int p_width,int p_height, List<int> p_data)
	{
		gridHeight = p_height;
		gridWidth = p_width;
		gridInfo = new List<int> ();
		foreach (int __int in p_data)
			gridInfo.Add (__int - 1);
		tileLoader.LoadTiles (gridWidth, gridHeight, gridInfo);
	}
	public void LoadScenery(int p_width,int p_height, List<int> p_data)
	{
		sceneryInfo = new List<int> ();
		foreach (int __int in p_data)
			sceneryInfo.Add (__int - 65);
		tileLoader.LoadScenery (gridWidth, gridHeight, sceneryInfo);
	}
	public void LoadTiles()
	{
		//tileLoader.LoadTiles (grid);
	}
	public bool TileIsWithinGrid(int p_posX, int p_posY)
	{
		if (p_posX < 0 || p_posX > gridWidth - 1 || p_posY < 0 || p_posY > gridHeight - 1)
			return false;
		return true;
	}
	public bool TileHasEnemy(int p_posX, int p_posY)
	{
		foreach(Enemy __enemy in gameManager.enemies)
			if ((int)__enemy.gridPosition.x == p_posX && (int)__enemy.gridPosition.y == p_posY)
				return true;
		return false;
	}
	public bool TileHasPlayer(int p_posX, int p_posY)
	{
		if ((int)gameManager.player.gridPosition.x == p_posX && (int)gameManager.player.gridPosition.y == p_posY)
			return true;
		return false;
	}
	public bool TilePassYawn(int p_posX, int p_posY)
	{
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.NOT_WALKABLE_BLOCK_YAWN)
			return false;
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.WALKABLE_BLOCK_YAWN)
			return false;
		return true;
	}
	public bool TileWalkable(int p_posX, int p_posY)
	{
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.NOT_WALKABLE_BLOCK_YAWN)
			return false;
		if (grid [p_posY].columns [p_posX].constraints == Tile.TileConstraints.NOT_WALKABLE_PASS_YAWN)
			return false;
		return true;
	}
}
