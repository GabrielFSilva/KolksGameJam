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
	public List<Sprite> tileSprites;
	public int gridWidth;
	public int gridHeight;



	void Start () 
	{
		LoadTiles ();
		gridHeight = grid.Count;
		gridWidth = grid [0].indexes.Count;
		playerMovimentCount = 0;
		player.gameSceneManager = this;
	}
	void Update()
	{
		movimentCountLabel.text = "Moviment \nCount: " + playerMovimentCount.ToString ();
	}
	private void LoadTiles()
	{
		GameObject __go;
		foreach (GridRow __row in grid) 
		{
			for (int i = 0; i < __row.indexes.Count; i++) 
			{
				__go = (GameObject)Instantiate (tilePrefab);
				__go.transform.parent = tilesContainer;
				__go.transform.localPosition = new Vector3 ((i * 2f) - __row.indexes.Count + 1f,
					(grid.IndexOf (__row) * -2f) + grid.Count - 1f);
				__go.GetComponent<SpriteRenderer> ().sprite = tileSprites [__row.indexes [i]];
			}
		}
	}
	public bool GetPathCollision(Vector2 p_position, PlayerManager.PlayerOrientation p_orientation)
	{
		int __posX = Mathf.RoundToInt (p_position.x + Mathf.Cos ((int)p_orientation * 90f * Mathf.Deg2Rad));
		int __posY = Mathf.RoundToInt (p_position.y - Mathf.Sin ((int)p_orientation * 90f * Mathf.Deg2Rad));

		if (__posX < 0 || __posX > gridWidth - 1 || __posY < 0 || __posY > gridHeight - 1)
			return false;
		return grid [__posY].walkable [__posX];
			
	}
}
