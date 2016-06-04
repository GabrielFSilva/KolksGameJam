using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameSceneManager : MonoBehaviour 
{
	public Text	movimentCountLabel;

	public PlayerManager player;
	public int playerMovimentCount;

	//Grid Stuff
	public Transform tilesContainer;
	public GameObject tilePrefab;
	public List<GridRow> grid;
	public List<Sprite> tileFloorSprites;
	public List<Sprite> tileContentSprites;
	public int gridWidth;
	public int gridHeight;



	void Start () 
	{
		LoadTiles ();
		gridHeight = grid.Count;
		gridWidth = grid [0].columns.Count;
		playerMovimentCount = 0;
		player.gameSceneManager = this;
	}
	void Update()
	{
		movimentCountLabel.text = "Moviment \nCount: " + playerMovimentCount.ToString ();
	}
	private void LoadTiles()
	{
		GameObject __tileFloor;
		GameObject __tileContent;
		foreach (GridRow __row in grid) 
		{
			for (int i = 0; i < __row.columns.Count; i++) 
			{
				__tileFloor = (GameObject)Instantiate (tilePrefab);
				__tileFloor.name = "TileFloor";
				__tileFloor.transform.parent = tilesContainer;
				__tileFloor.transform.localPosition = new Vector3 ((i * 2f) - __row.columns.Count + 1f,
					(grid.IndexOf (__row) * -2f) + grid.Count - 1f);
				SpriteRenderer __sr = __tileFloor.GetComponent<SpriteRenderer> ();
				if ((i + grid.IndexOf (__row)) % 2 == 0)
					__sr.sprite = tileFloorSprites [0];
				else
					__sr.sprite = tileFloorSprites [1];

				//Load File Content
				if (__row.columns [i].content != Tile.TileContent.NOTHING) 
				{
					__tileContent = (GameObject)Instantiate (tilePrefab);
					__tileContent.name = __row.columns [i].content.ToString();
					__tileContent.transform.parent = __tileFloor.transform;
					__tileContent.transform.localPosition = Vector3.zero;
					__tileContent.GetComponent<SpriteRenderer> ().sprite = tileContentSprites [(int)__row.columns [i].content - 1];
					__tileContent.GetComponent<SpriteRenderer> ().sortingOrder = 1;
					__tileContent.transform.localRotation = Quaternion.Euler (
						new Vector3 (0f, 0f, (int)__row.columns [i].orientation * 90f));
				}
			}
		}
	}
	public bool GetPathCollision(Vector2 p_position, Tile.PlayerOrientation p_orientation)
	{
		int __posX = Mathf.RoundToInt (p_position.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (p_position.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));

		//Map Limit Block
		if (__posX < 0 || __posX > gridWidth - 1 || __posY < 0 || __posY > gridHeight - 1)
			return false;
		//Tile Block
		if (grid [__posY].columns [__posX].constraints == Tile.TileConstraints.NOT_WALKABLE_BLOCK_YAWN ||
		    grid [__posY].columns [__posX].constraints == Tile.TileConstraints.NOT_WALKABLE_PASS_YAWN)
			return false;
		
		return true;
			
	}
}
