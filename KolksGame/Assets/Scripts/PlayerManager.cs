using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
	public Player 			player;
	public GameObject 		playerPrefab;
	public Transform 		playerContainer;

	public void LoadPlayer(int p_posIndex)
	{
		TupleInt __pos = TupleInt.IndexToTuple (p_posIndex, GridManager.gridSize);
		GameObject __player = (GameObject)Instantiate (playerPrefab);

		__player.name = "Player";
		__player.transform.parent = playerContainer;
		__player.transform.localPosition = new Vector3 (
			(__pos.Item1 *  2f) - (GridManager.gridSize.Item1 - 1f),
			(__pos.Item2 * -2f) + (GridManager.gridSize.Item2 - 1f));
		player = __player.GetComponent<Player> ();
		player.gridPos = __pos;
	}
	public void TryChangePlayerOrientation(int p_posIndex, Orientation p_orientation)
	{
		TupleInt __pos = TupleInt.IndexToTuple (p_posIndex, GridManager.gridSize);
		if (player.gridPos.Equals (__pos)) 
			player.ChangeOrientation (p_orientation);
	}
}
