using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour 
{
	public GameSceneManager gameManager;
	public TileLoader		tileLoader;

	public List<Tile>	 	tiles;
	public List<int> 		gridInfo;
	public List<int> 		sceneryInfo;

	public static TupleInt 	gridSize;

	public void SetGridDimensions(int p_width,int p_height)
	{
		gridSize = new TupleInt (p_width, p_height);
	}
	public void LoadTiles(List<int> p_data)
	{
		gridInfo = new List<int> ();
		foreach (int __int in p_data)
			gridInfo.Add (__int - 1);
		tileLoader.LoadTiles (gridSize, gridInfo);
	}
	public void LoadScenery(List<int> p_data)
	{
		sceneryInfo = new List<int> ();
		foreach (int __int in p_data)
			sceneryInfo.Add (__int - 65);
		tileLoader.LoadScenery (gridSize, sceneryInfo);

		tiles = new List<Tile> ();
		for (int i = 0; i < gridInfo.Count; i++) 
		{
			tiles.Add (new Tile ());
			if (sceneryInfo [i] > 0) 
			{
				tiles [i].constraints = tileLoader.constraintsReferences [sceneryInfo [i]];
				tiles [i].content = (Tile.TileContent)sceneryInfo [i];
			}
		}
	}
	public bool TileIsWithinGrid(TupleInt p_pos)
	{
		if (p_pos.Item1 < 0 || p_pos.Item1 > gridSize.Item1 - 1 || 
			p_pos.Item2 < 0 || p_pos.Item2 > gridSize.Item2 - 1)
			return false;
		return true;
	}
	public bool TileHasEnemy(TupleInt p_pos)
	{
		foreach(Enemy __enemy in gameManager.enemies)
			if (__enemy.gridPos.Equals(p_pos))
				return true;
		return false;
	}
	public bool TileHasPlayer(TupleInt p_pos)
	{
		if (gameManager.player.gridPos.Equals(p_pos))
			return true;
		return false;
	}
	public bool TilePassYawn(TupleInt p_pos)
	{
		if (tiles[p_pos.ToIndex(gridSize)].constraints == Tile.TileConstraints.NOT_WALKABLE_BLOCK_YAWN)
			return false;
		if (tiles[p_pos.ToIndex(gridSize)].constraints == Tile.TileConstraints.WALKABLE_BLOCK_YAWN)
			return false;
		return true;
	}
	public bool TileWalkable(TupleInt p_pos)
	{
		if (tiles[p_pos.ToIndex(gridSize)].constraints == Tile.TileConstraints.NOT_WALKABLE_BLOCK_YAWN)
			return false;
		if (tiles[p_pos.ToIndex(gridSize)].constraints == Tile.TileConstraints.NOT_WALKABLE_PASS_YAWN)
			return false;
		return true;
	}
}
