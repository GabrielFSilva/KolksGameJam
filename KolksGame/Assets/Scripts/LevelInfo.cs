using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour 
{
	public enum ActionsAvailable
	{
		YAWN,
		YAWN_HELLO,
		YAWN_HELLO_EXCUSE,
	}
	public int movesAvailable;
	public ActionsAvailable actions;
	public PlayerManager player;
	public GameObject grid;
	public GameObject enemiesContainer;
}
