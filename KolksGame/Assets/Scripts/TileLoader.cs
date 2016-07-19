using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileLoader : MonoBehaviour 
{
	public Transform tilesContainer;
	public GameObject tilePrefab;
	public List<Sprite> tileFloorSprites;
	public List<Sprite> tileContentSprites;

	public void LoadTiles(int p_width,int p_height, List<int> p_data)
	{
		GameObject __tileFloor;
		for (int i = 0; i < p_data.Count; i ++)
		{
			__tileFloor = (GameObject)Instantiate (tilePrefab);
			__tileFloor.name = "TileFloor";
			__tileFloor.transform.parent = tilesContainer;
			__tileFloor.transform.localPosition = new Vector3 ((i % p_width * 2f) - p_width + 1f, 
				(i / p_width * -2f) + (p_width) - 1f);

			SpriteRenderer __sr = __tileFloor.GetComponent<SpriteRenderer> ();
			if (p_data[i] == 0)
				__sr.sprite = tileFloorSprites [0];
			else
				__sr.sprite = tileFloorSprites [1];
		}
	}
	public void LoadScenery(int p_width,int p_height, List<int> p_data)
	{
		GameObject __tileContent;
		for (int i = 0; i < p_data.Count; i ++)
		{
			if (p_data [i] >= 0) 
			{
				__tileContent = (GameObject)Instantiate (tilePrefab);
				__tileContent.name = "Content";
				__tileContent.transform.parent = tilesContainer;
				__tileContent.transform.localPosition = new Vector3 ((i % p_width * 2f) - p_width + 1f, 
					(i / p_width * -2f) + (p_width) - 1f);
				__tileContent.GetComponent<SpriteRenderer> ().sprite = tileContentSprites [p_data [i] - 1];
				__tileContent.GetComponent<SpriteRenderer> ().sortingOrder = 1;
			}
		}
	}
}
