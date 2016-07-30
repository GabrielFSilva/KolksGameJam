using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour 
{
	public event Action<Tile.PlayerOrientation>	OnScreenClicked;
	public BoxCollider2D	inputCollider;

	public Player	player;

	public void Clicked()
	{
		Vector3 __pos = Input.mousePosition;
		__pos.z = player.transform.position.z;
		float __angle = AngleBetweenVector2 (player.transform.position, Camera.main.ScreenToWorldPoint (__pos));
		if (OnScreenClicked != null) 
		{
			if (__angle > -45f && __angle <= 45f)
				OnScreenClicked (Tile.PlayerOrientation.RIGHT);
			else if (__angle > 45f && __angle <= 135f)
				OnScreenClicked (Tile.PlayerOrientation.UP);
			else if (__angle > 135f || __angle <= -135f)
				OnScreenClicked (Tile.PlayerOrientation.LEFT);
			else if (__angle > -135f && __angle <= -45f)
				OnScreenClicked (Tile.PlayerOrientation.DOWN);
		}
	}
	private float AngleBetweenVector2(Vector2 p_vec1, Vector2 p_vec2)
	{
		Vector2 __difference = p_vec2 - p_vec1;
		float __sign = (p_vec2.y < p_vec1.y) ? -1f : 1f;
		return Vector2.Angle (Vector2.right, __difference) * __sign;
	}
}

