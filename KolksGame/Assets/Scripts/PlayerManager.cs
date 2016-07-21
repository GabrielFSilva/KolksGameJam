using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	public Player 			player;
	public GameObject 		playerPrefab;
	public Transform 		playerContainer;

	public void LoadPlayer(int p_posIndex)
	{
		int __width = GridManager.gridWidth;
		int __x = Mathf.RoundToInt(p_posIndex % __width);
		int __y = Mathf.RoundToInt(p_posIndex / __width);

		GameObject __player = (GameObject)Instantiate (playerPrefab);
		__player.name = "Player";
		__player.transform.parent = playerContainer;
		__player.transform.localPosition = new Vector3 ((__x * 2f) - __width + 1f,
			(__y * -2f) + (GridManager.gridHeight - 1f));
		
		player = __player.GetComponent<Player> ();
		player.gridPosition = new Vector2(__x, __y);
	}
	public void TryChangePlayerOrientation(int p_posIndex, Tile.PlayerOrientation p_orientation)
	{
		int __x = Mathf.RoundToInt(p_posIndex % GridManager.gridWidth);
		int __y = Mathf.RoundToInt(p_posIndex / GridManager.gridWidth);
		if (player.gridPosition.x == __x && player.gridPosition.y == __y)
			player.ChangeOrientation (p_orientation);
	}
}
