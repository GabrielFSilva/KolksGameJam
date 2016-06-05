using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridRow : MonoBehaviour 
{
	public List<Tile> columns;
}

[System.Serializable]
public class Tile
{
	public enum TileContent
	{
		NOTHING,
		WALL,
		CHAIR,
		ARM_CHAIR,
		PLANT_1,
		TABLE_SMALL,
		TABLE_1_LEFT,
		TABLE_1_RIGHT,
		TABLE_2_UP,
		TABLE_2_DOWN,
		BOOKSHELF,
		FILESHELF,
		LEAVES
	}
	public enum PlayerOrientation
	{
		RIGHT,
		UP,
		LEFT,
		DOWN
	}
	public enum TileOrientation
	{
		DOWN,
		RIGHT,
		UP,
		LEFT
	}
	public enum TileConstraints
	{
		WALKABLE_PASS_YAWN,
		WALKABLE_BLOCK_YAWN,
		NOT_WALKABLE_PASS_YAWN,
		NOT_WALKABLE_BLOCK_YAWN
	}

	public TileContent content;
	public TileOrientation orientation;
	public TileConstraints constraints;

}
