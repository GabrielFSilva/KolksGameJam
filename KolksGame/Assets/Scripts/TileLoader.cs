using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileLoader : MonoBehaviour 
{
	public Transform tilesContainer;
	public Transform sceneryContainer;
	public GameObject tilePrefab;
	public List<Sprite> tileFloorSprites;
	public List<Sprite> tileContentSprites;

	public List<Tile.TileConstraints> 	constraintsReferences;
	public List<SpriteRenderer> 		floorTiles;

	public void LoadTiles(TupleInt p_gridSize, List<int> p_data)
	{
		GameObject __tileFloor;
		for (int i = 0; i < p_data.Count; i ++)
		{
			__tileFloor = (GameObject)Instantiate (tilePrefab);
			__tileFloor.name = "TileFloor";
			__tileFloor.transform.parent = tilesContainer;
			__tileFloor.transform.localPosition = new Vector3 ((i % p_gridSize.Item1 * 2f) - p_gridSize.Item1 + 1f, 
				(i / p_gridSize.Item1 * -2f) + (p_gridSize.Item1) - 1f);

			SpriteRenderer __sr = __tileFloor.GetComponent<SpriteRenderer> ();
			floorTiles.Add (__sr);
			__sr.sortingOrder = -90;
			if (p_data[i] == 0)
				__sr.sprite = tileFloorSprites [0];
			else
				__sr.sprite = tileFloorSprites [1];
		}
	}
	public void LoadScenery(TupleInt p_gridSize, List<int> p_data)
	{
		GameObject __tileContent;
		SpriteRenderer __spriteRenderer;
		for (int i = 0; i < p_data.Count; i ++)
		{
			if (p_data [i] > 0) 
			{
				__tileContent = (GameObject)Instantiate (tilePrefab);
				__tileContent.name = "Content";
				__tileContent.transform.parent = sceneryContainer;
				__tileContent.transform.localPosition = new Vector3 (
					(i % p_gridSize.Item1 * 2f) - p_gridSize.Item1 + 1f, 
					(i / p_gridSize.Item1 * -2f) + (p_gridSize.Item1) - 1f);
				__spriteRenderer = __tileContent.GetComponent<SpriteRenderer> ();
				__spriteRenderer.sprite = tileContentSprites [p_data [i] - 1];
				__spriteRenderer.sortingOrder = Mathf.RoundToInt(((i / p_gridSize.Item1 * -2f) + (p_gridSize.Item1) - 1f) * -10f);
			}
		}
	}
}
